namespace Boilerpipe.Net.Filters.Heuristics {
  using System.Collections.Generic;
  using System.Linq;

  using Boilerpipe.Net.Document;

  /// <summary>
  ///   Adds the labels of the preceding block to the current block, optionally adding a prefix.
  /// </summary>
  public sealed class AddPrecedingLabelsFilter : IBoilerpipeFilter {
    /// <summary>
    ///   The singleton instance for <see cref="AddPrecedingLabelsFilter" /> with no prefix.
    /// </summary>
    public static readonly AddPrecedingLabelsFilter Instance = new AddPrecedingLabelsFilter("");

    /// <summary>
    ///   The singleton instance for <see cref="AddPrecedingLabelsFilter" /> with ^ as prefix.
    /// </summary>
    public static readonly AddPrecedingLabelsFilter InstancePre = new AddPrecedingLabelsFilter("^");

    private readonly string _labelPrefix;

    /// <summary>
    ///   Creates a new <see cref="AddPrecedingLabelsFilter" /> instance.
    /// </summary>
    /// <param name="labelPrefix">The label prefix.</param>
    public AddPrecedingLabelsFilter(string labelPrefix) {
      _labelPrefix = labelPrefix;
    }

    public bool Process(TextDocument doc) {
      List<TextBlock> textBlocks = doc.TextBlocks;
      if (textBlocks.Count < 2) {
        return false;
      }

      bool changes = false;
      TextBlock blockBelow = null;

      textBlocks.Reverse();
      foreach (TextBlock block in textBlocks) {
        if (blockBelow == null) {
          blockBelow = block;
          continue;
        }

        IEnumerable<string> labels = block.Labels;
        if (labels != null) {
          IList<string> enumerable = labels.ToList();
          if (enumerable.Count > 0) {
            foreach (string l in enumerable) {
              blockBelow.AddLabel(_labelPrefix + l);
            }
            changes = true;
          }
        }
        blockBelow = block;
      }

      return changes;
    }
  }
}
