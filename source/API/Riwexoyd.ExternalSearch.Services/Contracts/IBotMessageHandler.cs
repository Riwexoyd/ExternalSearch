namespace Riwexoyd.ExternalSearch.Services.Contracts
{
    internal interface IBotMessageHandler
    {
        /// <summary>
        /// Обработать сообщение
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="message">Сообщение</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Задача</returns>
        Task Handle(long chatId, long userId, string message, CancellationToken cancellationToken);
    }
}
