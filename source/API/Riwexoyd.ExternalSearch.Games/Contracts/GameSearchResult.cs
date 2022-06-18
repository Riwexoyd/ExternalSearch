using Riwexoyd.ExternalSearch.Core.Contracts;

namespace Riwexoyd.ExternalSearch.Games.Contracts
{
    public sealed class GameSearchResult : ISearchResult
    {
        public Guid ProviderUid { get; internal set; }

        public string? GameTitle { get; internal set; }

        public int? Price { get; internal set; }

        public string? Url { get; internal set; }
    }
}
