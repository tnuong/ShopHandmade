using back_end.Core.Models;
using back_end.Core.Requests;
using back_end.Core.Responses;
using back_end.Core.Responses.Resources;
using back_end.Data;
using back_end.Infrastructures.Cloudinary;
using back_end.Mappers;
using back_end.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace back_end.Services.Implements
{
    public class HienThiBannerService : IHienThiBannerService
    {
        private readonly MyStoreDbContext dbContext;
        private readonly IUploadService uploadService;
        private readonly ApplicationMapper applicationMapper;

        public HienThiBannerService(MyStoreDbContext dbContext, IUploadService uploadService, ApplicationMapper applicationMapper)
        {
            this.dbContext = dbContext;
            this.uploadService = uploadService;
            this.applicationMapper = applicationMapper;
        }

        public async Task<BaseResponse> CreateSlideShow(CreateSlideShowRequest request)
        {
            HienThiBanner slideShow = new HienThiBanner();
            slideShow.NutHanhDong = request.BtnTitle;
            slideShow.TieuDe = request.Title;
            slideShow.MoTaNgan = request.Description;

            var backgroundImage = await uploadService.UploadSingleFileAsync(request.BackgroundImage);
            slideShow.DuongDanhAnhNen = backgroundImage;
            await dbContext.AddAsync(slideShow);
            await dbContext.SaveChangesAsync();

            var response = new BaseResponse()
            {
                StatusCode = System.Net.HttpStatusCode.Created,
                Message = "Thêm slide mới thành công",
                Success = true
            };

            return response;
        }

        public async Task EditSlideShow(int id, EditSlideShowRequest request)
        {
            HienThiBanner? checkSlideShow = await dbContext.HienThiBanners
                .SingleOrDefaultAsync(s => s.MaHienThiBanner == id)
                    ?? throw new DirectoryNotFoundException("Không tìm thấy slideshow nào");

            checkSlideShow.NutHanhDong = request.BtnTitle;
            checkSlideShow.TieuDe = request.Title;
            checkSlideShow.MoTaNgan = request.Description;

            if(request.BackgroundImage != null)
            {
                var backgroundImage = await uploadService.UploadSingleFileAsync(request.BackgroundImage);
                checkSlideShow.DuongDanhAnhNen = backgroundImage;
            }

            int rows = await dbContext.SaveChangesAsync();
            if (rows == 0) throw new Exception("Cập nhật slide thất bại");
        }

        public async Task<BaseResponse> GetAllSlideShows()
        {
            var slideShows = await dbContext.HienThiBanners.ToListAsync();

            
            return new DataResponse<List<HienThiBannerResource>>()
            {
                Data = slideShows.Select(s => applicationMapper.MapToBannerResource(s)).ToList(),
                Message = "Lấy tất cả slideshow thành công",
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task RemoveSlideShow(int id)
        {
            HienThiBanner? checkSlideShow = await dbContext.HienThiBanners
                .SingleOrDefaultAsync(s => s.MaHienThiBanner == id)
                    ?? throw new DirectoryNotFoundException("Không tìm thấy slideshow nào");

            dbContext.HienThiBanners.Remove(checkSlideShow);
            int rows = await dbContext.SaveChangesAsync();
            if (rows == 0) throw new Exception("Xóa slide thất bại");

        }
    }
}
