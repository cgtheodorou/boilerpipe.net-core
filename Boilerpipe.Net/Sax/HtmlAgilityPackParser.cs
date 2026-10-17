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
      switch (htmlNode.NodeType) {
        case HtmlNodeType.Document:
          TraverseChildren(htmlNode);
          break;
        case HtmlNodeType.Text:
          string text = HtmlEntity.DeEntitize(htmlNode.InnerText);
          _contentHandler.Characters(text.ToCharArray(), 0, text.Length);
          break;
        case HtmlNodeType.Element:
          var attributes = new Attributes();
          foreach (HtmlAttribute attribute in htmlNode.Attributes) {
            attributes.Add(attribute.Name, attribute.Value);
          }

          _contentHandler.StartElement(null, htmlNode.Name, htmlNode.Name, attributes);
          TraverseChildren(htmlNode);
          _contentHandler.EndElement(null, htmlNode.Name, htmlNode.Name);
          break;
      }
    }

    private void TraverseChildren(HtmlNode htmlNode) {
      foreach (HtmlNode childNode in htmlNode.ChildNodes) {
        TraverseNode(childNode);
      }
    }
  }
}
