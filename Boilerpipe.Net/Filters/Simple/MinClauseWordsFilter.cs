namespace Boilerpipe.Net.Filters.Simple {
  using System.Text.RegularExpressions;

  using Boilerpipe.Net.Document;

  /// <summary>
  ///   Keeps only blocks that have at least one segment fragment ("clause") with at
  ///   least <em>k</em> words (default: 5).
  /// </summary>
  /// <remarks>
  ///   NOTE: You might consider using the <see cref="SplitParagraphBlocksFilter" /> upstream
  /// </remarks>
  /// <seealso cref="SplitParagraphBlocksFilter" />
  public sealed class MinClauseWordsFilter : IBoilerpipeFilter {
    /// <summary>
    ///   Returns the singleton instance for <see cref="MinClauseWordsFilter" />
    /// </summary>
    public static readonly MinClauseWordsFilter Instance = new MinClauseWordsFilter(5, false);

    private readonly bool _acceptClausesWithoutDelimiter;
    private readonly int _minWords;
    private readonly Regex _patClauseDelimiter = new Regex("[\\p{L}\\d][\\,\\.\\:\\;\\!\\?]+([ \\n\\r]+|$)", RegexOptions.Compiled);
    private readonly Regex _patWhitespace = new Regex("[ \\n\\r]+", RegexOptions.Compiled);

    /// <summary>
    ///   Creates a new instance of <see cref="MinClauseWordsFilter" />
    /// </summary>
    /// <param name="minWords"></param>
    public MinClauseWordsFilter(int minWords)
      : this(minWords, false) {
    }

    /// <summary>
    ///   Creates a new instance of <see cref="MinClauseWordsFilter" />
    /// </summary>
    /// <param name="minWords"></param>
    /// <param name="acceptClausesWithoutDelimiter"></param>
    public MinClauseWordsFilter(int minWords, bool acceptClausesWithoutDelimiter) {
      _minWords = minWords;
      _acceptClausesWithoutDelimiter = acceptClausesWithoutDelimiter;
    }

    public bool Process(TextDocument doc) {
      bool changes = false;
      foreach (TextBlock tb in doc.TextBlocks) {
        if (!tb.IsContent) {
          continue;
        }
        string text = tb.Text;

        Match m = _patClauseDelimiter.Match(text);
        int start = 0;
        int end;
        bool hasClause = false;
        while (m.Success) {
          end = m.Index + 1 - start;
          hasClause = IsClause(text.Substring(start, end));
          start += m.Length;

          if (hasClause) {
            break;
          }
          m = _patClauseDelimiter.Match(text, start);
        }
        end = text.Length - start;

        // since clauses should *always end* with a delimiter, we normally
        // don't consider text without one
        if (_acceptClausesWithoutDelimiter) {
          hasClause |= IsClause(text.Substring(start, end));
        }

        if (!hasClause) {
          tb.IsContent = false;
          changes = true;
          // System.err.println("IS NOT CONTENT: " + text);
        }
      }

      return changes;
    }

    private bool IsClause(string text) {
      MatchCollection matches = _patWhitespace.Matches(text);
      int n = 1;
      foreach (Match match in matches) {
        if (match.Success) {
          n++;
          if (n >= _minWords) {
            return true;
          }
        }
      }
      return n >= _minWords;
    }
  }
}
