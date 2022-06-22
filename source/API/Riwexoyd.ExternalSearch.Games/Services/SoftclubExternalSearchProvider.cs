using AngleSharp.Dom;

using Riwexoyd.ExternalSearch.Games.Contracts;
using Riwexoyd.ExternalSearch.Games.Converters;

using System.Text.RegularExpressions;

namespace Riwexoyd.ExternalSearch.Games.Services
{
    internal sealed class SoftclubExternalSearchProvider : ParseExternalSearchProvider
    {
        public override string SearchUri { get; } = "https://digital.softclub.ru/products/search?query={0}";

        public override Guid Uid { get; } = new Guid("{390B5D6F-C7A4-4DB9-B846-8935C58E2D8D}");

        public override string Name { get; } = "SoftClub (https://digital.softclub.ru/)";

        public override Uri BaseLinkUri { get; } = new Uri("https://digital.softclub.ru/");

        protected override IEnumerable<GameSearchResult> ParseDocument(IDocument document)
        {
            IHtmlCollection<IElement> shopItems = document.QuerySelectorAll("a.game-card");
            List<GameSearchResult> gameSearchResults = new(shopItems.Length);

            foreach (IElement shopItem in shopItems)
            {
                IElement titleElement = shopItem.QuerySelector("h3")!;
                IElement priceElement = shopItem.QuerySelector("span.price-card")!;

                string? link = shopItem.GetAttribute("href");

                if (string.IsNullOrEmpty(link))
                    continue;

                string stringPrice = priceElement?.TextContent ?? string.Empty;

                stringPrice = string.Join("", stringPrice.Split(' '));

                Match costMatch = NullableIntParseConverter.DigitalRegex.Match(stringPrice);
                int? price = null;

                if (costMatch.Success && int.TryParse(costMatch.Value, out int tempPrice))
                    price = tempPrice;

                gameSearchResults.Add(new GameSearchResult
                {
                    GameTitle = titleElement.TextContent,
                    Price = price,
                    ProviderUid = Uid,
                    Url = GetGameLink(link)
                });
            }

            return gameSearchResults;
        }
    }
}
