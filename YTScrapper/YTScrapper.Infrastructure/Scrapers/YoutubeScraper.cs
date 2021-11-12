using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using YTScrapper.Application.Contracts;
using YTScrapper.Application.DTOs;
using YTScrapper.Application.Exceptions;
using YTScrapper.Domain.Models;

namespace YTScrapper.Infrastructure.Scrapers
{
    public class YoutubeScraper : ScraperBase
    {
        private const string _websiteName = "YouTube";
        private readonly IWebClientService _clientProvider;

        public YoutubeScraper(ILogger<YoutubeScraper> logger, IWebClientService clientProvider, ISearchService searchService)
            : base(logger, searchService)
        {
            _clientProvider = clientProvider;
        }

        public override async Task<SearchResult> ScrapVideoInner(
            SearchRequest request,
            CancellationToken token = default)
        {
            var video = await ScrapVideo(request.SearchUrl);

            return new SearchResult(video)
            {
                Website = _websiteName
            };
        }

        private async Task<SearchItem> ScrapVideo(string searchUrl)
        {
            if (searchUrl.Contains("www.youtube.com/watch?v=") is false &&
                searchUrl.Contains("https://youtu.be") is false)
            {
                throw new YoutubeWrongVideoUrlException(searchUrl);
            }

            var searchItem = new SearchItem()
            {
                SearchItemUrl = searchUrl,
                ImagePreviewUrl = string.Empty
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
            var durationTimeSpan = TimeSpan.FromMilliseconds(Convert.ToDouble(duration));
            searchItem.Duration = $"{durationTimeSpan.Hours}:{durationTimeSpan.Minutes}:{durationTimeSpan.Seconds}";

            string thumbnailRegex = @"""url"":\s*""(https:\/\/i\.ytimg\.com\/vi\/[^""]+)"",";
            searchItem.ImagePreviewUrl = GetFirstMatchFromRegexPattern(thumbnailRegex, htmlSearchPage);

            return searchItem;
        }

        private string GetFirstMatchFromRegexPattern(string regex, string text)
        {
            Regex r = new(regex, RegexOptions.IgnoreCase);
            Match m = r.Match(text);
            if (!m.Success)
            {
                throw new YoutubeWrongVideoUrlException(_websiteName);
            }
            else
            {
                return m.Groups[1].Value;
            }
        }
    }
}
