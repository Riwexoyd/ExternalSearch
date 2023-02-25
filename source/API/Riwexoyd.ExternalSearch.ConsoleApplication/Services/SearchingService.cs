using Microsoft.Extensions.DependencyInjection;

using Riwexoyd.ExternalSearch.Core.Contracts;
using Riwexoyd.ExternalSearch.Core.DependencyInjection;
using Riwexoyd.ExternalSearch.Games.Contracts;

using System.Text;

namespace Riwexoyd.ExternalSearch.ConsoleApplication.Services
{
    internal class SearchingService : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;

        public SearchingService()
        {
            ServiceCollection services = new();
            services.AddExternalSearch<GameSearchOptions, GameSearchResult>();
            _serviceProvider = services.BuildServiceProvider();
        }

        public void Dispose()
        {
            _serviceProvider.Dispose();
        }

        public Task<string> SearchAsync(string text, CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            IExternalSearchService<GameSearchOptions, GameSearchResult> externalSearchService
                = scope.ServiceProvider.GetRequiredService<IExternalSearchService<GameSearchOptions, GameSearchResult>>();

            return SearchGamesAsync(externalSearchService, text, cancellationToken);
        }

        private static async Task<string> SearchGamesAsync(IExternalSearchService<GameSearchOptions, GameSearchResult> externalSearchService, string gameTitle, CancellationToken cancellationToken)
        {
            IEnumerable<GameSearchResult> searchResult;
            try
            {
                searchResult = await externalSearchService.SearchAsync(new GameSearchOptions
                {
                    FilterProviders = false,
                    GameTitle = gameTitle
                }, cancellationToken);
            }
            catch (Exception e)
            {
                return $"Произошла ошибка при поиске игр: {e.Message}";
            }

            if (!searchResult.Any())
            {
                return "Поиск не дал результатов";
            }

            return BuildResultString(searchResult, gameTitle);
        }

        private static string BuildResultString(IEnumerable<GameSearchResult> searchResult, string gameTitle)
        {
            var clearedMessage = RemoveSpecialCharacters(gameTitle);

            IEnumerable<GameSearchResult> orderedSearchResult = searchResult
                .OrderByDescending(result => RemoveSpecialCharacters(result.GameTitle).Contains(clearedMessage, StringComparison.OrdinalIgnoreCase))
                .ThenByDescending(result => result.Price.HasValue)
                .ThenBy(result => result.Price)
                .ThenBy(result => result.GameTitle, StringComparer.OrdinalIgnoreCase);

            StringBuilder resultMessageBuilder = new();
            resultMessageBuilder.AppendLine($"Результаты поиска \"{gameTitle}\":");
            resultMessageBuilder.AppendLine();
            int item = 1;
            foreach (var result in orderedSearchResult)
            {
                resultMessageBuilder.Append($"{item}. {result.GameTitle}");
                if (result.Price.HasValue)
                    resultMessageBuilder.Append($" [{result.Price}]");
                else
                    resultMessageBuilder.Append(" [Цена отсутствует]");

                resultMessageBuilder.AppendLine();
                resultMessageBuilder.Append(result.Url);
                resultMessageBuilder.AppendLine();
                resultMessageBuilder.AppendLine();
                item++;
            }

            return resultMessageBuilder.ToString();
        }

        private static string RemoveSpecialCharacters(string? input)
        {
            return string.IsNullOrWhiteSpace(input) ? string.Empty : new string(input.Where(character => char.IsLetterOrDigit(character)).ToArray());
        }
    }
}
