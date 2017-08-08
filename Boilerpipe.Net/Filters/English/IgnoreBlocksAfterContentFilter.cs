namespace Boilerpipe.Net.Filters.English {
  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Labels;

  /// <summary>
  ///   Marks all blocks as "non-content" that occur after blocks that have been
  ///   marked <see cref="DefaultLabels.INDICATES_END_OF_TEXT" />. These marks are ignored
  ///   unless a minimum number of words in content blocks occur before this mark (default: 60).
  ///   This can be used in conjunction with an upstream <see cref="TerminatingBlocksFinder" />.
  /// </summary>
  /// <seealso cref="TerminatingBlocksFinder" />
  public sealed class IgnoreBlocksAfterContentFilter : HeuristicFilterBase, IBoilerpipeFilter {
    /// <summary>
    ///   The singleton instance for <see cref="IgnoreBlocksAfterContentFilter" /> with min 60 words.
    /// </summary>
    public static readonly IgnoreBlocksAfterContentFilter DefaultInstance = new IgnoreBlocksAfterContentFilter(60);
    /// <summary>
    ///   The singleton instance for <see cref="IgnoreBlocksAfterContentFilter" /> with min 200 words.
    /// </summary>
    public static readonly IgnoreBlocksAfterContentFilter Instance200 = new IgnoreBlocksAfterContentFilter(200);

    private readonly int _minNumWords;

    /// <summary>
    ///   Creates a new instance of <see cref="IgnoreBlocksAfterContentFilter" />
    /// </summary>
    /// <param name="minNumWords">The minimun number of words.</param>
    public IgnoreBlocksAfterContentFilter(int minNumWords) {
      _minNumWords = minNumWords;
    }

    public bool Process(TextDocument doc) {
      bool changes = false;

      int numWords = 0;
      bool foundEndOfText = false;
      foreach (TextBlock block in doc.TextBlocks) {
        bool endOfText = block.HasLabel(DefaultLabels.INDICATES_END_OF_TEXT);
        if (block.IsContent) {
          numWords += GetNumFullTextWords(block);
        }
        if (endOfText && numWords >= _minNumWords) {
          foundEndOfText = true;
        }
        if (foundEndOfText) {
          changes = true;
          block.IsContent = false;
        }
      }

      return changes;
    }
  }
}
