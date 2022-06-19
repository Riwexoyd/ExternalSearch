using Riwexoyd.ExternalSearch.Games.Contracts;
using Riwexoyd.ExternalSearch.Games.Models;

using System.Text.Json;

namespace Riwexoyd.ExternalSearch.Games.Services
{
    internal sealed class SteamPayExternalSearchProvider : GetGameExternalSearchProvider
    {
        public override Guid Uid { get; } = new Guid("{3BA27E1B-E93A-4C5E-B1B6-DDC84B4FA156}");

        public override string Name { get; } = "SteamPay (https://steampay.com/)";

        public override Uri BaseLinkUri { get; } = new("https://steampay.com/game/");

        protected override string SearchUri { get; } = "https://steampay.com/ajax/autocomplete/search?query={0}";

        protected override async Task<IEnumerable<GameSearchResult>> GetDataFromStream(Stream stream, CancellationToken cancellationToken)
        {
            SteamPaySearchResult? data = await JsonSerializer.DeserializeAsync<SteamPaySearchResult>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            }, cancellationToken);

            if (data == null)
                return Enumerable.Empty<GameSearchResult>();

            return data.Suggestions!.Where(i => !string.IsNullOrEmpty(i.Data.Link)).Select(Map);
        }

        private GameSearchResult Map(SteamPaySuggestion game)
        {
            return new GameSearchResult
            {
                Url = GetGameLink(game.Data.Link),
                Price = game.Data.Price,
                GameTitle = game.Data.Value,
                ProviderUid = Uid
            };
        }
    }
}
