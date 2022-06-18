namespace Riwexoyd.ExternalSearch.Core.Contracts
{
    public interface IExternalSearchService<TSearchOptions, TSearchResult>
        where TSearchOptions : ISearchOptions, new()
        where TSearchResult : ISearchResult, new()
    {
        Task<IEnumerable<TSearchResult>> SearchAsync(TSearchOptions searchOptions, CancellationToken cancellationToken);
    }
}
