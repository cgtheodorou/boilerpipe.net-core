namespace Boilerpipe.Net.Extractors {
  using System.Collections.Generic;

  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Estimators;

  /// <summary>
  ///   A full-text extractor trained on <a href="http://krdwrd.org/">krdwrd</a>
  ///   <a href="https://krdwrd.org/trac/attachment/wiki/Corpora/Canola/CANOLA.pdf">Canola</a>.
  ///   Works well with <see cref="SimpleEstimator" />, too.
  /// </summary>
  public class CanolaExtractor : BaseExtractor {
    /// <summary>
    ///   The singleton instance for <see cref="CanolaExtractor" />
    /// </summary>
    public static readonly CanolaExtractor Instance = new CanolaExtractor();

    /// <summary>
    ///   The actual classifier, exposed.
    /// </summary>
    public static readonly IBoilerpipeFilter Classifier = new ClassifierBoilerpipeFilter();

    public override bool Process(TextDocument doc) {
      return Classifier.Process(doc);
    }

    private class ClassifierBoilerpipeFilter : IBoilerpipeFilter {
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

      private static bool Classify(TextBlock prev, TextBlock curr, TextBlock next) {
        bool isContent = (curr.LinkDensity > 0 && next.NumWords > 11)
                         || (curr.NumWords > 19
                             || (next.NumWords > 6 && next.LinkDensity == 0 && prev.LinkDensity == 0
                                 && (curr.NumWords > 6 || prev.NumWords > 7 || next.NumWords > 19)));

        if (curr.IsContent != isContent) {
          curr.IsContent = isContent;
          return true;
        }
        return false;
      }
    };
  }
}
