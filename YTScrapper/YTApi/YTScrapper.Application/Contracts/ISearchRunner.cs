using System.Threading;
using System.Threading.Tasks;
using YTSearch.Application.DTOs;
using YTSearch.Domain.Models;
using YTSearch.Shared.Models;

namespace YTSearch.Application.Contracts
{
    public interface ISearchRunner
    {
        Task<SuccessOrFailure<YouTubeModel>> Run(SearchRunnerRequest searchRunnerRequest, CancellationToken token = default);
    }
}
