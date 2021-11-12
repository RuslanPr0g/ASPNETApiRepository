using Microsoft.Extensions.DependencyInjection;
using YTScrapper.Application.Contracts;
using YTScrapper.Application.Services;

namespace YTScrapper.Application.Extentions
{
    public static class DiExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ISearchService, SearchService>();

            return services;
        }
    }
}
