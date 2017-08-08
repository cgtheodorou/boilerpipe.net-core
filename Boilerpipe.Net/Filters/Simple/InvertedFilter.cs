namespace Boilerpipe.Net.Filters.Simple {
  using System.Collections.Generic;

  using Boilerpipe.Net.Document;

  /// <summary>
  ///   Reverts the "isContent" flag for all <see cref="TextBlock" />s
  /// </summary>
  public sealed class InvertedFilter : IBoilerpipeFilter {
    /// <summary>
    ///   Returns the singleton instance for <see cref="InvertedFilter" />
    /// </summary>
    public static readonly InvertedFilter Instance = new InvertedFilter();

    private InvertedFilter() {
    }

    public bool Process(TextDocument doc) {
      List<TextBlock> tbs = doc.TextBlocks;
      if (tbs.Count == 0) {
        return false;
      }
      foreach (TextBlock tb in tbs) {
        tb.IsContent = !tb.IsContent;
      }

      return true;
    }
  }
}
