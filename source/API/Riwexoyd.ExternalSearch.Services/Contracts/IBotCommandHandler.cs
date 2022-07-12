namespace Riwexoyd.ExternalSearch.Services.Contracts
{
    /// <summary>
    /// Интерфейс команды бота
    /// </summary>
    internal interface IBotCommandHandler
    {
        /// <summary>
        /// Название команды
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Описание команды
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Обработать команду
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Задача</returns>
        Task Handle(long chatId, long userId, CancellationToken cancellationToken);
    }
}
