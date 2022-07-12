using Microsoft.Extensions.Logging;

using Riwexoyd.ExternalSearch.Services.Contracts;

using Telegram.Bot.Types;

namespace Riwexoyd.ExternalSearch.Services.Implementations
{
    internal sealed class TelegramBotUpdateHandler : ITelegramBotUpdateHandler
    {
        private readonly ILogger<TelegramBotUpdateHandler> _logger;
        private readonly IEnumerable<IBotCommandHandler> _botCommands;
        private readonly IEnumerable<IBotMessageHandler> _messageHandlers;

        public TelegramBotUpdateHandler(ILogger<TelegramBotUpdateHandler> logger, IEnumerable<IBotCommandHandler> botCommands, IEnumerable<IBotMessageHandler> messageHandlers)
        {
            _logger = logger;
            _botCommands = botCommands;
            _messageHandlers = messageHandlers;
        }

        /// <summary>
        /// Обработать обновление
        /// </summary>
        /// <param name="update">Обновление</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Задача</returns>
        public async Task HandleUpdate(Update update, CancellationToken cancellationToken)
        {
            switch (update.Type)
            {
                case Telegram.Bot.Types.Enums.UpdateType.Message:
                    await HandleMessage(update.Message, cancellationToken);
                    break;
            }
        }

        private async Task HandleMessage(Message message, CancellationToken cancellationToken)
        {
            var chatId = message.Chat.Id;
            var userId = message.From.Id;
            var text = message.Text;
            if (text.StartsWith('/'))
            {
                var command = _botCommands.FirstOrDefault(i => text.StartsWith(i.Name));
                if (command == null)
                {
                    _logger.LogDebug("TelegramBotUpdateHandler: unknown command {0}", message.Text);
                    return;
                }

                _logger.LogDebug("TelegramBotUpdateHandler handling command");
                await command.Handle(chatId, userId, cancellationToken);
            }
            else
            {
                await Task.WhenAll(_messageHandlers.Select(handler => handler.Handle(chatId, userId, text, cancellationToken)));
            }
        }
    }
}
