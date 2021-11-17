using MediumApi.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediumApi.Application.Contract
{
    public interface IMediumWebsiteRepository
    {
        Task<List<Post>> Get();
        Task<int> Add(Post post);
        Task Update(Post post);
        Task Delete(Post post);
    }
}
