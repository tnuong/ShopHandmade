using back_end.Core.Requests;
using back_end.Core.Responses;
using back_end.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class BaiVietController : ControllerBase
    {
        private readonly IBaiVietService blogService;

        public BaiVietController(IBaiVietService blogService)
        {
            this.blogService = blogService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<BaseResponse> GetAll()
        {
            var response = await blogService.GetAllBlogs();
            return response;
        }

        [HttpGet("lien-quan/{maNguoiDung}/{maBaiViet}")]
        [ProducesResponseType(200)]
        public async Task<BaseResponse> GetAllBlogsRelatedUser([FromRoute] string maNguoiDung, [FromRoute] int maBaiViet)
        {
            var response = await blogService.GetAllBlogsRelatedUser(maNguoiDung, maBaiViet);
            return response;
        }

        [HttpGet("ngoai-tru/{maBaiViet}")]
        [ProducesResponseType(200)]
        public async Task<BaseResponse> GetAllBlogsExceptBlogId([FromRoute] int maBaiViet)
        {
            var response = await blogService.GetAllBlogsExceptCurrentBlog(maBaiViet);
            return response;
        }

        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<BaseResponse> CreateBlog([FromForm] CreateBlogRequest request)
        {
            var response = await blogService.CreateBlog(request);
            return response;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        public async Task<BaseResponse> GetBlogById([FromRoute] int id)
        {
            var response = await blogService.GetBlogById(id);
            return response;
        }

        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public async Task UpdateBlog([FromRoute] int id, [FromForm] EditBlogRequest request)
        {
            await blogService.UpdateBlog(id, request);
        }

        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpPut("bai-viet-an/{id}")]
        [ProducesResponseType(204)]
        public async Task HiddenBlog([FromRoute] int id)
        {
            await blogService.HiddenBlog(id);
        }

        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpPut("hien-thi/{id}")]
        [ProducesResponseType(204)]
        public async Task ShowBlog([FromRoute] int id)
        {
            await blogService.ShowBlog(id);
        }

        [Authorize(Roles = "ADMIN, EMPLOYEE")]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task DeleteBlog([FromRoute] int id)
        {
            await blogService.DeleteBlog(id);
        }
    }
}
