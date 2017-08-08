namespace Boilerpipe.Net.Filters.Heuristics {
  using System;
  using System.Collections.Generic;
  using System.Text.RegularExpressions;

  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Labels;

  /// <summary>
  ///  Marks <see cref="TextBlock"/>s which contain parts of the HTML
  ///  <code>&lt;TITLE&gt;</code> tag, using some heuristics which are quite
  ///  specific to the news domain.
  /// </summary>
  public sealed class DocumentTitleMatchClassifier : IBoilerpipeFilter {
    private readonly ISet<string> _potentialTitles;

    /// <summary>
    /// Creates a new instance of <see cref="DocumentTitleMatchClassifier"/>
    /// </summary>
    /// <param name="title">the title to match.</param>
    public DocumentTitleMatchClassifier(string title) {
      if (title == null) {
        _potentialTitles = null;
      } else {
        title = title.Trim();
        if (title.Length == 0) {
          _potentialTitles = null;
        } else {
          _potentialTitles = new HashSet<String> { title };

          string p = GetLongestPart(title, "[ ]*[\\|»|:][ ]*");
          if (p != null) {
            _potentialTitles.Add(p);
          }
          p = GetLongestPart(title, "[ ]*[\\|»|:\\(\\)][ ]*");
          if (p != null) {
            _potentialTitles.Add(p);
          }
          p = GetLongestPart(title, "[ ]*[\\|»|:\\(\\)\\-][ ]*");
          if (p != null) {
            _potentialTitles.Add(p);
          }
          p = GetLongestPart(title, "[ ]*[\\|»|,|:\\(\\)\\-][ ]*");
          if (p != null) {
            _potentialTitles.Add(p);
          }
        }
      }
    }

    /// <summary>
    /// Returns the potential titles
    /// </summary>
    public IEnumerable<String> PotentialTitles {
      get {
        return _potentialTitles;
      }
    }

    public bool Process(TextDocument doc) {
      if (_potentialTitles == null) {
        return false;
      }
      bool changes = false;

      foreach (TextBlock tb in doc.TextBlocks) {
        string text = tb.Text.Trim();
        foreach (string candidate in _potentialTitles) {
          if (candidate.Equals(text)) {
            tb.AddLabel(DefaultLabels.TITLE);
            changes = true;
          }
        }
      }

      return changes;
    }

    private String GetLongestPart(string title, string pattern) {
      String[] parts = Regex.Split(title, pattern);
      if (parts.Length == 1) {
        return null;
      }
      int longestNumWords = 0;
      String longestPart = "";
      foreach (string p in parts) {
        if (p.Contains(".com")) {
          continue;
        }
        int numWords = Regex.Split(p, "[\b]+").Length;
        if (numWords > longestNumWords || p.Length > longestPart.Length) {
          longestNumWords = numWords;
          longestPart = p;
        }
      }
      if (longestPart.Length == 0) {
        return null;
      }
      return longestPart.Trim();
    }
  }
}
