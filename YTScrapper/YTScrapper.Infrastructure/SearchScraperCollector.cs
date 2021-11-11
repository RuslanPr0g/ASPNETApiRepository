using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YTScrapper.Application.Contracts;
using YTScrapper.Application.Enums;
using YTScrapper.Infrastructure.Scrapers;

namespace YTScrapper.Infrastructure
{
    public class SearchScraperCollector : ISearchScrapperCollector
    {
        private static readonly Dictionary<SupportedWebsite, Type> _websiteNameToScraperType = new()
        {
            { SupportedWebsite.YouTube, typeof(YoutubeScraper) },
        };

        private readonly ILogger<SearchScraperCollector> _logger;
        private readonly IServiceProvider _provider;

        public SearchScraperCollector(ILogger<SearchScraperCollector> logger, IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        public ValueTask<IEnumerable<ISearchScrapper>> CollectFor(List<SupportedWebsite> websites)
        {
            List<ISearchScrapper> scrapers = new();
            foreach (var website in websites)
            {
                if (HasScraper(website, out var scraperType))
                {
                    var result = _provider.GetService(scraperType);
                    if (result is ISearchScrapper scraper)
                    {
                        scrapers.Add(scraper);
                    }
                }
                else
                {
                    _logger.LogInformation("Unable to locate scraper for requested website '{0}'", website);
                }
            }
            return ValueTask.FromResult<IEnumerable<ISearchScrapper>>(scrapers);
        }

        private static bool HasScraper(SupportedWebsite website, out Type scraper)
        {
            return _websiteNameToScraperType.TryGetValue(website, out scraper);
        }
    }
}
