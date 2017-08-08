namespace Boilerpipe.Net.Filters.Simple {
  using System.Linq;

  using Boilerpipe.Net.Document;

  /// <summary>
  ///   Marks all blocks that contain a given label as "content".
  /// </summary>
  public sealed class LabelToContentFilter : IBoilerpipeFilter {
    private readonly string[] _labels;

    /// <summary>
    ///   Creates a new instance of <see cref="LabelToContentFilter" />
    /// </summary>
    /// <param name="label"></param>
    public LabelToContentFilter(params string[] label) {
      _labels = label;
    }

    public bool Process(TextDocument doc) {
      bool changes = false;

      foreach (TextBlock tb in doc.TextBlocks.Where(tb => !tb.IsContent && _labels.Any(tb.HasLabel))) {
        tb.IsContent = true;
        changes = true;
      }

      return changes;
    }
  }
}
