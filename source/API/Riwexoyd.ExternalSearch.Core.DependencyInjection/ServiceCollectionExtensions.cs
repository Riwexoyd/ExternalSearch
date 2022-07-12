using Microsoft.Extensions.DependencyInjection;

using Riwexoyd.ExternalSearch.Core.Contracts;
using Riwexoyd.ExternalSearch.Core.Services;

namespace Riwexoyd.ExternalSearch.Core.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExternalSearch<TSearchOptions, TSearchResult>(this IServiceCollection services)
            where TSearchOptions : ISearchOptions, new()
            where TSearchResult : ISearchResult, new()
        {
            services.AddTransient<IExternalSearchService<TSearchOptions, TSearchResult>, ExternalSearchService<TSearchOptions, TSearchResult>>();

            var externalSearchProviderTypes = GetExternalSearchProviderTypes<TSearchOptions, TSearchResult>();
            var externalSearchProviderBaseType = typeof(IExternalSearchProvider<TSearchOptions, TSearchResult>);

            foreach (var extenalSearchProvider in externalSearchProviderTypes)
            {
                services.AddTransient(externalSearchProviderBaseType, extenalSearchProvider);
            }

            return services;
        }

        private static IEnumerable<Type> GetExternalSearchProviderTypes<TSearchOptions, TSearchResult>()
            where TSearchOptions : ISearchOptions, new()
            where TSearchResult : ISearchResult, new()
        {
            var scanType = typeof(IExternalSearchProvider<TSearchOptions, TSearchResult>);
            var implimentationType = typeof(TSearchOptions);
            return implimentationType.Assembly
                .GetTypes()
                .Where(type => !type.IsAbstract && !type.IsInterface && scanType.IsAssignableFrom(type))
                .ToList();
        }
    }
}
