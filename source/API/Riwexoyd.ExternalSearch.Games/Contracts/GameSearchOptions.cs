using Riwexoyd.ExternalSearch.Core.Contracts;

namespace Riwexoyd.ExternalSearch.Games.Contracts
{
    public sealed class GameSearchOptions : ISearchOptions
    {
        public IReadOnlyCollection<Guid> Providers { get; } = Array.Empty<Guid>();

        public string GameTitle { get; set; }

        public bool FilterProviders { get; set; }
    }
}
