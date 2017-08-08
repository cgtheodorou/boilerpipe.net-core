namespace Boilerpipe.Net.Filters.Simple {
  using System.Collections.Generic;

  using Boilerpipe.Net.Conditions;
  using Boilerpipe.Net.Document;

  public class SurroundingToContentFilter : IBoilerpipeFilter {
    public static readonly SurroundingToContentFilter InstanceText =
      new SurroundingToContentFilter(new TextTextBlockCondition());

    private readonly ITextBlockCondition _cond;

    public SurroundingToContentFilter(ITextBlockCondition cond) {
      _cond = cond;
    }

    public bool Process(TextDocument doc) {
      List<TextBlock> tbs = doc.TextBlocks;
      if (tbs.Count < 3) {
        return false;
      }

      TextBlock a = tbs[0];
      TextBlock b = tbs[1];
      bool hasChanges = false;
      for (int index = 0; index < tbs.Count; index++) {
        TextBlock c = tbs[index];
        if (!b.IsContent && a.IsContent && c.IsContent && _cond.MeetsCondition(b)) {
          b.IsContent = true;
          hasChanges = true;
        }

        a = c;
        if (index == tbs.Count) {
          break;
        }
        b = c;
      }

      return hasChanges;
    }

    private class TextTextBlockCondition : ITextBlockCondition {
      public bool MeetsCondition(TextBlock block) {
        return block.LinkDensity == 0 && block.NumWords > 6;
      }
    }
  }
}
