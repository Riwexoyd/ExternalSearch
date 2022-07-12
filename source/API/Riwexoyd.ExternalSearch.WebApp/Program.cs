using Riwexoyd.ExternalSearch.Presentation.Extensions;
using Riwexoyd.ExternalSearch.Services.Extensions;

namespace Riwexoyd.ExternalSearch.WebApp
{
    public sealed class Program
    {
        private const string TelegramBotSection = "TelegramBot";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var services = builder.Services;
            var configuration = builder.Configuration;

            services.AddSearchServices(configuration, TelegramBotSection);
            services.AddServiceControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.ConfigureApplication(configuration, TelegramBotSection);

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}