using MediumApi.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MediumApi.Application.Contract
{
    public interface IMediumWebsiteRepository
    {
        Task<List<Post>> GetPostsByAuthorUsername(string username, CancellationToken cts = default);
    }
}
