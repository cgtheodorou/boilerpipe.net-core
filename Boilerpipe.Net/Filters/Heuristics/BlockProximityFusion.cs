namespace Boilerpipe.Net.Filters.Heuristics {
  using System.Collections.Generic;
  using System.Linq;

  using Boilerpipe.Net.Document;

  /// <summary>
  ///   Fuses adjacent blocks if their distance (in blocks) does not exceed a certain limit.
  ///   This probably makes sense only in cases where an upstream filter already has removed some blocks.
  /// </summary>
  public sealed class BlockProximityFusion : IBoilerpipeFilter {
    /// <summary>
    ///   The singleton instance for <see cref="BlockProximityFusion" /> with max distance 1.
    /// </summary>
    public static readonly BlockProximityFusion MaxDistance1 = new BlockProximityFusion(1, false, false);

    /// <summary>
    ///   The singleton instance for <see cref="BlockProximityFusion" /> with max distance 1 and same tag level.
    /// </summary>
    public static readonly BlockProximityFusion MaxDistance1SameTaglevel = new BlockProximityFusion(1, false, true);

    /// <summary>
    ///   The singleton instance for <see cref="BlockProximityFusion" /> with max distance 1 and content only.
    /// </summary>
    public static readonly BlockProximityFusion MaxDistance1ContentOnly = new BlockProximityFusion(1, true, false);

    /// <summary>
    ///   The singleton instance for <see cref="BlockProximityFusion" /> with max distance 1, content only and same tag level.
    /// </summary>
    public static readonly BlockProximityFusion MaxDistance1ContentOnlySameTaglevel = new BlockProximityFusion(1, true, true);

    private readonly bool _contentOnly;
    private readonly int _maxBlocksDistance;
    private readonly bool _sameTagLevelOnly;

    /// <summary>
    ///   Creates a new <see cref="BlockProximityFusion" /> instance.
    /// </summary>
    /// <param name="maxBlocksDistance">The maximum distance in blocks.</param>
    /// <param name="contentOnly">Only process content.</param>
    /// <param name="sameTagLevelOnly">Only process if same tag level.</param>
    public BlockProximityFusion(int maxBlocksDistance, bool contentOnly, bool sameTagLevelOnly) {
      _maxBlocksDistance = maxBlocksDistance;
      _contentOnly = contentOnly;
      _sameTagLevelOnly = sameTagLevelOnly;
    }

    public bool Process(TextDocument doc) {
      List<TextBlock> textBlocks = doc.TextBlocks;
      if (textBlocks.Count < 2) {
        return false;
      }

      bool changes = false;
      TextBlock prevBlock;

      int offset;
      if (_contentOnly) {
        prevBlock = null;
        offset = 0;
        foreach (TextBlock tb in textBlocks) {
          offset++;
          if (tb.IsContent) {
            prevBlock = tb;
            break;
          }
        }
        if (prevBlock == null) {
          return false;
        }
      } else {
        prevBlock = textBlocks[0];
        offset = 1;
      }

      foreach (TextBlock block in textBlocks.Skip(offset).ToList()) {
        if (!block.IsContent) {
          prevBlock = block;
          continue;
        }
        int diffBlocks = block.OffsetBlocksStart - prevBlock.OffsetBlocksEnd - 1;
        if (diffBlocks <= _maxBlocksDistance) {
          bool ok = true;
          if (_contentOnly) {
            if (!prevBlock.IsContent || !block.IsContent) {
              ok = false;
            }
          }
          if (ok && _sameTagLevelOnly && prevBlock.TagLevel != block.TagLevel) {
            ok = false;
          }
          if (ok) {
            prevBlock.MergeNext(block);
            textBlocks.Remove(block);
            changes = true;
          } else {
            prevBlock = block;
          }
        } else {
          prevBlock = block;
        }
      }

      return changes;
    }
  }
}
