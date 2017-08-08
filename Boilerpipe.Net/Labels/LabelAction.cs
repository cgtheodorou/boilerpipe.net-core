namespace Boilerpipe.Net.Labels {
  using Boilerpipe.Net.Document;

  /// <summary>
  ///   Helps adding labels to <see cref="TextBlock" />s.
  /// </summary>
  /// <seealso cref="ConditionalLabelAction" />
  public class LabelAction {
    protected string[] Labels;

    public LabelAction(params string[] labels) {
      Labels = labels;
    }

    public virtual void AddTo(TextBlock block) {
      AddLabelsTo(block);
    }

    protected virtual void AddLabelsTo(TextBlock tb) {
      tb.AddLabels(Labels);
    }

    public override string ToString() {
      return base.ToString() + "{" + string.Join(",", Labels) + "}";
    }
  }
}
