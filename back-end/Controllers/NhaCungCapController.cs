using back_end.Core.Requests;
using back_end.Services.Implements;
using back_end.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhaCungCapController : ControllerBase
    {
        private readonly INhaCungCapService supplierService;

        public NhaCungCapController(INhaCungCapService supplierService)
        {
            this.supplierService = supplierService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierRequest request)
        {
            var response = await supplierService.CreateSupplier(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 8, [FromQuery] string searchString = "")
        {
            var response = await supplierService.GetAllSuppliers(pageIndex, pageSize, searchString);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierById([FromRoute] int id)
        {
            var response = await supplierService.GetSupplierById(id);
            return Ok(response);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateManufacturer([FromRoute] int id, [FromBody] EditSupplierRequest request)
        {
            var response = await supplierService.UpdateSupplier(id, request);
            return Ok(response);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteManufacturer([FromRoute] int id)
        {
            var response = await supplierService.DeleteSupplier(id);
            return Ok(response);
        }
    }
}
