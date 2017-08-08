namespace Boilerpipe.Net.Filters.Simple {
  using System.Linq;

  using Boilerpipe.Net.Document;

  /// <summary>
  /// Keeps only those content blocks which contain at least <c>k</c> words.
  /// </summary>
  public sealed class MinWordsFilter : IBoilerpipeFilter {
    private readonly int _minWords;

    /// <summary>
    /// Creates a new instance of <see cref="MinWordsFilter"/>
    /// </summary>
    /// <param name="minWords">the number of words</param>
    public MinWordsFilter(int minWords) {
      _minWords = minWords;
    }

    public bool Process(TextDocument doc) {
      bool changes = false;

      foreach (TextBlock tb in doc.TextBlocks.Where(tb => tb.IsContent && tb.NumWords < _minWords)) {
        tb.IsContent = false;
        changes = true;
      }

      return changes;
    }
  }
}
