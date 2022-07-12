using Microsoft.Extensions.DependencyInjection;

namespace Riwexoyd.ExternalSearch.Presentation.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceControllers(this IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson();

            return services;
        }
    }
}
