using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YTScrapper.Application.Contracts;
using YTScrapper.Infrastructure.Repository;
using YTScrapper.Infrastructure.Runners;
using YTScrapper.Infrastructure.Services;

namespace YTScrapper.Infrastructure
{
    public static class DIExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ISearchRunner, YouTubeSearchRunner>();
            services.AddScoped<ISearchItemRepository, YouTubeRepository>();
            services.AddScoped<IWebClientService, WebClientService>();

            return services;
        }
    }
}
