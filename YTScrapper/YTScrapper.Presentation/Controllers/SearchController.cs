using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using YTScrapper.Application.DTOs;
using YTScrapper.Application.Runners;

namespace YTScrapper.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IScraperRunner _runner;

        public SearchController(IScraperRunner runner)
        {
            _runner = runner;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchRequest searchRequest, CancellationToken token)
        {
            var result = await _runner.Run(searchRequest, token);
            return Ok(result);
        }
    }
}
