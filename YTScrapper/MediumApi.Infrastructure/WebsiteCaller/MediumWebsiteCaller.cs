using MediumApi.Application.Contract;
using MediumApi.Domain.Global;
using MediumApi.Domain.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
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
            var httpClient = _httpClientFactory.CreateClient(MediumConstants.Name);
            using var httpResponse = await httpClient.GetAsync(username, cts);

            if (!httpResponse.IsSuccessStatusCode)
                return new();

            using var sr = new StreamReader(await httpResponse.Content.ReadAsStreamAsync(cts));

            // 4) Convert RSS data to JSON one

            using var jtr = new JsonTextReader(sr);
            return new JsonSerializer().Deserialize<List<Post>>(jtr) ?? new();
        }
    }
}
