using MediumApi.Domain.Models;
using System.Threading;
using System.Threading.Tasks;
using YTSearch.Shared.Models;

namespace MediumApi.Application.Contract
{
    public interface IMediumPostService
    {
        Task<SuccessOrFailure<Post>> GetPost(string url, CancellationToken cancellationToken);
    }
}
