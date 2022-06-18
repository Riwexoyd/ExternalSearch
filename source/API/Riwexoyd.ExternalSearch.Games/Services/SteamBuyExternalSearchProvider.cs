using AngleSharp;
using AngleSharp.Dom;

using Riwexoyd.ExternalSearch.Games.Contracts;
using Riwexoyd.ExternalSearch.Games.Models;

using System.Text.Json;
using System.Text.RegularExpressions;

namespace Riwexoyd.ExternalSearch.Games.Services
{
    internal sealed class SteamBuyExternalSearchProvider : GetGameExternalSearchProvider
    {
        private static readonly Uri GameUri = new Uri("https://steambuy.com/");
        private static readonly Regex DigitalRegex = new(@"\d+");
        private readonly IBrowsingContext _browsingContext;

        public override Guid Uid { get; } = new Guid("{7EA92CB7-4C1D-4DDF-A36D-51E09139FACE}");

        public override string Name { get; } = "SteamBuy (https://steambuy.com/)";

        protected override string SearchUri { get; } = "https://steambuy.com/ajax/_get.php?a=search&q={0}";

        public SteamBuyExternalSearchProvider()
        {
            _browsingContext = BrowsingContext.New();
        }

        protected override async Task<IEnumerable<GameSearchResult>> GetDataFromStream(Stream stream, CancellationToken cancellationToken)
        {
            SteamBuySearchResult? data = await JsonSerializer.DeserializeAsync<SteamBuySearchResult>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            }, cancellationToken);

            if (data == null || data.All == 0)
                return Enumerable.Empty<GameSearchResult>();

            IDocument document = await _browsingContext.OpenAsync(req => req.Content(data.Html), cancellationToken);
            IHtmlCollection<IElement> items = document.QuerySelectorAll("div.search-result__item");

            List<GameSearchResult> results = new List<GameSearchResult>(items.Length);

            foreach (var item in items)
            {
                IElement titleElemement = item.QuerySelector("div.search-result__title")!;
                IElement linkElement = item.QuerySelector("a.search-result__link")!;
                IElement costElement = item.QuerySelector("div.search-result__cost")!;
                string? link = linkElement.GetAttribute("href");

                if (string.IsNullOrEmpty(link))
                    continue;

                Match costMatch = DigitalRegex.Match(costElement.TextContent);
                int? price = null;

                if (costMatch.Success && int.TryParse(costMatch.Value, out int tempPrice))
                    price = tempPrice;

                GameSearchResult result = new GameSearchResult
                {
                    Price = price,
                    ProviderUid = Uid,
                    GameTitle = titleElemement.TextContent,
                    Url = new Uri(GameUri, link).AbsoluteUri
                };

                results.Add(result);
            }

            return results;
        }
    }
}
