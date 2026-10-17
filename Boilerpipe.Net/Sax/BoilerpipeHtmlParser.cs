namespace Boilerpipe.Net.Sax {
  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Extractors;

    /// <summary>
    ///   A simple Parser, used by <see cref="BaseExtractor" />.
    ///   The parser uses <a href="https://html-agility-pack.net/">HtmlAgilityPack</a> to parse HTML content.
    /// </summary>
    public class BoilerpipeHtmlParser : IBoilerpipeDocumentSource {
    private BoilerpipeHtmlContentHandler _contentHandler;

    /// <summary>
    ///   Constructs a <see cref="BoilerpipeHtmlParser" /> using a default HTML content handler.
    /// </summary>
    public BoilerpipeHtmlParser()
      : this(new BoilerpipeHtmlContentHandler()) {
    }

    /// <summary>
    ///   Constructs a <see cref="BoilerpipeHtmlParser"/> using the given <see cref="BoilerpipeHtmlContentHandler" />.
    /// </summary>
    /// <param name="contentHandler">the <see cref="BoilerpipeHtmlContentHandler" /> to use</param>
    public BoilerpipeHtmlParser(BoilerpipeHtmlContentHandler contentHandler) {
      SetContentHandler(contentHandler);
    }

    public void Parse(InputSource source) {
      new HtmlAgilityPackParser(_contentHandler).Parse(source);
    }

    /// <summary>
    /// Returns a <see cref="TextDocument"/> containing the extracted <see cref="TextBlock"/>s.
    /// </summary>
    /// <returns>The <see cref="TextDocument"/></returns>
    /// <remarks>
    /// NOTE: Only call this after <see cref="Parse(InputSource)"/>
    /// </remarks>
    public TextDocument ToTextDocument() {
      return _contentHandler.ToTextDocument();
    }

    public void SetContentHandler(BoilerpipeHtmlContentHandler contentHandler) {
    	_contentHandler = contentHandler;
    }
  }
}
