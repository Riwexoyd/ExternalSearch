namespace Riwexoyd.ExternalSearch.Core.Contracts
{
    public interface IExternalSearchProvider<TSearchOptions, TSearchResult>
        where TSearchOptions : ISearchOptions, new()
        where TSearchResult : ISearchResult, new()
    {
        Guid Uid { get; }

        string Name { get; }

        Task<IEnumerable<TSearchResult>> SearchAsync(TSearchOptions options, CancellationToken cancellationToken);
    }
}
