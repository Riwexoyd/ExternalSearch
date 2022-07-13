using AngleSharp.Dom;

using Riwexoyd.ExternalSearch.Games.Contracts;
using Riwexoyd.ExternalSearch.Games.Converters;

using System.Text.RegularExpressions;

namespace Riwexoyd.ExternalSearch.Games.Services
{
    internal sealed class InteresExternalSearchProvider : ParseExternalSearchProvider
    {
        private static readonly string[] Consoles = { "[XBOX]", "[PS4]", "[PS5]" };

        public override string SearchUri { get; } = "https://www.1c-interes.ru/search/?q={0}&ib=114&genre=3353622";

        public override Guid Uid { get; } = new Guid("{B340BCB9-60CC-4553-B7C4-DAD6213431AE}");

        public override string Name { get; } = "1c interes (https://www.1c-interes.ru/)";

        public override Uri BaseLinkUri { get; } = new Uri("https://www.1c-interes.ru/");

        protected override IEnumerable<GameSearchResult> ParseDocument(IDocument document)
        {
            IHtmlCollection<IElement> shopItems = document.QuerySelectorAll("div.catalog_item");
            List<GameSearchResult> gameSearchResults = new(shopItems.Length);

            foreach (IElement shopItem in shopItems)
            {
                IElement titleElement = shopItem.QuerySelector("div.catalog_item__name a")!;
                IElement priceElement = shopItem.QuerySelector("div.catalog_item__price a ins strong")!;

                string gameTitle = titleElement.TextContent.Trim();

                string? upperTitle = gameTitle.ToUpper();

                if (Consoles.Any(console => upperTitle.Contains(console)))
                    continue;

                string? link = titleElement.GetAttribute("href");

                if (string.IsNullOrEmpty(link))
                    continue;

                string stringPrice = priceElement?.TextContent ?? string.Empty;

                Match costMatch = NullableIntParseConverter.DigitalRegex.Match(stringPrice);
                int? price = null;

                if (costMatch.Success && int.TryParse(costMatch.Value, out int tempPrice))
                    price = tempPrice;

                gameSearchResults.Add(new GameSearchResult
                {
                    GameTitle = gameTitle,
                    Price = price,
                    ProviderUid = Uid,
                    Url = GetGameLink(link)
                });
            }

            return gameSearchResults;
        }
    }
}
