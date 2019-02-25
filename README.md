# boilerpipe.net-core
Boilerpipe text extraction library ported to .Net Core based on rasmusjp's implementation in .NET 4.5 which you can find here https://github.com/rasmusjp/boilerpipe.net

## Installation

To install [Boilerpipe.Net.Core](https://www.nuget.org/packages/Boilerpipe.Net.Core) from the [NuGet Gallery](http://www.nuget.org), run the following in the [Package Manager Console](http://docs.nuget.org/docs/start-here/using-the-package-manager-console)
```powershell
PM> Install-Package Boilerpipe.Net.Core
```

## Usage

```c#
using Boilerpipe.Net.Extractors;
...
string html = ...
// NOTE: Use ArticleExtractor unless DefaultExtractor gives better results for you
string text = CommonExtractors.ArticleExtractor.GetText(html);

```

# License

Boilerpipe.Net is licensed under [LGPL V3](LICENSE).
