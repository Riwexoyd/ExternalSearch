using Riwexoyd.ExternalSearch.Core.Contracts;
using Riwexoyd.ExternalSearch.Games.Contracts;

namespace Riwexoyd.ExternalSearch.Games.Services
{
    internal abstract class GameExternalSearchProvider : IExternalSearchProvider<GameSearchOptions, GameSearchResult>
    {
        public static readonly HttpClient HttpClient = new HttpClient();

        public abstract Guid Uid { get; }

        public abstract string Name { get; }

        public abstract Task<IEnumerable<GameSearchResult>> SearchAsync(GameSearchOptions options, CancellationToken cancellationToken);
    }
}
