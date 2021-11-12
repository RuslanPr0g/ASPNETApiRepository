using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YTScrapper.Application.Contracts;
using YTScrapper.Application.DTOs;
using YTScrapper.Application.Filters;
using YTScrapper.Application.Runners;

namespace YTScrapper.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IScraperRunner _runner;
        private readonly ISearchService _service;

        public SearchController(IScraperRunner runner, ISearchService service)
        {
            _runner = runner;
            _service = service;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchRequest searchRequest, CancellationToken token)
        {
            var searches = await _service.GetAllSearchItems();

            if (searches.Any(s => s.SearchItemUrl == searchRequest.SearchUrl))
            {
                return Ok(searches.First(s => s.SearchItemUrl == searchRequest.SearchUrl));
            }

            var result = (await _runner.Run(searchRequest, token)).First();

            if (result.HasValue)
            {
                return Ok(result.Value);
            }
            else
            {
                return BadRequest(result.NullMessage);
            }
        }

        [HttpPost("search/specific")]
        public async Task<IActionResult> SearchByFilter([FromBody] SearchSpecificQuery searchRequest)
        {
            var searchItemFilter = new SearchItemFilter
            {
                Id = searchRequest.Id,
                Author = searchRequest.Author,
            };

            var result = await _service.GetSearchItemsByParameter(searchItemFilter);

            if (result.Count > 0)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("No searches found by your request.");
            }
        }
    }
}
