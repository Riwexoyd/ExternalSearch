using Riwexoyd.ExternalSearch.Services.Contracts;

using Telegram.Bot;

namespace Riwexoyd.ExternalSearch.Services.Implementations
{
    internal sealed class StartBotCommandHandler : IBotCommandHandler
    {
        private readonly ITelegramBotClient _telegramBotClient;

        public StartBotCommandHandler(ITelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        public string Name { get; } = "/start";

        public string Description { get; } = "Отображает информацию по использованию бота";

        public async Task Handle(long chatId, long userId, CancellationToken cancellationToken)
        {
            await _telegramBotClient.SendTextMessageAsync(chatId, @"Данный бот ищет ключи видеоигр по различным сайтам и выводит список результатов.
Для использования бота просто отправьте в сообщении название искомой игры.", cancellationToken: cancellationToken);
        }
    }
}
