namespace Boilerpipe.Net.Labels {
  using Boilerpipe.Net.Conditions;
  using Boilerpipe.Net.Document;

  /// <summary>
  ///   Adds labels to a <see cref="TextBlock" /> if the given criteria are met.
  /// </summary>
  public sealed class ConditionalLabelAction : LabelAction {
    private readonly ITextBlockCondition _condition;

    /// <summary>
    ///   Creates a new instance of <see cref="ConditionalLabelAction" />
    /// </summary>
    /// <param name="condition">the condition</param>
    /// <param name="labels">the labels to add</param>
    public ConditionalLabelAction(ITextBlockCondition condition, params string[] labels)
      : base(labels) {
      _condition = condition;
    }

    /// <summary>
    ///   Adds the labels to the <see cref="TextBlock" /> if the condition is met.
    /// </summary>
    /// <param name="block">The <see cref="TextBlock" /> to add the labels to.</param>
    public override void AddTo(TextBlock block) {
      if (_condition.MeetsCondition(block)) {
        AddLabelsTo(block);
      }
    }
  }
}
