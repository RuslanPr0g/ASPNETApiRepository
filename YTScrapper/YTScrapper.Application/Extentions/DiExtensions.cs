using Microsoft.Extensions.DependencyInjection;
using YTScrapper.Application.Runners;

namespace YTScrapper.Application.Extentions
{
    public static class DiExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IScraperRunner, ScraperRunner>();

            return services;
        }
    }
}
