namespace Boilerpipe.Net.Sax {
  using System;

  using global::Sax.Net;
  using global::Sax.Net.Helpers;

  using HtmlAgilityPack;

  internal class HtmlAgilityPackParser : IXmlReader {
    public bool GetFeature(string name) {
      throw new NotSupportedException();
    }

    public void SetFeature(string name, bool value) {
      throw new NotSupportedException();
    }

    public object GetProperty(string name) {
      throw new NotSupportedException();
    }

    public void SetProperty(string name, object value) {
      throw new NotSupportedException();
    }

    public void Parse(InputSource input) {
      var document = new HtmlDocument();
      if (input.Stream != null) {
        document.Load(input.Stream, input.Encoding);
      } else if (input.Reader != null) {
        document.Load(input.Reader);
      } else if (input.PublicId != null) {
        document.Load(input.PublicId);
      } else if (input.SystemId != null) {
        document.Load(input.SystemId);
      }

      ContentHandler.StartDocument();

      TraverseNode(document.DocumentNode);

      ContentHandler.EndDocument();
    }

    public void Parse(string systemId) {
      Parse(new InputSource(systemId));
    }

    public IEntityResolver EntityResolver { get; set; }
    public IDTDHandler DTDHandler { get; set; }
    public IContentHandler ContentHandler { get; set; }
    public IErrorHandler ErrorHandler { get; set; }

    private void TraverseNode(HtmlNode htmlNode) {
      if (htmlNode == null || htmlNode.NodeType == HtmlNodeType.Comment) {
        return;
      }

      var attributes = new Attributes();
      if (htmlNode.HasAttributes) {
        foreach (HtmlAttribute attribute in htmlNode.Attributes) {
          attributes.AddAttribute(null, htmlNode.Name, attribute.Name, null, attribute.Value);
        }
      }

      ContentHandler.StartElement(null, htmlNode.Name, htmlNode.Name, attributes);
      if (htmlNode.NodeType == HtmlNodeType.Text) {
        ContentHandler.Characters(htmlNode.InnerText.ToCharArray(), 0, htmlNode.InnerText.Length);
      } else if (htmlNode.HasChildNodes) {
        foreach (HtmlNode childNode in htmlNode.ChildNodes) {
          TraverseNode(childNode);
        }
      }
      ContentHandler.EndElement(null, htmlNode.Name, htmlNode.Name);
    }
  }
}
