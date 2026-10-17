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

# License

Boilerpipe.Net is licensed under [LGPL V3](LICENSE).
