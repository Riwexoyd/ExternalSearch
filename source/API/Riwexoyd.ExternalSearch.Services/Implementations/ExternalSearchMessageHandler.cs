using Riwexoyd.ExternalSearch.Services.Contracts;

using Telegram.Bot;

namespace Riwexoyd.ExternalSearch.Services.Implementations
{
    internal sealed class ExternalSearchMessageHandler : IBotMessageHandler
    {
        private readonly ITelegramBotClient _telegramBotClient;

        public Task Handle(long chatId, long userId, string message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
