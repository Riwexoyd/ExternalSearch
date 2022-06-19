using Riwexoyd.ExternalSearch.Core.Contracts;
using Riwexoyd.ExternalSearch.Games.Contracts;

namespace Riwexoyd.ExternalSearch.Games.Services
{
    internal abstract class GameExternalSearchProvider : IExternalSearchProvider<GameSearchOptions, GameSearchResult>
    {
        public abstract string SearchUri { get; }

        public abstract Guid Uid { get; }

        public abstract string Name { get; }

        public abstract Uri BaseLinkUri { get; }

        public abstract Task<IEnumerable<GameSearchResult>> SearchAsync(GameSearchOptions options, CancellationToken cancellationToken);

        public string GetGameLink(string? relativeUri)
        {
            return new Uri(BaseLinkUri, relativeUri).AbsoluteUri;
        }

        public string GetSearchUri(GameSearchOptions gameSearchOptions)
        {
            return string.Format(SearchUri, Uri.EscapeDataString(gameSearchOptions?.GameTitle ?? string.Empty));
        }
    }
}
