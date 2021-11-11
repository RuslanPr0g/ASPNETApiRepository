using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using YTScrapper.Application.Contracts;
using YTScrapper.Application.DTOs;
using YTScrapper.Application.Exceptions;
using YTScrapper.Domain.Models;
using YTScrapper.Infrastructure.Services;

namespace YTScrapper.Infrastructure.Scrapers
{
    public class YoutubeScraper : ScraperBase
    {
        private const string _websiteName = "YouTube";
        private readonly IWebClientService _clientProvider;

        public YoutubeScraper(ILogger<YoutubeScraper> logger, IWebClientService clientProvider) : base(logger)
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
            if (searchUrl.Contains("www.youtube.com/watch?v=") is false)
            {
                throw new YoutubeWrongVideoUrlException(searchUrl);
            }

            using var client = await _clientProvider.Provide();

            await _clientProvider.SetDefaultUserString(client);
            var htmlSearchPage = client.DownloadString(searchUrl);

            var title = string.Empty;
            var description = string.Empty;
            var author = string.Empty;
            var duration = string.Empty;

            // title: "title":\s*"([a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':\\|,.<>\/?\s\n]+)",
            string titleRegex = @"""title"":\s*""([a - zA - Z0 - 9!@#$%^&*()_+\-=\[\]{};':\\|,.<>\/?\s\n]+)"",";
            title = GetFirstMatchFromRegexPattern(titleRegex, htmlSearchPage);

            // description: "shortDescription":\s*"([a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':\\|,.<>\/?\s\n]+)",
            string descriptionRegex = @"""shortDescription"":\s*""([a - zA - Z0 - 9!@#$%^&*()_+\-=\[\]{};':\\|,.<>\/?\s\n]+)"",";
            description = GetFirstMatchFromRegexPattern(descriptionRegex, htmlSearchPage);

            // author: "channelName":\s*"([a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':\\|,.<>\/?\s\n]+)",
            string authorRegex = @"""channelName"":\s*""([a - zA - Z0 - 9!@#$%^&*()_+\-=\[\]{};':\\|,.<>\/?\s\n]+)"",";
            author = GetFirstMatchFromRegexPattern(authorRegex, htmlSearchPage);

            // duration: "approxDurationMs":\s*"([a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':\\|,.<>\/?\s\n]+)",
            string durationRegex = @"""approxDurationMs"":\s*""([a - zA - Z0 - 9!@#$%^&*()_+\-=\[\]{};':\\|,.<>\/?\s\n]+)"",";
            duration = GetFirstMatchFromRegexPattern(durationRegex, htmlSearchPage);

            return new()
            {
                SearchItemUrl = searchUrl,
                ImagePreviewUrl = string.Empty,
                Title = title,
                Description = description,
                Author = author,
                Duration = duration
            };
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
                return m.Groups[0].Value;
            }
        }
    }
}
