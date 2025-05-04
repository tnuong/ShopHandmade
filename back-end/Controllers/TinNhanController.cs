using back_end.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TinNhanController : ControllerBase
    {
        private readonly ITinNhanService messageService;

        public TinNhanController(ITinNhanService messageService)
        {
            this.messageService = messageService;
        }

        [HttpGet("{maNguoiNhan}")]
        public async Task<IActionResult> GetAllMessages([FromRoute] string maNguoiNhan)
        {
            var response = await messageService.GetAllMessages(maNguoiNhan);
            return Ok(response);
        }
    }
}
