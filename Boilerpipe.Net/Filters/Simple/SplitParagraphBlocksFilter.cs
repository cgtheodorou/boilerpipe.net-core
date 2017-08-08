namespace Boilerpipe.Net.Filters.Simple {
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text.RegularExpressions;

  using Boilerpipe.Net.Document;

  /// <summary>
  ///   Splits TextBlocks at paragraph boundaries.
  /// </summary>
  /// <remarks>
  ///   NOTE: This is not fully supported (i.e., it will break highlighting support
  ///   via <see cref="TextBlock.ContainedTextElements"/>), but this one probably is necessary for some other
  ///   filters.
  /// </remarks>
  /// <seealso cref="MinClauseWordsFilter" />
  public sealed class SplitParagraphBlocksFilter : IBoilerpipeFilter {
    /// <summary>
    ///   Returns the singleton instance for <see cref="SplitParagraphBlocksFilter" />
    /// </summary>
    public static readonly SplitParagraphBlocksFilter Instance = new SplitParagraphBlocksFilter();

    public bool Process(TextDocument doc) {
      bool changes = false;

      List<TextBlock> blocks = doc.TextBlocks;
      var blocksNew = new List<TextBlock>();

      foreach (TextBlock tb in blocks) {
        string text = tb.Text;
        string[] paragraphs = Regex.Split(text, "[\n\r]+");
        if (paragraphs.Length < 2) {
          blocksNew.Add(tb);
          continue;
        }
        bool isContent = tb.IsContent;
        List<string> labels = (tb.Labels ?? Enumerable.Empty<string>()).ToList();
        foreach (String p in paragraphs) {
          var tbP = new TextBlock(p) { IsContent = isContent };
          tbP.AddLabels(labels);
          blocksNew.Add(tbP);
          changes = true;
        }
      }

      if (changes) {
        blocks.Clear();
        blocks.AddRange(blocksNew);
      }

      return changes;
    }
  }
}
