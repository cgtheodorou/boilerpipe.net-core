namespace Boilerpipe.Net.Sax {
  using Boilerpipe.Net.Document;
  using Boilerpipe.Net.Extractors;

  using global::Sax.Net;

    /// <summary>
    ///   A simple Parser, used by <see cref="BaseExtractor" />.
    ///   The parser uses <a href="https://htmlagilitypack.codeplex.com/">HtmlAgilityPack</a> to parse HTML content.
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
    ///   Constructs a <see cref="BoilerpipeHtmlParser"/> using the given <see cref="IContentHandler" />.
    /// </summary>
    /// <param name="contentHandler">the <see cref="IContentHandler" /> to use</param>
    public BoilerpipeHtmlParser(BoilerpipeHtmlContentHandler contentHandler) {
      SetContentHandler(contentHandler);
    }

    public void Parse(InputSource source) {
      IXmlReader parser = new HtmlAgilityPackParser();
      parser.ContentHandler = _contentHandler;
      parser.Parse(source);
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

    public void SetContentHandler(IContentHandler contentHandler) {
    	_contentHandler = null;
    }
  }
}
