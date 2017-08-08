namespace Boilerpipe.Net.Sax {
  using System.IO;

  using Boilerpipe.Net.Document;

  using global::Sax.Net;

  /// <summary>
  ///     Parses an <see cref="InputSource" /> using SAX and returns a <see cref="TextDocument" />.
  /// </summary>
  public sealed class BoilerpipeSAXInput : IBoilerpipeInput {
    private readonly InputSource _source;

    /// <summary>
    ///     Creates a new instance of <see cref="BoilerpipeSAXInput" /> for the given <see cref="InputSource" />.
    /// </summary>
    /// <param name="source"></param>
    public BoilerpipeSAXInput(InputSource source) {
      _source = source;
    }

    /// <summary>
    ///     Retrieves the {@link TextDocument} using a default HTML parser.
    /// </summary>
    /// <returns></returns>
    public TextDocument GetTextDocument() {
      return GetTextDocument(new BoilerpipeHtmlParser());
    }

    /// <summary>
    ///     Retrieves the <see cref="TextDocument" /> using the given HTML parser.
    /// </summary>
    /// <param name="parser">The parser used to transform the input into boilerpipe's internal representation.</param>
    /// <returns>The retrieved <see cref="TextDocument" /></returns>
    /// <exception cref="BoilerpipeProcessingException"></exception>
    public TextDocument GetTextDocument(BoilerpipeHtmlParser parser) {
      try {
        parser.Parse(_source);
      } catch (IOException ex) {
        throw new BoilerpipeProcessingException(ex.Message, ex);
      } catch (SAXException ex) {
        throw new BoilerpipeProcessingException(ex.Message, ex);
      }

      return parser.ToTextDocument();
    }
  }
}
