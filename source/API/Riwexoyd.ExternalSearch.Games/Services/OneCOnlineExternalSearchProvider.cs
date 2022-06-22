using AngleSharp.Dom;

using Riwexoyd.ExternalSearch.Games.Contracts;
using Riwexoyd.ExternalSearch.Games.Converters;

using System.Text.RegularExpressions;

namespace Riwexoyd.ExternalSearch.Games.Services
{
    internal sealed class OneCOnlineExternalSearchProvider : ParseExternalSearchProvider
    {
        public override string SearchUri { get; } = "https://online.1c.ru/search/index.php?q={0}&where=iblock_GAMES_IB";

        public override Guid Uid { get; } = new Guid("{3144AA9D-0132-4A73-A129-D4324495BA52}");

        public override string Name { get; } = "1c Online (https://online.1c.ru/)";

        public override Uri BaseLinkUri { get; } = new Uri("https://online.1c.ru/");

        protected override IEnumerable<GameSearchResult> ParseDocument(IDocument document)
        {
            IElement searchPage = document.QuerySelector("div.search_page")!;
            List<GameSearchResult> gameSearchResults = new();

            List<IElement> shopItems = searchPage.QuerySelectorAll("a")
                .Where(elem => elem.HasAttribute("href") && 
                elem.Attributes["href"]!.Value.StartsWith("/games/game/") &&
                elem.Children.Any(child => child.TagName.ToUpper() == "B"))
                .ToList();

            IHtmlCollection<IElement>? prices = searchPage.QuerySelectorAll("div.price big");

            if (shopItems.Count != prices.Length)
                return Enumerable.Empty<GameSearchResult>();

            foreach (int shopItemIndex in Enumerable.Range(0, shopItems.Count))
            {
                IElement titleElement = shopItems[shopItemIndex];
                IElement priceElement = prices[shopItemIndex];

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
                    GameTitle = titleElement.TextContent.Trim(),
                    Price = price,
                    ProviderUid = Uid,
                    Url = GetGameLink(link)
                });
            }

            return gameSearchResults;
        }
    }
}
