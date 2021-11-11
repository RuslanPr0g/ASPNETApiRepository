using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTScrapper.Application.Contracts;

namespace YTScrapper.Infrastructure.Scrapers
{
    public class YoutubeScraper : ScraperBase
    {
        public YoutubeScraper(ILogger<ISearchScrapper> logger) : base(logger)
        {
        }
    }
}
