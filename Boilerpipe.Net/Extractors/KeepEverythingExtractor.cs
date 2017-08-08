namespace Boilerpipe.Net.Extractors {
  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Filters.Simple;

  /// <summary>
  /// A full-text extractor that keeps everything.
  /// </summary>
  public sealed class KeepEverythingExtractor : BaseExtractor {
    /// <summary>
    ///   The singleton instance for <see cref="KeepEverythingExtractor" />
    /// </summary>
    public static readonly KeepEverythingExtractor Instance = new KeepEverythingExtractor();

    private KeepEverythingExtractor() {
    }

    public override bool Process(TextDocument doc) {
      return MarkEverythingContentFilter.Instance.Process(doc);
    }
  }
}
