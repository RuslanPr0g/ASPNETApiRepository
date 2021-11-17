using MediumApi.Application.Contract;
using MediumApi.Domain.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using YTSearch.Shared.Models;

namespace MediumApi.Application.Service
{
    public class PostService : IPostService
    {
        public Task<SuccessOrFailure<Post>> GetPost(string url, CancellationToken cancellationToken)
        {
            // 1) Get url from the request, e.g. https://medium.com/young-coder/7-of-my-favorite-little-javascript-tricks-4f2a1cfe68b4
            // 2) Get username from the link
            // 3) Get data from medium site where username is located, e.g. https://medium.com/feed/young-coder
            // 4) Convert RSS data to JSON one
            // 5) Find record with info about the post, search criteria is "link" field with url to the post, e.g. https://medium.com/young-coder/7-of-my-favorite-little-javascript-tricks-4f2a1cfe68b4
            // 6) Return Post

            throw new NotImplementedException();
        }

        private async Task<SuccessOrFailure<Post>> GetMyObjectAsync(string url, CancellationToken cts = default)
        {
            using var httpResponse = await _httpClient.GetAsync(url, cts);

            if (!httpResponse.IsSuccessStatusCode)
                return SuccessOrFailure<Post>.CreateNull("Wrong url provided.");

            using var sr = new StreamReader(await httpResponse.Content.ReadAsStreamAsync(cts));

            using var jtr = new JsonTextReader(sr);
            return new JsonSerializer().Deserialize<Post>(jtr)
                ?? SuccessOrFailure<Post>.CreateNull("Wrong url provided.");
        }
    }
}
