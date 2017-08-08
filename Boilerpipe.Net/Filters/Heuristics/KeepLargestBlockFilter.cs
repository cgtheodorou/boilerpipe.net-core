namespace Boilerpipe.Net.Filters.Heuristics {
  using System.Collections.Generic;
  using System.Linq;

  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Labels;

  /// <summary>
  ///   Keeps the largest <see cref="TextBlock" /> only (by the number of words). In case of
  ///   more than one block with the same number of words, the first block is chosen.
  ///   All discarded blocks are marked "not content" and flagged as
  ///   <see cref="DefaultLabels.MIGHT_BE_CONTENT" />.
  /// </summary>
  /// <remarks>
  ///   Note that, by default, only TextBlocks marked as "content" are taken into consideration.
  /// </remarks>
  public sealed class KeepLargestBlockFilter : IBoilerpipeFilter {
    /// <summary>
    ///   The singleton instance for <see cref="KeepLargestBlockFilter" />
    /// </summary>
    public static readonly KeepLargestBlockFilter Instance = new KeepLargestBlockFilter(false);

    /// <summary>
    ///   The singleton instance for <see cref="KeepLargestBlockFilter" />
    /// </summary>
    public static readonly KeepLargestBlockFilter InstanceExpandToSameTaglevel = new KeepLargestBlockFilter(true);

    private readonly bool _expandToSameLevelText;

    /// <summary>
    ///   Creates a new instance of <see cref="KeepLargestBlockFilter" />
    /// </summary>
    /// <param name="expandToSameLevelText"></param>
    public KeepLargestBlockFilter(bool expandToSameLevelText) {
      _expandToSameLevelText = expandToSameLevelText;
    }

    public bool Process(TextDocument doc) {
      List<TextBlock> textBlocks = doc.TextBlocks;
      if (textBlocks.Count < 2) {
        return false;
      }

      int maxNumWords = -1;
      TextBlock largestBlock = null;

      int level = -1;

      int i = 0;
      int n = -1;
      foreach (TextBlock tb in textBlocks) {
        if (tb.IsContent) {
          int nw = tb.NumWords;
          if (nw > maxNumWords) {
            largestBlock = tb;
            maxNumWords = nw;

            n = i;

            if (_expandToSameLevelText) {
              level = tb.TagLevel;
            }
          }
        }
        i++;
      }
      foreach (TextBlock tb in textBlocks) {
        if (tb == largestBlock) {
          tb.IsContent = true;
        } else {
          tb.IsContent = false;
          tb.AddLabel(DefaultLabels.MIGHT_BE_CONTENT);
        }
      }
      if (_expandToSameLevelText && n != -1) {
        foreach (TextBlock tb in textBlocks.Skip(n).Reverse()) {
          int tl = tb.TagLevel;
          if (tl < level) {
            break;
          }
          if (tl == level) {
            tb.IsContent = true;
          }
        }
        foreach (TextBlock tb in textBlocks.Skip(n)) {
          int tl = tb.TagLevel;
          if (tl < level) {
            break;
          }
          if (tl == level) {
            tb.IsContent = true;
          }
        }
      }

      return true;
    }
  }
}
