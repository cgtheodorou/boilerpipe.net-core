namespace Boilerpipe.Net.Sax {
  /// <summary>
  ///   Default <see cref="ITagAction" />. Seem to work well.
  /// </summary>
  /// <seealso cref="TagActionDictionary" />
  public class DefaultTagActionDictionary : TagActionDictionary {
    /// <summary>
    ///   The singleton instance for <see cref="DefaultTagActionDictionary" />
    /// </summary>
    public static readonly TagActionDictionary Instance = new DefaultTagActionDictionary();

    /// <summary>
    ///   Creates a new instance of <see cref="DefaultTagActionDictionary" />
    /// </summary>
    protected DefaultTagActionDictionary() {
      SetTagAction("STYLE", CommonTagActions.IgnorableElement);
      SetTagAction("SCRIPT", CommonTagActions.IgnorableElement);
      SetTagAction("OPTION", CommonTagActions.IgnorableElement);
      SetTagAction("OBJECT", CommonTagActions.IgnorableElement);
      SetTagAction("EMBED", CommonTagActions.IgnorableElement);
      SetTagAction("APPLET", CommonTagActions.IgnorableElement);
      SetTagAction("LINK", CommonTagActions.IgnorableElement);

      SetTagAction("A", CommonTagActions.AnchorText);
      SetTagAction("BODY", CommonTagActions.Body);

      SetTagAction("STRIKE", CommonTagActions.InlineNoWhitespace);
      SetTagAction("U", CommonTagActions.InlineNoWhitespace);
      SetTagAction("B", CommonTagActions.InlineNoWhitespace);
      SetTagAction("I", CommonTagActions.InlineNoWhitespace);
      SetTagAction("EM", CommonTagActions.InlineNoWhitespace);
      SetTagAction("STRONG", CommonTagActions.InlineNoWhitespace);
      SetTagAction("SPAN", CommonTagActions.InlineNoWhitespace);

      // New in 1.1 (especially to improve extraction quality from Wikipedia etc.)
      SetTagAction("SUP", CommonTagActions.InlineNoWhitespace);

      // New in 1.2
      SetTagAction("CODE", CommonTagActions.InlineNoWhitespace);
      SetTagAction("TT", CommonTagActions.InlineNoWhitespace);
      SetTagAction("SUB", CommonTagActions.InlineNoWhitespace);
      SetTagAction("VAR", CommonTagActions.InlineNoWhitespace);

      SetTagAction("ABBR", CommonTagActions.InlineWhitespace);
      SetTagAction("ACRONYM", CommonTagActions.InlineWhitespace);

      SetTagAction("FONT", CommonTagActions.InlineNoWhitespace); // could also use TA_FONT

      // added in 1.1.1
      SetTagAction("NOSCRIPT", CommonTagActions.IgnorableElement);
    }
  }
}
