using Riwexoyd.ExternalSearch.Core.Contracts;

namespace Riwexoyd.ExternalSearch.Core.Services
{
    internal sealed class ExternalSearchService<TSearchOptions, TSearchResult> : IExternalSearchService<TSearchOptions, TSearchResult>
        where TSearchOptions : ISearchOptions, new()
        where TSearchResult : ISearchResult, new()
    {
        private readonly IDictionary<Guid, IExternalSearchProvider<TSearchOptions, TSearchResult>> _externalSearchProviders;

        public ExternalSearchService(IEnumerable<IExternalSearchProvider<TSearchOptions, TSearchResult>> externalSearchProviders)
        {
            _externalSearchProviders = externalSearchProviders.ToDictionary(i => i.Uid);
        }

        public async Task<IEnumerable<TSearchResult>> SearchAsync(TSearchOptions searchOptions, CancellationToken cancellationToken)
        {
            ICollection<IExternalSearchProvider<TSearchOptions, TSearchResult>> searchProviders = FilterProviders(searchOptions);
            List<Task<IEnumerable<TSearchResult>>> searchTasks = new List<Task<IEnumerable<TSearchResult>>>(searchProviders.Count);

            foreach (var provider in searchProviders)
            {
                searchTasks.Add(provider.SearchAsync(searchOptions, cancellationToken));
            }

            IEnumerable<TSearchResult>[]? result = await Task.WhenAll(searchTasks);

            return result.SelectMany(i => i);
        }

        private ICollection<IExternalSearchProvider<TSearchOptions, TSearchResult>> FilterProviders(TSearchOptions searchOptions)
        {
            if (!searchOptions.FilterProviders)
                return _externalSearchProviders.Values;

            if (searchOptions.Providers == null || !searchOptions.Providers.Any())
                return Array.Empty<IExternalSearchProvider<TSearchOptions, TSearchResult>>();

            List<IExternalSearchProvider<TSearchOptions, TSearchResult>> providers = 
                new List<IExternalSearchProvider<TSearchOptions, TSearchResult>>(searchOptions.Providers.Count);

            foreach (var provider in searchOptions.Providers)
            {
                if (!_externalSearchProviders.TryGetValue(provider, out var searchProvider)) continue;
                providers.Add(searchProvider);
            }

            return providers;
        }
    }
}
