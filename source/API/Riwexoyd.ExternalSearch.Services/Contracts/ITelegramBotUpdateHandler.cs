using Telegram.Bot.Types;

namespace Riwexoyd.ExternalSearch.Services.Contracts
{
    public interface ITelegramBotUpdateHandler
    {
        Task HandleUpdate(Update update, CancellationToken cancellationToken);
    }
}
