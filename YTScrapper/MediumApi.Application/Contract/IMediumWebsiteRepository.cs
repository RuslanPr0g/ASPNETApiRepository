using MediumApi.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediumApi.Application.Contract
{
    public interface IMediumWebsiteRepository
    {
        Task<List<Post>> GetPosts();
        Task<int> AddPost(Post post);
        Task UpdatePost(Post post);
        Task DeletePost(Post post);
    }
}
