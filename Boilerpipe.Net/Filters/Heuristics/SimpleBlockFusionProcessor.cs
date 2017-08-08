namespace Boilerpipe.Net.Filters.Heuristics {
  using System.Collections.Generic;
  using System.Linq;

  using Boilerpipe.Net.Document;

  /// <summary>
  ///   Merges two subsequent blocks if their text densities are equal.
  /// </summary>
  public class SimpleBlockFusionProcessor : IBoilerpipeFilter {
    /// <summary>
    ///   The singleton instance for <see cref="SimpleBlockFusionProcessor" />
    /// </summary>
    public static readonly SimpleBlockFusionProcessor Instance = new SimpleBlockFusionProcessor();

    public bool Process(TextDocument doc) {
      List<TextBlock> textBlocks = doc.TextBlocks;
      bool changes = false;

      if (textBlocks.Count < 2) {
        return false;
      }

      TextBlock b1 = textBlocks[0];
      foreach (TextBlock b2 in textBlocks.Skip(1).ToList()) {
        bool similar = (b1.TextDensity == b2.TextDensity);

        if (similar) {
          b1.MergeNext(b2);
          textBlocks.Remove(b2);
          changes = true;
        } else {
          b1 = b2;
        }
      }

      return changes;
    }
  }
}
