using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YTSearch.Application.Contracts;
using YTSearch.Infrastructure.Repository;
using YTSearch.Infrastructure.Runners;
using YTSearch.Infrastructure.Services;

namespace YTSearch.Infrastructure
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
