using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using YTScrapper.Application.Contracts;
using YTScrapper.Application.Enums;
using YTScrapper.Application.Exceptions;
using YTScrapper.Application.Result;
using YTScrapper.Domain.Models;
using YTScrapper.Shared.Extentions;
using YTScrapper.Shared.Models;

namespace YTScrapper.Infrastructure.Runners
{
    public class YouTubeSearchRunner : ISearchRunner
    {
        private const string LogFormat = "StatusCode: '{code}', scraper '{scraperName}' {scraperMessage} in {time}";

        private readonly IWebClientService _clientProvider;
        private readonly IConfiguration _configuration;
        private readonly ILogger<YouTubeSearchRunner> _logger;

        public YouTubeSearchRunner(ILogger<YouTubeSearchRunner> logger, IWebClientService clientProvider,
            IConfiguration configuration)
        {
            _logger = logger;
            _clientProvider = clientProvider;
            _configuration = configuration;
        }

        public async Task<SuccessOrFailure<YouTubeModel>> Run(string url, CancellationToken token = default)
        {
            var watch = Stopwatch.StartNew();
            var searchResult = await PerformSearch(url, token);
            watch.Stop();

            var ranFor = watch.Elapsed;
            this.LogRun(searchResult, ranFor);

            return searchResult.Result;
        }

        private async Task<SearchResult> PerformSearch(string url, CancellationToken token)
        {
            SearchStatusCode code;
            SuccessOrFailure<YouTubeModel> resultOrNull;
            try
            {
                resultOrNull = await SearchVideoInner(url, token);
                code = SearchStatusCode.Success;
            }
            catch (YoutubeWrongVideoUrlException exception)
            {
                code = SearchStatusCode.HandledError;
                resultOrNull = SuccessOrFailure<YouTubeModel>.CreateNull(exception.Message);
            }
            catch (Exception exception)
            {
                code = SearchStatusCode.UnhandledError;
                resultOrNull = SuccessOrFailure<YouTubeModel>.CreateNull(exception.Message);
            }

            return new SearchResult
            {
                Result = resultOrNull,
                StatusCode = code
            };
        }

        private async Task<YouTubeModel> SearchVideoInner(string url, CancellationToken token)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _configuration.GetValue<string>("UserSecretGoogleAPI"),
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = url;
            searchListRequest.MaxResults = 50;

            var searchListResponse = await searchListRequest.ExecuteAsync(token);

            return new YouTubeModel
            {
                Author = searchListResponse.Items.First().Snippet.ChannelTitle
            };
        }

        private void LogRun(SearchResult searchResult, TimeSpan ranFor)
        {
            var time = ranFor.ToString();
            var scraperName = GetType().Name;
            var codeMessage = searchResult.StatusCode.ToString("g");

            _logger.LogInformation(LogFormat, codeMessage, scraperName, codeMessage, time);
        }
    }
}
