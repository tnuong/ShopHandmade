using back_end.Core.Requests;
using back_end.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class DanhGiaController : ControllerBase
    {
        private readonly IDanhGiaService evaluationService;

        public DanhGiaController(IDanhGiaService evaluationService)
        {
            this.evaluationService = evaluationService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("xuat-sac")]
        public async Task<IActionResult> GetAllExcellentEvaluations()
        {
            var response = await evaluationService.GetAllExcellentEvaluation();
            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateEvaluation([FromBody] EvaluationRequest request)
        {
            var response = await evaluationService.CreateEvaluation(request);
            return Ok(response);
        }

        [HttpGet("{maSanPham}")]
        public async Task<IActionResult> GetAllByProductId([FromRoute] int maSanPham, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var response = await evaluationService.GetAllByProductId(maSanPham, pageIndex, pageSize);
            return Ok(response);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> InteractiveEvaluation([FromRoute] int id)
        {
            await evaluationService.InteractEvaluation(id);
            return NoContent();
        }
    }
}
