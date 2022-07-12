using Riwexoyd.ExternalSearch.Games.Contracts;
using Riwexoyd.ExternalSearch.Games.Models;

using System.Text.Json;

namespace Riwexoyd.ExternalSearch.Games.Services
{
    internal sealed class ZakaExternalSearchProvider : HttpGameExternalSearchProvider
    {
        public override Guid Uid { get; } = new Guid("{81DDECCD-9E81-4C64-B0EA-F6C20D803563}");

        public override string Name { get; } = "Zaka-Zaka (https://zaka-zaka.com/)";

        public override Uri BaseLinkUri { get; } = new Uri("https://zaka-zaka.com/game/");

        public override string SearchUri { get; } = "https://zaka-zaka.com/search/ajax/?game={0}";

        protected override async Task<IEnumerable<GameSearchResult>> GetDataFromStream(Stream stream, CancellationToken cancellationToken)
        {
            ZakaSearchResult[]? data = await JsonSerializer.DeserializeAsync<ZakaSearchResult[]>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            }, cancellationToken);

            if (data == null)
                return Enumerable.Empty<GameSearchResult>();

            return data.Where(i => !string.IsNullOrEmpty(i.Url) && i.Type.ToUpper() == "GAME").Select(Map);
        }

        private GameSearchResult Map(ZakaSearchResult game)
        {
            return new GameSearchResult
            {
                Price = game.Price,
                ProviderUid = Uid,
                GameTitle = game.Name,
                Url = GetGameLink(game.Url)
            };
        }
    }
}
