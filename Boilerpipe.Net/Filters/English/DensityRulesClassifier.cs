namespace Boilerpipe.Net.Filters.English {
  using System.Collections.Generic;

  using Boilerpipe.Net.Document;

  /// <summary>
  ///   Classifies <see cref="TextBlock"/>s as content/not-content through rules that have
  ///   been determined using the C4.8 machine learning algorithm, as described in the
  ///   paper "Boilerplate Detection using Shallow Text Features", particularly using
  ///   text densities and link densities.
  /// </summary>
  public class DensityRulesClassifier : IBoilerpipeFilter {
    /// <summary>
    ///   The singleton instance for <see cref="DensityRulesClassifier" />
    /// </summary>
    public static readonly DensityRulesClassifier Instance = new DensityRulesClassifier();

    public bool Process(TextDocument doc) {
      List<TextBlock> textBlocks = doc.TextBlocks;
      bool hasChanges = false;

      IEnumerator<TextBlock> it = textBlocks.GetEnumerator();
      if (!it.MoveNext()) {
        return false;
      }
      TextBlock prevBlock = TextBlock.EmptyStart;
      TextBlock currentBlock = it.Current;
      TextBlock nextBlock = it.MoveNext() ? it.Current : TextBlock.EmptyStart;

      hasChanges = Classify(prevBlock, currentBlock, nextBlock) | hasChanges;

      if (nextBlock != TextBlock.EmptyStart) {
        while (it.MoveNext()) {
          prevBlock = currentBlock;
          currentBlock = nextBlock;
          nextBlock = it.Current;
          hasChanges = Classify(prevBlock, currentBlock, nextBlock) | hasChanges;
        }
        prevBlock = currentBlock;
        currentBlock = nextBlock;
        nextBlock = TextBlock.EmptyStart;
        hasChanges = Classify(prevBlock, currentBlock, nextBlock) | hasChanges;
      }

      return hasChanges;
    }

    protected bool Classify(TextBlock prev, TextBlock curr, TextBlock next) {
      bool isContent;

      if (curr.LinkDensity <= 0.333333) {
        if (prev.LinkDensity <= 0.555556) {
          if (curr.TextDensity <= 9) {
            if (next.TextDensity <= 10) {
              if (prev.TextDensity <= 4) {
                isContent = false;
              } else {
                isContent = true;
              }
            } else {
              isContent = true;
            }
          } else {
            if (next.TextDensity == 0) {
              isContent = false;
            } else {
              isContent = true;
            }
          }
        } else {
          if (next.TextDensity <= 11) {
            isContent = false;
          } else {
            isContent = true;
          }
        }
      } else {
        isContent = false;
      }

      if (curr.IsContent != isContent) {
        curr.IsContent = isContent;
        return true;
      }
      return false;
    }
  }
}
