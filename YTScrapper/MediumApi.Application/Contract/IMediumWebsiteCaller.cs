using MediumApi.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MediumApi.Application.Contract
{
    public interface IMediumWebsiteCaller
    {
        Task<List<Post>> GetPostsByAuthorUsername(string username, CancellationToken cts = default);
    }
}
