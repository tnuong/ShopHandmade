using back_end.Core.Requests;
using back_end.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly ISanPhamService productService;

        public SanPhamController(ISanPhamService productService) {
            this.productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequest request)
        {
            var response = await productService.CreateProduct(request);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] EditProductRequest request)
        {
            await productService.UpdateProduct(id, request);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("top-yeu-thich")]
        public async Task<IActionResult> GetTopFavoriteProducts()
        {
            var response = await productService.GetMostFavoriteProducts();
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("top-ban-chay")]
        public async Task<IActionResult> GetTopBestSellerProducts()
        {
            var response = await productService.GetBestSellerProducts();
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("tim-kiem")]
        public async Task<IActionResult> SeachProducts([FromQuery] string searchString = "", [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 6)
        {
            var response = await productService.SearchProduct(searchString, pageIndex, pageSize);
            return Ok(response);
        }

        [HttpPut("upload/hinh-dai-dien/{id}")]
        public async Task<IActionResult> UpdateThumbnail([FromRoute] int id, [FromForm] UploadSingleFileRequest request)
        {
            await productService.UploadThumbnail(id, request.File);
            return NoContent();
        }

        [HttpPut("upload/hinh-phong-to/{id}")]
        public async Task<IActionResult> UploadZoomImage([FromRoute] int id, [FromForm] UploadSingleFileRequest request)
        {
            await productService.UploadZoomImage(id, request.File);
            return NoContent();
        }

        [HttpPut("upload/hinh-anh/{id}")]
        public async Task<IActionResult> UploadImages([FromRoute] int id, [FromForm] UploadFileRequest request)
        {
            await productService.UploadImages(id, request.Files);
            return NoContent();
        }

        [HttpPut("xoa-hinh-anh")]
        public async Task<IActionResult> RemoveImages([FromBody] RemoveImageRequest request)
        {
            await productService.RemoveImages(request.ImageIds);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] double giaThapNhat = 0,
            [FromQuery] double giaCaoNhat = 0,
            [FromQuery] string sapXepTheo = null,
            [FromQuery] string thuTuSapXep = null,
            [FromQuery] List<int> thuongHieuIds = null,
            [FromQuery] List<int> danhMucIds = null,
            [FromQuery] List<int> mauSacIds = null,
            [FromQuery] List<int> kichThuocIds = null
        )
        {
           
            var response = await productService.GetAllProducts(pageIndex, pageSize, giaThapNhat, giaCaoNhat, thuongHieuIds, danhMucIds, mauSacIds, kichThuocIds, sapXepTheo, thuTuSapXep);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] int id)
        {
            var response = await productService.GetProductById(id);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveProduct([FromRoute] int id)
        {
            await productService.RemoveProduct(id);
            return NoContent();
        }
    }
}
