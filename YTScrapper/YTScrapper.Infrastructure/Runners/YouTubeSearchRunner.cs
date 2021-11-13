using Google.Apis.Services;
using Google.Apis.Util;
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
using YTScrapper.Application.DTOs;
using YTScrapper.Application.Enums;
using YTScrapper.Application.Exceptions;
using YTScrapper.Application.Result;
using YTScrapper.Domain.Models;
using YTScrapper.Shared.Extentions;
using YTScrapper.Shared.Helper;
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

        public async Task<SuccessOrFailure<YouTubeModel>> Run(SearchRunnerRequest searchRunnerRequest, CancellationToken token = default)
        {
            var watch = Stopwatch.StartNew();
            var searchResult = await PerformSearch(searchRunnerRequest.Url, token);
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
            var videoId = YouTubeHelper.GetYouTubeVideoIdFromUrl(url);

            if (videoId.IsEmpty())
            {
                throw new YoutubeWrongVideoUrlException(url);
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _configuration.GetValue<string>("UserSecretGoogleAPI"),
                ApplicationName = this.GetType().ToString()
            });

            var videoSearchRequest = youtubeService.Videos.List("snippet, ContentDetails");
            videoSearchRequest.Id = videoId;
            videoSearchRequest.MaxResults = 1;

            var searchListResponse = await videoSearchRequest.ExecuteAsync(token);

            var videoData = searchListResponse.Items.FirstOrDefault(i => i.Id == videoId);

            if (videoData is null)
            {
                throw new YoutubeWrongVideoUrlException(url);
            }

            string duration = GetDurationFromYouTubeForamt(videoData.ContentDetails?.Duration);

            return new YouTubeModel
            {
                Url = url,
                ImagePreviewUrl = videoData.Snippet.Thumbnails.High.Url,
                Title = videoData.Snippet.Title,
                Description = videoData.Snippet.Description,
                Author = videoData.Snippet.ChannelTitle,
                Duration = duration,
            };
        }

        // PT1H3M56S -> 01:03:56
        private string GetDurationFromYouTubeForamt(string duration)
        {
            if (duration.IsEmpty())
            {
                return null;
            }

            var hours = RegexHelper.GetNMatchFromRegexPattern(@"PT(\d+H)?(\d+M)?(\d+S)?", duration, 1);
            var minutes = RegexHelper.GetNMatchFromRegexPattern(@"PT(\d+H)?(\d+M)?(\d+S)?", duration, 2);
            var seconds = RegexHelper.GetNMatchFromRegexPattern(@"PT(\d+H)?(\d+M)?(\d+S)?", duration, 3);

            if (duration.Contains("H"))
            {
                hours = hours.Replace("H", "");
            }
            else
            {
                hours = "0";
            }

            if (duration.Contains("M"))
            {
                minutes = minutes.Replace("M", "");
            }
            else
            {
                minutes = "0";
            }

            if (duration.Contains("S"))
            {
                seconds = seconds.Replace("S", "");
            }
            else
            {
                seconds = "0";
            }

            return $"{hours}:{minutes}:{seconds}";
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
