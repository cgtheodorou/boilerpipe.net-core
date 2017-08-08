namespace Boilerpipe.Net.Conditions {
  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Labels;

  /// <summary>
  ///   Evaluates whether a given <see cref="TextBlock" /> meets a certain condition.
  ///   Useful in combination with <see cref="ConditionalLabelAction" />.
  /// </summary>
  public interface ITextBlockCondition {
    /// <summary>
    ///   Returns <code>true</code> if the given <see cref="TextBlock" /> meets the defined condition.
    /// </summary>
    /// <param name="block">The <see cref="TextBlock" /> to test.</param>
    /// <returns><code>true</code> if the condition is met.</returns>
    bool MeetsCondition(TextBlock block);
  }
}
