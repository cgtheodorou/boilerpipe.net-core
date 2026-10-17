namespace Boilerpipe.Net.Sax {
  using System.IO;
  using System.Text;

  /// <summary>
  ///   A single input source for HTML content: a character stream, a byte stream, or a file path given as <see cref="SystemId" />.
  /// </summary>
  public sealed class InputSource {
    public InputSource() {
    }

    public InputSource(string systemId) {
      SystemId = systemId;
    }

    public InputSource(Stream stream) {
      Stream = stream;
    }

    public InputSource(TextReader reader) {
      Reader = reader;
    }

    public string PublicId { get; set; }

    public string SystemId { get; set; }

    public Stream Stream { get; set; }

    public Encoding Encoding { get; set; }

    public TextReader Reader { get; set; }
  }
}
