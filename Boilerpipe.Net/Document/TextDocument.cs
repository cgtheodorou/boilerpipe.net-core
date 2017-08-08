namespace Boilerpipe.Net.Document {
  using System;
  using System.Collections.Generic;
  using System.Text;

  /// <summary>
  ///   A text document, consisting of one or more <see cref="TextBlock" />s.
  /// </summary>
  public class TextDocument {
    private readonly List<TextBlock> _textBlocks;
    private readonly string _title;

    /// <summary>
    ///   Creates a new <see cref="TextDocument" /> with given <see cref="TextBlock" />s, and no title.
    /// </summary>
    /// <param name="textBlocks">The text blocks of this document</param>
    public TextDocument(List<TextBlock> textBlocks)
      : this(null, textBlocks) {
    }

    /// <summary>
    ///   Creates a new <see cref="TextDocument" /> with given <see cref="TextBlock" />s and given title.
    /// </summary>
    /// <param name="title">The "main" title for this text document.</param>
    /// <param name="textBlocks">The text blocks of this document</param>
    public TextDocument(string title, List<TextBlock> textBlocks) {
      _title = title;
      _textBlocks = textBlocks;
    }

    /// <summary>
    ///   A list of <see cref="TextBlock"/>s, in sequential order of appearance.
    /// </summary>
    public List<TextBlock> TextBlocks {
      get {
        return _textBlocks;
      }
    }

    /// <summary>
    ///   Returns the "main" title for this document, or <code>null</code> if no such title has ben set.
    /// </summary>
    public string Title {
      get {
        return _title;
      }
    }

    /// <summary>
    ///   Returns the <see cref="TextDocument" />'s content.
    /// </summary>
    public string Content {
      get {
        return GetText(true, false);
      }
    }

    /// <summary>
    ///   Returns the <see cref="TextDocument" />'s content, non-content or both
    /// </summary>
    /// <param name="includeContent">Whether to include TextBlocks marked as "content".</param>
    /// <param name="includeNonContent">Whether to include TextBlocks marked as "non-content".</param>
    /// <returns>The text.</returns>
    public String GetText(bool includeContent, bool includeNonContent) {
      var sb = new StringBuilder();
      foreach (TextBlock block in TextBlocks) {
        if (block.IsContent) {
          if (!includeContent) {
            continue;
          }
        } else {
          if (!includeNonContent) {
            continue;
          }
        }
        sb.Append(block.Text);
        sb.Append('\n');
      }
      return sb.ToString();
    }

    /// <summary>
    ///   Returns detailed debugging information about the contained <see cref="TextBlock" />s.
    /// </summary>
    /// <returns>Debug information</returns>
    public String DebugString() {
      var sb = new StringBuilder();
      foreach (TextBlock tb in TextBlocks) {
        sb.Append(tb);
        sb.Append('\n');
      }
      return sb.ToString();
    }
  }
}
