using back_end.Core.Models;
using back_end.Core.Requests;
using back_end.Core.Responses;
using back_end.Core.Responses.Resources;
using back_end.Data;
using back_end.Exceptions;
using back_end.Extensions;
using back_end.Infrastructures.Cloudinary;
using back_end.Mappers;
using back_end.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace back_end.Services.Implements
{
    public class BaiVietService : IBaiVietService
    {
        private readonly MyStoreDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationMapper applicationMapper;
        private readonly IUploadService uploadService;

        public BaiVietService(MyStoreDbContext dbContext, IHttpContextAccessor httpContextAccessor, ApplicationMapper applicationMapper, IUploadService uploadService)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
            this.applicationMapper = applicationMapper;
            this.uploadService = uploadService;
        }

        public async Task<BaseResponse> CreateBlog(CreateBlogRequest request)
        {
            var userId = httpContextAccessor.HttpContext.User.GetUserId();
            BaiViet blog = new BaiViet();
            blog.TieuDe = request.Title;
            blog.MaTacGia = userId;
            blog.VanBanTho = request.TextPlain;
            blog.NgayTao = DateTime.Now;
            blog.NoiDung = request.Content;
            var thumbnail = await uploadService.UploadSingleFileAsync(request.Thumbnail);
            blog.HinhDaiDien = thumbnail;

            await dbContext.BaiViets.AddAsync(blog);
            await dbContext.SaveChangesAsync();

            var response = new BaseResponse();
            response.Message = "Tạo một bài viết mới thành công";
            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;

            return response;
        }

        public async Task DeleteBlog(int blogId)
        {
            BaiViet blog = await dbContext.BaiViets
                .Where(b => !b.TrangThaiXoa)
                .SingleOrDefaultAsync(b => b.MaBaiViet == blogId)
                    ?? throw new NotFoundException("Không tìm thấy bài viết nào");
            blog.TrangThaiXoa = true;
            int rows = await dbContext.SaveChangesAsync();
            if (rows == 0) throw new Exception("Không thể xóa bài viết");
        }

        public async Task<BaseResponse> GetAllBlogs()
        {
            var blogs = await dbContext.BaiViets
                .Where(b => !b.TrangThaiXoa)
                .Include(b => b.TacGia)
                .ToListAsync();

            var resources = blogs.Select(blog => applicationMapper.MapToBlogResource(blog)).ToList();

            var response = new DataResponse<List<BaiVietResource>>()
            {
                Data = resources,
                Message = "Lấy tất cả bài viết thành công",
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK
            };

            return response;
        }

        public async Task<BaseResponse> GetAllBlogsExceptCurrentBlog(int blogId)
        {
            var blogs = await dbContext.BaiViets
                .Where(b => !b.TrangThaiXoa)
                .Include(b => b.TacGia)
                .Where(b => b.MaBaiViet != blogId)
                .ToListAsync();

            var resources = blogs.Select(blog => applicationMapper.MapToBlogResource(blog)).ToList();

            var response = new DataResponse<List<BaiVietResource>>()
            {
                Data = resources,
                Message = "Lấy tất cả bài viết thành công",
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK
            };

            return response;
        }

        public async Task<BaseResponse> GetAllBlogsRelatedUser(string userId, int blogId)
        {
            var blogs = await dbContext.BaiViets
                .Where(b => !b.TrangThaiXoa)
                .Include(b => b.TacGia)
                .Where(b => b.MaTacGia == userId && b.MaBaiViet != blogId)
                .ToListAsync();

            var resources = blogs.Select(blog => applicationMapper.MapToBlogResource(blog)).ToList();

            var response = new DataResponse<List<BaiVietResource>>()
            {
                Data = resources,
                Message = "Lấy tất cả bài viết thành công",
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK
            };

            return response;
        }

        public async Task<BaseResponse> GetBlogById(int id)
        {
            BaiViet blog = await dbContext.BaiViets.Where(b => !b.TrangThaiXoa)
                .Include(blog => blog.TacGia)
                .SingleOrDefaultAsync(b => b.MaBaiViet == id)
                    ?? throw new NotFoundException("Không tìm thấy bài viết nào");


            var resources = applicationMapper.MapToBlogResource(blog);

            var response = new DataResponse<BaiVietResource>()
            {
                Data = resources,
                Message = "Lấy bài viết thành công",
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK
            };

            return response;
        }

        public async Task HiddenBlog(int blogId)
        {
            BaiViet blog = await dbContext.BaiViets
                .Where(b => !b.TrangThaiXoa)
                .SingleOrDefaultAsync(b => b.MaBaiViet == blogId)
                    ?? throw new NotFoundException("Không tìm thấy bài viết nào");
            blog.TrangThaiAn = true;
            int rows = await dbContext.SaveChangesAsync();
            if (rows == 0) throw new Exception("Không thể ẩn bài viết");
        }

        public async Task ShowBlog(int blogId)
        {
            BaiViet blog = await dbContext.BaiViets
                .Where(b => !b.TrangThaiXoa)
                .SingleOrDefaultAsync(b => b.MaBaiViet == blogId)
                    ?? throw new NotFoundException("Không tìm thấy bài viết nào");
            blog.TrangThaiAn = false;
            int rows = await dbContext.SaveChangesAsync();
            if (rows == 0) throw new Exception("Không thể bỏ ẩn bài viết");
        }

        public async Task UpdateBlog(int id, EditBlogRequest request)
        {
            BaiViet blog = await dbContext.BaiViets.Where(b => !b.TrangThaiXoa)
                .Include(blog => blog.TacGia)
                .SingleOrDefaultAsync(b => b.MaBaiViet == id)
                    ?? throw new NotFoundException("Không tìm thấy bài viết nào");

            blog.TieuDe = request.Title;
            blog.VanBanTho = request.TextPlain;
            blog.NoiDung = request.Content;

            if(request.Thumbnail != null)
            {
                var thumbnail = await uploadService.UploadSingleFileAsync(request.Thumbnail);
                blog.HinhDaiDien = thumbnail;
            }

            int rowChanges = await dbContext.SaveChangesAsync();

            if (rowChanges == 0) throw new Exception("Cập nhật bài viết thất bại");

        }
    }
}
