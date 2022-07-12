using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Riwexoyd.ExternalSearch.Core.DependencyInjection;
using Riwexoyd.ExternalSearch.Games.Contracts;
using Riwexoyd.ExternalSearch.Services.Configurations;
using Riwexoyd.ExternalSearch.Services.Contracts;
using Riwexoyd.ExternalSearch.Services.Implementations;

using Telegram.Bot;

namespace Riwexoyd.ExternalSearch.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSearchServices(this IServiceCollection services, IConfiguration configuration, string telegramBotSection)
        {
            var botConfig = configuration.GetSection(telegramBotSection).Get<TelegramBotConfiguration>();

            services.Configure<TelegramBotConfiguration>(configuration.GetSection(telegramBotSection));

            services.AddHttpClient(telegramBotSection)
                .AddTypedClient<ITelegramBotClient>(httpClient
                    => new TelegramBotClient(botConfig.Token, httpClient));

            services.AddHostedService<TelegramBotHostService>();

            services.AddTransient<ITelegramBotUpdateHandler, TelegramBotUpdateHandler>();

            services.AddExternalSearch<GameSearchOptions, GameSearchResult>();

            // Регистрируем команды бота
            var botCommandType = typeof(IBotCommandHandler);
            var commandTypes = GetImplimentationTypes(botCommandType);
            foreach (var commandType in commandTypes)
            {
                services.AddTransient(botCommandType, commandType);
            }

            // Регистрируем обработчики текста бота
            var botMessageHandlerType = typeof(IBotMessageHandler);
            var messageHandlerTypes = GetImplimentationTypes(botMessageHandlerType);
            foreach (var messageHandler in messageHandlerTypes)
            {
                services.AddTransient(botMessageHandlerType, messageHandler);
            }

            return services;
        }

        private static IEnumerable<Type> GetImplimentationTypes(Type type)
        {
            return type.Assembly
                .GetTypes()
                .Where(i => type.IsAssignableFrom(i) && i != type && !i.IsAbstract && !i.IsInterface)
                .ToList();
        }
    }
}
