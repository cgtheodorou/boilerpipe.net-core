namespace Boilerpipe.Net.Filters.English {
  using System.Collections.Generic;

  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Filters.Heuristics;
  using Boilerpipe.Net.Labels;

  /// <summary>
  ///   <para>
  ///     Keeps the largest <see cref="TextBlock" /> only (by the number of words). In case of
  ///     more than one block with the same number of words, the first block is chosen.
  ///     All discarded blocks are marked "not content" and flagged as <see cref="DefaultLabels.MIGHT_BE_CONTENT" />
  ///   </para>
  ///   <para>
  ///     *
  ///     As opposed to <see cref="KeepLargestBlockFilter" />, the number of words are
  ///     computed using <see cref="HeuristicFilterBase.GetNumFullTextWords(TextBlock)" />, which only counts
  ///     words that occur in text elements with at least 9 words and are thus believed to be full text.
  ///   </para>
  ///   <para>
  ///     NOTE: Without language-specific fine-tuning (i.e., running the default instance), this filter
  ///     may lead to suboptimal results. You better use <see cref="KeepLargestBlockFilter" /> instead, which
  ///     works at the level of number-of-words instead of text densities.
  ///   </para>
  /// </summary>
  public sealed class KeepLargestFulltextBlockFilter : HeuristicFilterBase, IBoilerpipeFilter {
    /// <summary>
    ///   The singleton instance for <see cref="KeepLargestFulltextBlockFilter" />.
    /// </summary>
    public static readonly KeepLargestFulltextBlockFilter Instance = new KeepLargestFulltextBlockFilter();

    public bool Process(TextDocument doc) {
      List<TextBlock> textBlocks = doc.TextBlocks;
      if (textBlocks.Count < 2) {
        return false;
      }

      int max = -1;
      TextBlock largestBlock = null;;
      foreach (TextBlock tb in textBlocks) {
        if (!tb.IsContent) {
          continue;
        }
        int numWords = GetNumFullTextWords(tb);
        if (numWords > max) {
          largestBlock = tb;
          max = numWords;
        }
      }

      if (largestBlock == null) {
        return false;
      }

      foreach (TextBlock tb in textBlocks) {
        if (tb == largestBlock) {
          tb.IsContent = true;
        } else {
          tb.IsContent = false;
          tb.AddLabel(DefaultLabels.MIGHT_BE_CONTENT);
        }
      }

      return true;
    }
  }
}
