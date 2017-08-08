namespace Boilerpipe.Net.Labels {
  using Boilerpipe.Net.Document;

  /// <summary>
  ///   Some pre-defined labels which can be used in conjunction with
  ///   <see cref="TextBlock.AddLabel(string)" /> and <see cref="TextBlock.HasLabel(string)" />.
  /// </summary>
  public sealed class DefaultLabels {
    public const string TITLE = "de.l3s.boilerpipe/TITLE";
    public const string ARTICLE_METADATA = "de.l3s.boilerpipe/ARTICLE_METADATA";
    public const string INDICATES_END_OF_TEXT = "de.l3s.boilerpipe/INDICATES_END_OF_TEXT";
    public const string MIGHT_BE_CONTENT = "de.l3s.boilerpipe/MIGHT_BE_CONTENT";
    public const string STRICTLY_NOT_CONTENT = "de.l3s.boilerpipe/STRICTLY_NOT_CONTENT";
    public const string HR = "de.l3s.boilerpipe/HR";
    public const string MARKUP_PREFIX = "<";
  }
}
