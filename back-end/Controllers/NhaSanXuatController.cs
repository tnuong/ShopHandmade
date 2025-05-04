using back_end.Core.Requests;
using back_end.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhaSanXuatController : ControllerBase
    {
        private readonly INhaSanXuatService manufacturerService;

        public NhaSanXuatController(INhaSanXuatService manufacturerService)
        {
            this.manufacturerService = manufacturerService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> CreateManufacturer([FromBody] CreateManufacturerRequest request)
        {
            var response = await manufacturerService.CreateManufacturer(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllManufacturers([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 8, [FromQuery] string searchString = "")
        {
            var response = await manufacturerService.GetAllManufacturers(pageIndex, pageSize, searchString);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetManufacturerById([FromRoute] int id)
        {
            var response = await manufacturerService.GetManufacturerById(id);
            return Ok(response);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateManufacturer([FromRoute] int id, [FromBody] EditManufacturerRequest request)
        {
            var response = await manufacturerService.UpdateManufacturer(id, request);
            return Ok(response);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteManufacturer([FromRoute] int id)
        {
            var response = await manufacturerService.DeleteManufacturer(id);
            return Ok(response);
        }
    }
}
