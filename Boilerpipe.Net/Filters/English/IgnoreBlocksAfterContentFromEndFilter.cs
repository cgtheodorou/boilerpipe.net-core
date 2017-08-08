namespace Boilerpipe.Net.Filters.English {
  using System.Collections.Generic;

  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Labels;

  /// <summary>
  ///   Marks all blocks as "non-content" that occur after blocks that have been
  ///   marked <see cref="DefaultLabels.INDICATES_END_OF_TEXT"/>, and after any content block.
  ///   This filter can be used in conjunction with an upstream <see cref="TerminatingBlocksFinder"/>.
  /// </summary>
  /// <seealso cref="TerminatingBlocksFinder" />
  public sealed class IgnoreBlocksAfterContentFromEndFilter : HeuristicFilterBase, IBoilerpipeFilter {
    /// <summary>
    ///   The singleton instance for <see cref="IgnoreBlocksAfterContentFromEndFilter" />.
    /// </summary>
    public static readonly IgnoreBlocksAfterContentFromEndFilter Instance = new IgnoreBlocksAfterContentFromEndFilter();

    private IgnoreBlocksAfterContentFromEndFilter() {
    }

    public bool Process(TextDocument doc) {
      bool changes = false;
      int words = 0;

      List<TextBlock> blocks = doc.TextBlocks;
      if (blocks.Count != 0) {
        blocks.Reverse();

        foreach (TextBlock tb in blocks) {
          if (tb.HasLabel(DefaultLabels.INDICATES_END_OF_TEXT)) {
            tb.AddLabel(DefaultLabels.STRICTLY_NOT_CONTENT);
            tb.RemoveLabel(DefaultLabels.MIGHT_BE_CONTENT);
            tb.IsContent = false;
            changes = true;
          } else if (tb.IsContent) {
            words += tb.NumWords;
            if (words > 200) {
              break;
            }
          }
        }
      }

      return changes;
    }
  }
}
