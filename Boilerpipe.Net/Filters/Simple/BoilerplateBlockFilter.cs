namespace Boilerpipe.Net.Filters.Simple {
  using System.Collections.Generic;
  using System.Linq;

  using Boilerpipe.Net.Document;

  /// <summary>
  ///   Removes <see cref="TextBlock" />s which have explicitly been marked as "not content".
  /// </summary>
  public sealed class BoilerplateBlockFilter : IBoilerpipeFilter {
    /// <summary>
    ///   Returns the singleton instance for <see cref="BoilerplateBlockFilter" />
    /// </summary>
    public static readonly BoilerplateBlockFilter Instance = new BoilerplateBlockFilter();

    public bool Process(TextDocument doc) {
      List<TextBlock> textBlocks = doc.TextBlocks;
      bool hasChanges = false;

      foreach (TextBlock textBlock in textBlocks.ToList().Where(textBlock => !textBlock.IsContent)) {
        textBlocks.Remove(textBlock);
        hasChanges = true;
      }

      return hasChanges;
    }
  }
}
