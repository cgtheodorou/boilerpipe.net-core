# boilerpipe.net-core

Boilerpipe text extraction library ported to .NET, based on rasmusjp's implementation in .NET 4.5 which you can find here https://github.com/rasmusjp/boilerpipe.net

Targets .NET 10 and .NET 8.

## Installation

Install [Boilerpipe.Net.Core](https://www.nuget.org/packages/Boilerpipe.Net.Core) from NuGet:

```shell
dotnet add package Boilerpipe.Net.Core
```

## Usage

```c#
using Boilerpipe.Net.Extractors;
...
string html = ...
// NOTE: Use ArticleExtractor unless DefaultExtractor gives better results for you
string text = CommonExtractors.ArticleExtractor.GetText(html);
```

## Upgrading from 1.x

Version 2.0 no longer depends on the `Sax.Net.Core` package.
The types it exposed through this library's API (`InputSource`, `IAttributes`, `IContentHandler`) now live in the `Boilerpipe.Net.Sax` namespace, so replace `using Sax.Net;` with `using Boilerpipe.Net.Sax;`.
`BoilerpipeHtmlContentHandler` implements `IContentHandler` directly instead of deriving from `Sax.Net.Helpers.DefaultHandler`.
`BoilerpipeHtmlParser.SetContentHandler(IContentHandler)` has been removed, since it silently discarded the handler.
An `InputSource` with a `Stream` and no `Encoding` now detects the encoding instead of throwing.

Extraction results change in 2.0, and for most pages they get noticeably better.
In 1.x the HTML parser reported every text node as a separate element, so inline markup such as links or bold text split sentences into separate blocks, and the document title was never detected.
Text is now parsed the way the original Java boilerpipe does it, which keeps inline text together, populates `TextDocument.Title`, and lets `ArticleExtractor` use the title to locate the article.
HTML entities such as `&amp;`, `&nbsp;` and `&#8212;` are now decoded instead of appearing literally in the extracted text.

# License

Boilerpipe.Net is licensed under [LGPL V3](LICENSE).
