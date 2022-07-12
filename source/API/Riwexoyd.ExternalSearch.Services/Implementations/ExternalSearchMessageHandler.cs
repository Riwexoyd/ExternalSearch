using Microsoft.Extensions.Logging;

using Riwexoyd.ExternalSearch.Core.Contracts;
using Riwexoyd.ExternalSearch.Games.Contracts;
using Riwexoyd.ExternalSearch.Services.Contracts;

using System.Text;

using Telegram.Bot;

namespace Riwexoyd.ExternalSearch.Services.Implementations
{
    internal sealed class ExternalSearchMessageHandler : IBotMessageHandler
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IExternalSearchService<GameSearchOptions, GameSearchResult> _searchService;
        private readonly ILogger<ExternalSearchMessageHandler> _logger;

        public ExternalSearchMessageHandler(ITelegramBotClient telegramBotClient, IExternalSearchService<GameSearchOptions, GameSearchResult> searchService, ILogger<ExternalSearchMessageHandler> logger)
        {
            _telegramBotClient = telegramBotClient;
            _searchService = searchService;
            _logger = logger;
        }

        public async Task Handle(long chatId, long userId, string message, CancellationToken cancellationToken)
        {
            _logger.LogDebug("ExternalSearchMessageHandler search started");

            IEnumerable<GameSearchResult> searchResult;
            try
            {
                searchResult = await _searchService.SearchAsync(new GameSearchOptions
                {
                    FilterProviders = false,
                    GameTitle = message
                }, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ExternalSearchMessageHandler Error while searching game");
                await _telegramBotClient.SendTextMessageAsync(chatId, $"Произошла ошибка при поиске игр: {e.Message}", cancellationToken: cancellationToken);
                return;
            }

            _logger.LogDebug("ExternalSearchMessageHandler search finished");

            if (!searchResult.Any())
            {
                await _telegramBotClient.SendTextMessageAsync(chatId, "Поиск не дал результатов", cancellationToken: cancellationToken);
                return;
            }

            IEnumerable<GameSearchResult> enumerable = searchResult.OrderBy(result => result.GameTitle)
                .ThenBy(result => result.Price);

            int page = 1;
            while (enumerable.Any())
            {
                StringBuilder resultMessageBuilder = new StringBuilder();
                resultMessageBuilder.AppendLine($"Результаты поиска {page}:");
                page++;
                foreach (var result in enumerable.Take(15))
                {
                    resultMessageBuilder.Append($"{result.GameTitle}");
                    if (result.Price.HasValue)
                        resultMessageBuilder.Append($" [{result.Price}]");
                    resultMessageBuilder.Append($" - {result.Url}");
                    resultMessageBuilder.AppendLine();
                }

                enumerable = enumerable.Skip(15);

                await _telegramBotClient.SendTextMessageAsync(chatId, resultMessageBuilder.ToString(), cancellationToken: cancellationToken);
            }

            _logger.LogDebug("ExternalSearchMessageHandler handle finished");
        }
    }
}
