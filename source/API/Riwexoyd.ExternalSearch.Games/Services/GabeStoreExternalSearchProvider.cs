﻿using AngleSharp.Dom;

using Riwexoyd.ExternalSearch.Games.Contracts;
using Riwexoyd.ExternalSearch.Games.Converters;

using System.Text.RegularExpressions;

namespace Riwexoyd.ExternalSearch.Games.Services
{
    internal sealed class GabeStoreExternalSearchProvider : ParseExternalSearchProvider
    {
        public override string SearchUri { get; } = "https://gabestore.ru/result?ProductFilter%5Bsearch%5D={0}";

        public override Guid Uid { get; } = new Guid("{DF36F4AB-BFEC-4789-94AC-5CC20F3133BC}");

        public override string Name { get; } = "GabeStore (https://gabestore.ru/)";

        public override Uri BaseLinkUri { get; } = new Uri("https://gabestore.ru/");

        protected override IEnumerable<GameSearchResult> ParseDocument(IDocument document)
        {
            IHtmlCollection<IElement> shopItems = document.QuerySelectorAll("div.shop-item");
            List<GameSearchResult> gameSearchResults = new(shopItems.Length);

            foreach (IElement shopItem in shopItems)
            {
                IElement titleElemement = shopItem.QuerySelector("a.shop-item__name")!;
                IElement costElement = shopItem.QuerySelector("div.shop-item__price-current")!;

                string? link = titleElemement.GetAttribute("href");

                if (string.IsNullOrEmpty(link))
                    continue;

                Match costMatch = NullableIntParseConverter.DigitalRegex.Match(costElement.TextContent);
                int? price = null;

                if (costMatch.Success && int.TryParse(costMatch.Value, out int tempPrice))
                    price = tempPrice;

                gameSearchResults.Add(new GameSearchResult
                {
                    GameTitle = titleElemement.TextContent,
                    Price = price,
                    ProviderUid = Uid,
                    Url = GetGameLink(link)
                });
            }

            return gameSearchResults;
        }
    }
}
