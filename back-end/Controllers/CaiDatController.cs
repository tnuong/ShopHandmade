using back_end.Core.Requests;
using back_end.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class CaiDatController : ControllerBase
    {
        private readonly IHienThiBannerService _slideShowService;
        private readonly IHienThiDanhGiaService _reviewShowService;

        public CaiDatController(IHienThiBannerService slideShowService, IHienThiDanhGiaService reviewShowService)
        {
            _slideShowService = slideShowService;
            _reviewShowService = reviewShowService;
        }

        [AllowAnonymous]
        [HttpGet("HienThiBanner")]
        public async Task<IActionResult> GetAllSlideShows()
        {
            var response = await _slideShowService.GetAllSlideShows();
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("HienThiDanhGia")]
        public async Task<IActionResult> GetAllReviewShows()
        {
            var response = await _reviewShowService.GetAllReviewShows();
            return Ok(response);
        }

        [HttpPost("HienThiBanner")]
        public async Task<IActionResult> CreateSlideShow([FromForm] CreateSlideShowRequest request)
        {
            var response = await _slideShowService.CreateSlideShow(request);
            return Ok(response);
        }

        [HttpPut("HienThiBanner/{id}")]
        public async Task<IActionResult> EditSlideShow([FromRoute] int id, [FromForm] EditSlideShowRequest request)
        {
            await _slideShowService.EditSlideShow(id, request);
            return NoContent();
        }

        [HttpDelete("HienThiBanner/{id}")]
        public async Task<IActionResult> RemoveSlideShow([FromRoute] int id)
        {
            await _slideShowService.RemoveSlideShow(id);
            return NoContent();
        }

        [HttpPost("HienThiDanhGia")]
        public async Task<IActionResult> CreateReviewShow([FromBody] ReviewShowRequest request)
        {
            var response = await _reviewShowService.CreateReviewShow(request);
            return Ok(response);
        }

        [HttpDelete("HienThiDanhGia/{id}")]
        public async Task<IActionResult> EditReviewShow([FromRoute] int id)
        {
            await _reviewShowService.RemoveReviewShow(id);
            return NoContent();
        }
    }
}
