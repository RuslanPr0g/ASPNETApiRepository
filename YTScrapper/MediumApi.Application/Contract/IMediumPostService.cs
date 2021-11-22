using MediumApi.Application.Model;
using MediumApi.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace MediumApi.Application.Contract
{
    public interface IMediumPostService
    {
        Task<SuccessOrFailure<Post>> GetPost(string url, CancellationToken cancellationToken);
    }
}
