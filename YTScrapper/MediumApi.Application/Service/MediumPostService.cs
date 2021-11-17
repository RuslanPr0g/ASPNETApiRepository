using MediumApi.Application.Contract;
using MediumApi.Domain.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YTSearch.Shared.Helper;
using YTSearch.Shared.Models;

namespace MediumApi.Application.Service
{
    public class MediumPostService : IMediumPostService
    {
        private readonly IMediumWebsiteRepository _mediumWebsiteRepository;

        public MediumPostService(IMediumWebsiteRepository mediumWebsiteRepository)
        {
            _mediumWebsiteRepository = mediumWebsiteRepository;
        }

        public async Task<SuccessOrFailure<Post>> GetPost(string url, CancellationToken cancellationToken)
        {
            var username = RegexHelper.GetNMatchFromRegexPattern(@"^https:\/\/medium.com\/([^\/]+)", url, 1);

            var posts = await _mediumWebsiteRepository.GetPostsByAuthorUsername(username, cancellationToken);

            var post = posts.FirstOrDefault(p => p.Link.Contains(url));

            if (posts?.Count > 0)
            {
                return post;
            }
            else
            {
                return SuccessOrFailure<Post>.CreateNull("Wrong URL provided.");
            }
        }
    }
}
