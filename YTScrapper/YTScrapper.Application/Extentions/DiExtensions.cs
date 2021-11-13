using Microsoft.Extensions.DependencyInjection;
using YTSearch.Application.Contracts;
using YTSearch.Application.Services;

namespace YTSearch.Application.Extentions
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
