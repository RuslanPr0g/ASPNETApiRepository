using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YTScrapper.Domain.Models;
using YTScrapper.Shared.Models;

namespace YTScrapper.Application.Contracts
{
    public interface ISearchRunner
    {
        Task<SuccessOrFailure<YouTubeSearchItem>> Run(string url, CancellationToken token = default);
    }
}
