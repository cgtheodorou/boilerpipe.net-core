namespace Boilerpipe.Net.Filters.English {
  using Boilerpipe.Net.Document;

  /// <summary>
  ///   Keeps only those content blocks which contain at least k full-text words
  ///   (measured by <see cref="HeuristicFilterBase.GetNumFullTextWords(TextBlock)" />. k is 30 by default.
  /// </summary>
  public sealed class MinFulltextWordsFilter : HeuristicFilterBase, IBoilerpipeFilter {
    /// <summary>
    ///   The singleton instance for <see cref="MinFulltextWordsFilter" /> with 30 words.
    /// </summary>
    public static readonly MinFulltextWordsFilter DefaultInstance = new MinFulltextWordsFilter(30);

    private readonly int _minWords;

    /// <summary>
    ///   Creates a new instance of <see cref="MinFulltextWordsFilter" />
    /// </summary>
    /// <param name="minWords">The min number of words.</param>
    public MinFulltextWordsFilter(int minWords) {
      _minWords = minWords;
    }

    public bool Process(TextDocument doc) {
      bool changes = false;

      foreach (TextBlock tb in doc.TextBlocks) {
        if (!tb.IsContent) {
          continue;
        }
        if (GetNumFullTextWords(tb) < _minWords) {
          tb.IsContent = false;
          changes = true;
        }
      }

      return changes;
    }
  }
}
