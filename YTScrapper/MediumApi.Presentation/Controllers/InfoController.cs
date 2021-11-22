using MediumApi.Application.Contract;
using MediumApi.Application.Request;
using Microsoft.AspNetCore.Mvc;

namespace MediumApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly IMediumPostService _mediumService;

        public InfoController(IMediumPostService mediumService)
        {
            _mediumService = mediumService;
        }

        [HttpPost("post")]
        public async Task<IActionResult> Get([FromBody] PostInfoRequest postInfoRequest, CancellationToken cancellationToken)
        {
            var postValue = await _mediumService.GetPost(postInfoRequest.Url, cancellationToken);

            if (postValue.HasValue)
                return Ok(postValue.Value);
            else
                return BadRequest(postValue.NullMessage);
        }
    }
}
