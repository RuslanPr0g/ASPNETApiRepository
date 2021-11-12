using System.Threading;
using System.Threading.Tasks;
using YTScrapper.Application.DTOs;
using YTScrapper.Domain.Models;
using YTScrapper.Shared.Models;

namespace YTScrapper.Application.Contracts
{
    public interface ISearchRunner
    {
        Task<SuccessOrFailure<YouTubeModel>> Run(SearchRunnerRequest searchRunnerRequest, CancellationToken token = default);
    }
}
