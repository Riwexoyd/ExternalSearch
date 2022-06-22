using AngleSharp.Dom;

using Riwexoyd.ExternalSearch.Games.Contracts;
using Riwexoyd.ExternalSearch.Games.Converters;

using System.Text.RegularExpressions;

namespace Riwexoyd.ExternalSearch.Games.Services
{
    internal sealed class BukaExternalSearchProvider : ParseExternalSearchProvider
    {
        public override string SearchUri { get; } = "https://shop.buka.ru/search?q={0}";

        public override Guid Uid { get; } = new Guid("{179CB95E-8C6D-4A86-8A96-6CFEADBFE441}");

        public override string Name { get; } = "Buka (https://shop.buka.ru/)";

        public override Uri BaseLinkUri { get; } = new Uri("https://shop.buka.ru/");

        protected override IEnumerable<GameSearchResult> ParseDocument(IDocument document)
        {
            IHtmlCollection<IElement> shopItems = document.QuerySelectorAll("div.product-thumb");
            List<GameSearchResult> gameSearchResults = new(shopItems.Length);

            foreach (IElement shopItem in shopItems)
            {
                IElement description = shopItem.QuerySelector("div.product-thumb__description")!;
                IElement linkElement = shopItem.QuerySelector("a.js-product-click")!;

                string? link = linkElement.GetAttribute("href");

                if (string.IsNullOrEmpty(link))
                    continue;

                string title = description.QuerySelector("h3")?.TextContent ?? string.Empty;

                if (!title.Contains("PC"))
                    continue;

                string stringPrice = description.QuerySelector("strong.product-thumb__price")?.TextContent ?? string.Empty;

                Match costMatch = NullableIntParseConverter.DigitalRegex.Match(stringPrice);
                int? price = null;

                if (costMatch.Success && int.TryParse(costMatch.Value, out int tempPrice))
                    price = tempPrice;

                gameSearchResults.Add(new GameSearchResult
                {
                    GameTitle = title,
                    Price = price,
                    ProviderUid = Uid,
                    Url = GetGameLink(link)
                });
            }

            return gameSearchResults;
        }
    }
}
