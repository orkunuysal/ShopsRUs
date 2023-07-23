using Discount.Application.Services;
using Discount.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Discount.API.Controllers
{
    [Route("api/v1/[controller]")]
    public class SegmentController : Controller
    {
        private readonly ISegmentService _segmentService;

        public SegmentController(ISegmentService segmentService)
        {
            _segmentService = segmentService;
        }

        [Route("[action]", Name = "AddSegment")]
        [HttpPost]
        [ProducesResponseType(typeof(SegmentDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> AddSegment(SegmentDTO segment)
        {
            await _segmentService.AddSegment(segment);
            return Ok();
        }
    }
}
