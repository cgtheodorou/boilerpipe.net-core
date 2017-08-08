namespace Boilerpipe.Net.Filters.Simple {
  using System.Linq;

  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Labels;

  /// <summary>
  ///   Marks all blocks that contain a given label as "boilerplate".
  /// </summary>
  public sealed class LabelToBoilerplateFilter : IBoilerpipeFilter {
    /// <summary>
    ///   Returns the singleton instance for <see cref="LabelToBoilerplateFilter" /> with label
    ///   <see cref="DefaultLabels.STRICTLY_NOT_CONTENT" />
    /// </summary>
    public static readonly LabelToBoilerplateFilter InstanceStrictlyNotContent =
      new LabelToBoilerplateFilter(DefaultLabels.STRICTLY_NOT_CONTENT);

    private readonly string[] _labels;

    /// <summary>
    ///   Creates a new instance of <see cref="LabelToBoilerplateFilter" />
    /// </summary>
    /// <param name="label"></param>
    public LabelToBoilerplateFilter(params string[] label) {
      _labels = label;
    }

    public bool Process(TextDocument doc) {
      bool changes = false;

      foreach (TextBlock tb in doc.TextBlocks.Where(tb => tb.IsContent && _labels.Any(tb.HasLabel))) {
        tb.IsContent = false;
        changes = true;
      }

      return changes;
    }
  }
}
