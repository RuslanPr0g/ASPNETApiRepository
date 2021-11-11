using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YTScrapper.Application.Contracts;
using YTScrapper.Infrastructure.Config;
using YTScrapper.Infrastructure.Scrapers;
using YTScrapper.Infrastructure.Services;

namespace YTScrapper.Infrastructure
{
    public static class DIExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ISearchScrapperCollector, SearchScraperCollector>();
            services.AddScoped<YoutubeScraper>();
            services.AddScoped<IWebClientService, WebClientService>();
            services.AddScoped<DriverInitializer>();
            services.Configure<SeleniumConfig>(option =>
            {
                config.GetSection("Selenium").Bind(option);
            });

            return services;
        }
    }
}
