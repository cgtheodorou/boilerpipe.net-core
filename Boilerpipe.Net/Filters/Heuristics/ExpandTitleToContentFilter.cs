namespace Boilerpipe.Net.Filters.Heuristics {
  using System.Linq;

  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Labels;

  /// <summary>
  ///   Marks all <see cref="TextBlock" />s "content" which are between the headline and the part that
  ///   has already been marked content, if they are marked <see cref="DefaultLabels.MIGHT_BE_CONTENT" />.
  ///   <para>
  ///     This filter is quite specific to the news domain.
  ///   </para>
  /// </summary>
  public sealed class ExpandTitleToContentFilter : IBoilerpipeFilter {
    /// <summary>
    ///   The singleton instance for <see cref="ExpandTitleToContentFilter" />.
    /// </summary>
    public static ExpandTitleToContentFilter Instance = new ExpandTitleToContentFilter();

    public bool Process(TextDocument doc) {
      int i = 0;
      int title = -1;
      int contentStart = -1;
      foreach (TextBlock tb in doc.TextBlocks) {
        if (contentStart == -1 && tb.HasLabel(DefaultLabels.TITLE)) {
          title = i;
          contentStart = -1;
        }
        if (contentStart == -1 && tb.IsContent) {
          contentStart = i;
        }

        i++;
      }

      if (contentStart <= title || title == -1) {
        return false;
      }
      bool changes = false;
      foreach (TextBlock tb in doc.TextBlocks.Skip(title).Take(contentStart-title)) {
        if (tb.HasLabel(DefaultLabels.MIGHT_BE_CONTENT)) {
          bool isContentChanged = false;
          if (tb.IsContent != true) {
            tb.IsContent = true;
            isContentChanged = true;
          }
          changes = isContentChanged | changes;
        }
      }
      return changes;
    }
  }
}
