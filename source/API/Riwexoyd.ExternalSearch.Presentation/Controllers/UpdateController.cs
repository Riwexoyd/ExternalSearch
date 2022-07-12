using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Riwexoyd.ExternalSearch.Services.Contracts;

using Telegram.Bot.Types;

namespace Riwexoyd.ExternalSearch.Presentation.Controllers
{
    /// <summary>
    /// Контроллер получения обновлений от телеграма
    /// </summary>
    public sealed class UpdateController : ControllerBase
    {
        private readonly ITelegramBotUpdateHandler _telegramBotUpdateHandler;
        private readonly ILogger<UpdateController> _logger;

        public UpdateController(ITelegramBotUpdateHandler telegramBotUpdateHandler, ILogger<UpdateController> logger)
        {
            _telegramBotUpdateHandler = telegramBotUpdateHandler;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Receiving data from Telegram");
            await _telegramBotUpdateHandler.HandleUpdate(update, cancellationToken);

            return Ok();
        }
    }
}
