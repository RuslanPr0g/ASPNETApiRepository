using MediumApi.Application.Contract;
using MediumApi.Application.Model;
using MediumApi.Domain.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediumApi.Application.Service
{
    public class MediumPostService : IMediumPostService
    {
        private readonly IMediumWebsiteCaller _mediumWebsiteCaller;
        private readonly IMediumWebsiteRepository _mediumWebsiteRepository;

        public MediumPostService(IMediumWebsiteCaller mediumWebsiteCaller, IMediumWebsiteRepository mediumWebsiteRepository)
        {
            _mediumWebsiteCaller = mediumWebsiteCaller;
            _mediumWebsiteRepository = mediumWebsiteRepository;
        }

        public async Task<SuccessOrFailure<Post>> GetPost(string url, CancellationToken cancellationToken)
        {
            //var repoPosts = await _mediumWebsiteRepository.GetPosts();

            //if (repoPosts.Any(s => s.Link == url))
            //{
            //    return repoPosts.First(s => s.Link == url);
            //}

            var username = url.GetMediumUsernameFromUrl();

            var calledPosts = await _mediumWebsiteCaller.GetPostsByAuthorUsername(username, cancellationToken);

            var post = calledPosts.FirstOrDefault(p => p.Link.Contains(url));

            if (calledPosts?.Count > 0 && post is not null)
            {
                post.Link = url;
                //await _mediumWebsiteRepository.AddPost(post);

                return post;
            }
            else
            {
                return SuccessOrFailure<Post>.CreateNull("Wrong URL provided.");
            }
        }
    }
}
