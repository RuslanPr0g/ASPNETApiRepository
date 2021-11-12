using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using YTScrapper.Application.Contracts;
using YTScrapper.Application.DTOs;
using YTScrapper.Application.Enums;
using YTScrapper.Application.Exceptions;
using YTScrapper.Shared.Models;

namespace YTScrapper.Infrastructure.Scrapers
{
    public abstract class ScraperBase : ISearchScrapper
    {
        private const string LogFormat = "StatusCode: '{code}', scraper '{scraperName}' {scraperMessage} in {time}";

        private readonly ILogger<ISearchScrapper> _logger;
        private readonly ISearchService _searchService;

        public ScraperBase(ILogger<ISearchScrapper> logger, ISearchService searchService)
        {
            _logger = logger;
            _searchService = searchService;
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
                await _searchService.AddSearchItem(response.Item);
                resultOrNull = response;
                code = ScraperStatusCode.Success;
            }
            catch (YoutubeWrongVideoUrlException exception)
            {
                code = ScraperStatusCode.HandledError;
                resultOrNull = ValueOrNull<SearchResult>.CreateNull(exception.Message);
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
