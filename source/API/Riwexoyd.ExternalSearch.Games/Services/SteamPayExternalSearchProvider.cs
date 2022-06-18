using Riwexoyd.ExternalSearch.Games.Contracts;

namespace Riwexoyd.ExternalSearch.Games.Services
{
    internal sealed class SteamPayExternalSearchProvider : GetGameExternalSearchProvider
    {
        private static readonly Uri GameUri = new Uri("https://steampay.com/game/");

        public override Guid Uid { get; } = new Guid("{3BA27E1B-E93A-4C5E-B1B6-DDC84B4FA156}");

        public override string Name { get; } = "SteamPay (https://steampay.com/)";

        protected override string SearchUri { get; } = "https://steampay.com/ajax/autocomplete/search?query={0}";

        protected override Task<IEnumerable<GameSearchResult>> GetDataFromStream(Stream stream, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
