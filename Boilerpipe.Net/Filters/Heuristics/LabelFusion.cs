namespace Boilerpipe.Net.Filters.Heuristics {
  using System.Collections.Generic;
  using System.Linq;

  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Labels;

  /// <summary>
  ///   Fuses adjacent blocks if their labels are equal.
  /// </summary>
  public sealed class LabelFusion : IBoilerpipeFilter {
    /// <summary>
    ///   The singleton instance for <see cref="LabelFusion" />
    /// </summary>
    public static readonly LabelFusion Instance = new LabelFusion("");

    private readonly string _labelPrefix;

    /// <summary>
    ///   Creates a new <see cref="LabelFusion" /> instance.
    /// </summary>
    /// <param name="labelPrefix"></param>
    public LabelFusion(string labelPrefix) {
      _labelPrefix = labelPrefix;
    }

    public bool Process(TextDocument doc) {
      List<TextBlock> textBlocks = doc.TextBlocks;
      if (textBlocks.Count < 2) {
        return false;
      }

      bool changes = false;
      TextBlock prevBlock = textBlocks[0];
      const int offset = 1;

      foreach (TextBlock block in textBlocks.Skip(offset).ToList()) {
        if (EqualLabels(prevBlock.Labels, block.Labels)) {
          prevBlock.MergeNext(block);
          textBlocks.Remove(block);
          changes = true;
        } else {
          prevBlock = block;
        }
      }

      return changes;
    }

    private bool EqualLabels(IEnumerable<string> labels, IEnumerable<string> labels2) {
      if (labels == null || labels2 == null) {
        return false;
      }
      return MarkupLabelsOnly(labels).Equals(MarkupLabelsOnly(labels2));
    }

    private static IEnumerable<string> MarkupLabelsOnly(IEnumerable<string> set1) {
      return set1.Where(x => x.StartsWith(DefaultLabels.MARKUP_PREFIX));
      //List<string> set = new List<string>(set1);
      //foreach (string it in set.ToList().Where(it => !it.StartsWith(DefaultLabels.MARKUP_PREFIX))) {
      //  set.Remove(it);
      //}
      //return set;
    }
  }
}
