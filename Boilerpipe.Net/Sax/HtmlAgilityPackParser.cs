namespace Boilerpipe.Net.Sax {
  using HtmlAgilityPack;

  internal sealed class HtmlAgilityPackParser {
    private readonly IContentHandler _contentHandler;

    public HtmlAgilityPackParser(IContentHandler contentHandler) {
      _contentHandler = contentHandler;
    }

    public void Parse(InputSource input) {
      var document = new HtmlDocument();
      if (input.Stream != null) {
        if (input.Encoding != null) {
          document.Load(input.Stream, input.Encoding);
        } else {
          document.Load(input.Stream);
        }
      } else if (input.Reader != null) {
        document.Load(input.Reader);
      } else if (input.SystemId != null) {
        document.Load(input.SystemId);
      }

      _contentHandler.StartDocument();

      TraverseNode(document.DocumentNode);

      _contentHandler.EndDocument();
    }

    private void TraverseNode(HtmlNode htmlNode) {
      if (htmlNode == null || htmlNode.NodeType == HtmlNodeType.Comment) {
        return;
      }

      var attributes = new Attributes();
      if (htmlNode.HasAttributes) {
        foreach (HtmlAttribute attribute in htmlNode.Attributes) {
          attributes.Add(attribute.Name, attribute.Value);
        }
      }

      _contentHandler.StartElement(null, htmlNode.Name, htmlNode.Name, attributes);
      if (htmlNode.NodeType == HtmlNodeType.Text) {
        string text = htmlNode.InnerText;
        _contentHandler.Characters(text.ToCharArray(), 0, text.Length);
      } else if (htmlNode.HasChildNodes) {
        foreach (HtmlNode childNode in htmlNode.ChildNodes) {
          TraverseNode(childNode);
        }
      }
      _contentHandler.EndElement(null, htmlNode.Name, htmlNode.Name);
    }
  }
}
