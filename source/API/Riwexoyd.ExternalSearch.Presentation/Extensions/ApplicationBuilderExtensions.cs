using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

using Riwexoyd.ExternalSearch.Services.Configurations;

namespace Riwexoyd.ExternalSearch.Presentation.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureApplication(this IApplicationBuilder applicationBuilder, IConfiguration configuration, string telegramBotSection)
        {
            var botConfig = configuration.GetSection(telegramBotSection)
                .Get<TelegramBotConfiguration>();

            applicationBuilder.UseRouting();

            applicationBuilder.UseEndpoints(endpoints =>
            {
                // Регистрируем контроллер получения обновлений от телеграма по токену бота
                // Обновления будут приходить по адресу url/bot/{token}
                var token = botConfig.Token;
                endpoints.MapControllerRoute(name: "TelegramWebHook",
                                             pattern: $"bot/{token}",
                                             new { controller = "Update", action = "Post" });

                endpoints.MapControllers();
            });

            return applicationBuilder;
        }
    }
}
