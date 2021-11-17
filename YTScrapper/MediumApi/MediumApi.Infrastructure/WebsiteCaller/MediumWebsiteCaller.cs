using MediumApi.Application.Contract;
using MediumApi.Application.Response;
using MediumApi.Domain.Global;
using MediumApi.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MediumApi.Infrastructure.WebsiteGetter
{
    public class MediumWebsiteCaller : IMediumWebsiteCaller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MediumWebsiteCaller(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<Post>> GetPostsByAuthorUsername(string username, CancellationToken cts = default)
        {
            var result = await GetRssJsonResponseAsync(username, cts);
            return result.Items.Select(p => new Post
            {
                Id = p.Id,
                Link = p.Link,
                Title = p.Title,
                Author = p.Author,
                Description = p.Description,
                Content = p.Content,
                Thumbnail = p.Thumbnail,
                PubDate = p.PubDate,
                Categories = p.Categories.Select(c => new Category
                {
                    Id = -1,
                    PostId = p.Id,
                    Content = c
                }).ToList()
            }).ToList();
        }

        private async Task<RssJsonResponse> GetRssJsonResponseAsync(string username, CancellationToken cts = default)
        {
            var httpClient = _httpClientFactory.CreateClient(MediumConstants.Name);
            using var httpResponse = await httpClient.GetAsync($"?rss_url=https://medium.com/feed/{username}", cts);

            if (!httpResponse.IsSuccessStatusCode)
                return new();

            return await httpResponse.Content.ReadFromJsonAsync<RssJsonResponse>(cancellationToken: cts);
        }
    }
}
