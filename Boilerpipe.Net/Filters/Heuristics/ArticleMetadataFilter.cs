namespace Boilerpipe.Net.Filters.Heuristics {
  using System.Text.RegularExpressions;

  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Labels;

  /// <summary>
  ///   Article metadata filter.
  /// </summary>
  public class ArticleMetadataFilter : IBoilerpipeFilter {
    private static readonly Regex[] PatternsShort = {
      new Regex("^[0-9 \\,\\./]*\\b(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|January|February|March|April|May|June|July|August|September|October|November|December)?\\b[0-9 \\,\\:apm\\./]*([CPSDMGET]{2,3})?$", RegexOptions.Compiled),
      new Regex("^[Bb]y ", RegexOptions.Compiled)
    };

    /// <summary>
    ///   The singleton instance for <see cref="ArticleMetadataFilter" />.
    /// </summary>
    public static readonly ArticleMetadataFilter Instance = new ArticleMetadataFilter();

    private ArticleMetadataFilter() {
    }

    public bool Process(TextDocument doc) {
      bool changed = false;
      foreach (TextBlock tb in doc.TextBlocks) {
        if (tb.NumWords > 10) {
          continue;
        }
        string text = tb.Text;
        foreach (Regex regex in PatternsShort) {
          if (regex.IsMatch(text)) {
            changed = true;
            tb.IsContent = true;
            tb.AddLabel(DefaultLabels.ARTICLE_METADATA);
          }
        }
      }
      return changed;
    }
  }
}
