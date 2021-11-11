using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using YTScrapper.Application.Contracts;
using YTScrapper.Application.DTOs;
using YTScrapper.Application.Enums;
using YTScrapper.Shared.Models;

namespace YTScrapper.Infrastructure.Scrapers
{
    public abstract class ScraperBase : ISearchScrapper
    {
        private const string LogFormat = "StatusCode: '{code}', scraper '{scraperName}' {scraperMessage} in {time}";

        private readonly ILogger<ISearchScrapper> _logger;

        public ScraperBase(ILogger<ISearchScrapper> logger)
        {
            _logger = logger;
        }

        public async Task<ValueOrNull<SearchResult>> Scrap(SearchRequest request, CancellationToken token = default)
        {
            var watch = Stopwatch.StartNew();
            var searchResult = await PerformScrapping(request, token);
            watch.Stop();

            var ranFor = watch.Elapsed;
            LogRun(searchResult, ranFor);

            return searchResult.Result;
        }

        private void LogRun(ExtendedSearchResult searchResult, System.TimeSpan ranFor)
        {
            var time = ranFor.ToString();
            var scraperName = GetType().Name;
            var codeMessage = searchResult.StatusCode.ToString();

            _logger.LogInformation(LogFormat, codeMessage, scraperName, codeMessage, time);
        }

        public async Task<ExtendedSearchResult> PerformScrapping(SearchRequest request, CancellationToken token)
        {
            ScraperStatusCode code;
            ValueOrNull<SearchResult> resultOrNull;
            try
            {
                var response = await ScrapVideoInner(request, token);
                resultOrNull = response;
                code = ScraperStatusCode.Success;
            }
            catch (Exception exception)
            {
                code = ScraperStatusCode.UnhandledError;
                resultOrNull = ValueOrNull<SearchResult>.CreateNull(exception.Message);
            }

            return new ExtendedSearchResult
            {
                Result = resultOrNull,
                StatusCode = code
            };
        }

        public virtual Task<SearchResult> ScrapVideoInner(SearchRequest request, CancellationToken token = default)
        {
            return null;
        }
    }
}
