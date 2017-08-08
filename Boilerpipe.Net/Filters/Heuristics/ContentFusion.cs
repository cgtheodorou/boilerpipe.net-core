namespace Boilerpipe.Net.Filters.Heuristics {
  using System.Collections.Generic;
  using System.Linq;

  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Labels;

  public sealed class ContentFusion : IBoilerpipeFilter {
    /// <summary>
    ///   The singleton instance for <see cref="ContentFusion" />.
    /// </summary>
    public static readonly ContentFusion Instance = new ContentFusion();

    public bool Process(TextDocument doc) {
      List<TextBlock> textBlocks = doc.TextBlocks;
      if (textBlocks.Count < 2) {
        return false;
      }

      TextBlock prevBlock = textBlocks[0];

      bool changes;
      do {
        changes = false;
        foreach (TextBlock block in textBlocks.Skip(1).ToList()) {
          if (prevBlock.IsContent && block.LinkDensity < 0.56 && !block.HasLabel(DefaultLabels.STRICTLY_NOT_CONTENT)) {
            prevBlock.MergeNext(block);
            textBlocks.Remove(block);
            changes = true;
          } else {
            prevBlock = block;
          }
        }
      }
      while (changes);

      return true;
    }
  }
}
