namespace Boilerpipe.Net.Filters.English {
  using Boilerpipe.Net.Document;

  /// <summary>
  ///   Base class for some heuristics that are used by boilerpipe filters.
  /// </summary>
  public abstract class HeuristicFilterBase {
    protected static int GetNumFullTextWords(TextBlock tb) {
      return GetNumFullTextWords(tb, 9);
    }

    protected static int GetNumFullTextWords(TextBlock tb, float minTextDensity) {
      if (tb.TextDensity >= minTextDensity) {
        return tb.NumWords;
      }
      return 0;
    }
  }
}
