using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
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

        public async Task<SuccessOrFailure<YouTubeSearchItem>> Run(string url, CancellationToken token = default)
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
            SuccessOrFailure<YouTubeSearchItem> resultOrNull;
            try
            {
                resultOrNull = await SearchVideoInner(url, token);
                code = SearchStatusCode.Success;
            }
            catch (YoutubeWrongVideoUrlException exception)
            {
                code = SearchStatusCode.HandledError;
                resultOrNull = SuccessOrFailure<YouTubeSearchItem>.CreateNull(exception.Message);
            }
            catch (Exception exception)
            {
                code = SearchStatusCode.UnhandledError;
                resultOrNull = SuccessOrFailure<YouTubeSearchItem>.CreateNull(exception.Message);
            }

            return new SearchResult
            {
                Result = resultOrNull,
                StatusCode = code
            };
        }

        private async Task<YouTubeSearchItem> SearchVideoInner(string url, CancellationToken token)
        {
            return await ScrapVideo(url);

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _configuration.GetValue<string>("UserSecretGoogleAPI"),
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = "Google";
            searchListRequest.MaxResults = 50;

            var searchListResponse = await searchListRequest.ExecuteAsync(token);

            return null;
        }

        private void LogRun(SearchResult searchResult, TimeSpan ranFor)
        {
            var time = ranFor.ToString();
            var scraperName = GetType().Name;
            var codeMessage = searchResult.StatusCode.ToString("g");

            _logger.LogInformation(LogFormat, codeMessage, scraperName, codeMessage, time);
        }

        private async Task<YouTubeSearchItem> ScrapVideo(string searchUrl)
        {
            if (searchUrl.Contains("www.youtube.com/watch?v=") is false &&
                searchUrl.Contains("https://youtu.be") is false)
            {
                throw new YoutubeWrongVideoUrlException(searchUrl);
            }

            var searchItem = new YouTubeSearchItem()
            {
                Url = searchUrl
            };

            using var client = await _clientProvider.Provide();

            await _clientProvider.SetDefaultUserString(client);
            var htmlSearchPage = client.DownloadString(searchUrl);

            string titleRegex = @"""title"":\s*""([^""]+)"",";
            searchItem.Title = GetFirstMatchFromRegexPattern(titleRegex, htmlSearchPage);

            string descriptionRegex = @"""shortDescription"":\s*""([^""]+)"",";
            searchItem.Description = GetFirstMatchFromRegexPattern(descriptionRegex, htmlSearchPage);

            string authorRegex = @"""channelName"":\s*""([^""]+)"",";
            searchItem.Author = GetFirstMatchFromRegexPattern(authorRegex, htmlSearchPage);

            string durationRegex = @"""approxDurationMs"":\s*""([^""]+)"",";
            var duration = GetFirstMatchFromRegexPattern(durationRegex, htmlSearchPage);

            if (searchItem.Title.IsEmpty() || searchItem.Description.IsEmpty() ||
                searchItem.Author.IsEmpty() || searchItem.Duration.IsEmpty())
            {
                throw new YoutubeWrongVideoUrlException(searchUrl);
            }

            var durationTimeSpan = TimeSpan.FromMilliseconds(Convert.ToDouble(duration));
            searchItem.Duration = $"{durationTimeSpan.Hours}:{durationTimeSpan.Minutes}:{durationTimeSpan.Seconds}";

            return searchItem;
        }

        private static string GetFirstMatchFromRegexPattern(string regex, string text)
        {
            Regex r = new(regex, RegexOptions.IgnoreCase);
            Match m = r.Match(text);
            if (!m.Success)
            {
                return string.Empty;
            }
            else
            {
                return m.Groups[1].Value;
            }
        }
    }
}
