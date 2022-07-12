using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Riwexoyd.ExternalSearch.Services.Configurations;
using Riwexoyd.ExternalSearch.Services.Contracts;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Riwexoyd.ExternalSearch.Services.Implementations
{
    /// <summary>
    /// Сервис регистрации телеграм бота при запуске приложения
    /// </summary>
    internal sealed class TelegramBotHostService : IHostedService
    {
        private readonly ILogger<TelegramBotHostService> _logger;
        private readonly IServiceProvider _services;
        private readonly IOptions<TelegramBotConfiguration> _botConfig;

        public TelegramBotHostService(ILogger<TelegramBotHostService> logger,
            IServiceProvider services,
            IOptions<TelegramBotConfiguration> botConfig)
        {
            _logger = logger;
            _services = services;
            _botConfig = botConfig;
        }

        /// <summary>
        /// Запуск приложения: регистрируем телеграм бота в телеграме по указанному адресу
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Задача</returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            // регистрируем бота по адресу: url/bot/token
            var webhookAddress = @$"{_botConfig.Value.Url}/bot/{_botConfig.Value.Token}";
            _logger.LogInformation("Setting webhook: {0}", webhookAddress);
            await botClient.SetWebhookAsync(
                url: webhookAddress,
                allowedUpdates: Array.Empty<UpdateType>(),
                cancellationToken: cancellationToken);

            // Устанавливаем команды для бота
            IEnumerable<BotCommand> botCommands = scope.ServiceProvider.GetServices<IBotCommandHandler>().Select(i => new BotCommand
            {
                Command = i.Name,
                Description = i.Description
            });

            await botClient.SetMyCommandsAsync(botCommands, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Остановка приложения: удаляем у телеграм бота адрес регистрации
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Задача</returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_botConfig.Value?.DeleteWebHook == true)
            {
                using var scope = _services.CreateScope();
                var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

                _logger.LogInformation("Removing webhook");
                await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
            }
        }
    }
}
