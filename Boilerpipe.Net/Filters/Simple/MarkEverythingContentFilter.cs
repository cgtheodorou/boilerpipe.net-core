namespace Boilerpipe.Net.Filters.Simple {
  using System.Linq;

  using Boilerpipe.Net.Document;

  /// <summary>
  ///   Marks all blocks as content.
  /// </summary>
  public sealed class MarkEverythingContentFilter : IBoilerpipeFilter {
    /// <summary>
    ///   Returns the singleton instance for <see cref="MarkEverythingContentFilter" />
    /// </summary>
    public static readonly MarkEverythingContentFilter Instance = new MarkEverythingContentFilter();

    private MarkEverythingContentFilter() {
    }

    public bool Process(TextDocument doc) {
      bool changes = false;

      foreach (TextBlock tb in doc.TextBlocks.Where(tb => !tb.IsContent)) {
        tb.IsContent = true;
        changes = true;
      }

      return changes;
    }
  }
}
