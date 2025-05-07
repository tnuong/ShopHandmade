using back_end.Core.Requests;
using back_end.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class KhuyenMaiController : ControllerBase
    {
        private readonly IKhuyenMaiService khuyenMaiService;

        public KhuyenMaiController(IKhuyenMaiService khuyenMaiService)
        {
            this.khuyenMaiService = khuyenMaiService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePromotion([FromBody] PromotionRequest request)
        {
            var response = await khuyenMaiService.CreatePromotion(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetPromotion([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 8, [FromQuery] string searchString = "")
        {
            var response = await khuyenMaiService.GetAllPromotions(pageIndex, pageSize, searchString);
            return Ok(response);
        }

        [HttpPost("ap-dung")]
        public async Task<IActionResult> AssignPromotion([FromBody] AssignPromotionRequest request)
        {
            var response = await khuyenMaiService.AssignPromotion(request);
            return Ok(response);
        }

        [HttpGet("khong-phan-trang")]
        public async Task<IActionResult> GetPromotion()
        {
            var response = await khuyenMaiService.GetAllPromotions();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromotion([FromRoute] int id)
        {
            var response = await khuyenMaiService.GetPromotionById(id);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePromotion([FromRoute] int id, [FromBody] PromotionRequest request)
        {
            var response = await khuyenMaiService.UpdatePromotion(id, request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePromotion([FromRoute] int id)
        {
            var response = await khuyenMaiService.DeletePromotion(id);
            return Ok(response);
        }

        [HttpDelete("{productId}/{promotionId}")]
        public async Task<IActionResult> DeleteProductPromotion([FromRoute] int productId, [FromRoute] int promotionId)
        {
            var response = await khuyenMaiService.DeleteProductPromotion(productId, promotionId);
            return Ok(response);
        }
    }
}
