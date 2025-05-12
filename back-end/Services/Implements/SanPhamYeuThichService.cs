using back_end.Core.Models;
using back_end.Core.Responses;
using back_end.Core.Responses.Resources;
using back_end.Data;
using back_end.Exceptions;
using back_end.Extensions;
using back_end.Mappers;
using back_end.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace back_end.Services.Implements
{
    public class SanPhamYeuThichService : ISanPhamYeuThichService
    {
        private readonly MyStoreDbContext myStoreDbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationMapper applicationMapper;

        public SanPhamYeuThichService(MyStoreDbContext myStoreDbContext, IHttpContextAccessor httpContextAccessor, ApplicationMapper applicationMapper)
        {
            this.myStoreDbContext = myStoreDbContext;
            this.httpContextAccessor = httpContextAccessor;
            this.applicationMapper = applicationMapper;
        }

        public async Task<BaseResponse> AddSanPhamYeuThich(int maSanPham)
        {
            var sanPham = await myStoreDbContext.SanPhams
                .SingleOrDefaultAsync(s => s.MaSanPham == maSanPham)
                    ?? throw new NotFoundException("Không tìm thấy sản phẩm");

            var userId = httpContextAccessor.HttpContext.User.GetUserId();
            var dsYeuThich = await myStoreDbContext.DanhSachYeuThichs
                .Include(s => s.DanhSachSanPham)
                .SingleOrDefaultAsync(s => s.MaNguoiDung == userId);

            if (dsYeuThich is null)
            {
                dsYeuThich = new Core.Models.DanhSachYeuThich
                {
                    MaNguoiDung = userId
                };
                dsYeuThich.DanhSachSanPham ??= new List<SanPham>();
                dsYeuThich.DanhSachSanPham.Add(sanPham);

                await myStoreDbContext.DanhSachYeuThichs.AddAsync(dsYeuThich);
                await myStoreDbContext.SaveChangesAsync();
            }
            else
            {
                var isExist = dsYeuThich.DanhSachSanPham.Any(s => s.MaSanPham == sanPham.MaSanPham);
                if (!isExist)
                {
                    dsYeuThich.DanhSachSanPham ??= new List<SanPham>();
                    dsYeuThich.DanhSachSanPham.Add(sanPham);
                    await myStoreDbContext.SaveChangesAsync();
                }
                else throw new Exception("Sản phẩm đã có trong danh sách yêu thích");
            }

            return new BaseResponse()
            {
                Message = "Thêm sản phẩm vào danh sách yêu thích thành công",
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
            };
        }

        public async Task<BaseResponse> GetAllSanPham(int pageIndex, int pageSize)
        {
            var userId = httpContextAccessor.HttpContext.User.GetUserId();

            var dsYeuThich = await myStoreDbContext.DanhSachYeuThichs
                .Include(s => s.DanhSachSanPham)
                .SingleOrDefaultAsync(s => s.MaNguoiDung == userId);
                

            var totalItems = dsYeuThich?.DanhSachSanPham?.Count ?? 0;
            var result = new List<SanPhamResource>();

            if(dsYeuThich is not null)
            {
              
                foreach(var p in dsYeuThich.DanhSachSanPham)
                {
                    var product = applicationMapper.MapToProductResource(p);
                    product.HasWishlist = true;
                    result.Add(product);
                }
            }

            return new PaginationResponse<List<SanPhamResource>>()
            {
                Data = result,
                Message = "Lấy danh sách sản phẩm thành công",
                StatusCode = System.Net.HttpStatusCode.OK,
                Pagination = new Pagination()
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling((double)totalItems / pageSize)
                }
            };
        }

        public async Task<BaseResponse> RemoveSanPhamYeuThich(int maSanPham)
        {
            var sanPham = await myStoreDbContext.SanPhams
                .SingleOrDefaultAsync(s => s.MaSanPham == maSanPham)
                    ?? throw new NotFoundException("Không tìm thấy sản phẩm");

            var userId = httpContextAccessor.HttpContext.User.GetUserId();

            var dsYeuThich = await myStoreDbContext.DanhSachYeuThichs
                .Include(s => s.DanhSachSanPham)
                .SingleOrDefaultAsync(s => s.MaNguoiDung == userId)
                    ?? throw new Exception("Sản phẩm chưa nằm trong danh sách yêu thích");

            var isExist = dsYeuThich.DanhSachSanPham.Any(s => s.MaSanPham == sanPham.MaSanPham);
            if (isExist)
            {
                dsYeuThich.DanhSachSanPham.Remove(sanPham);
                await myStoreDbContext.SaveChangesAsync();
            }
            else throw new Exception("Sản phẩm chưa nằm trong danh sách yêu thích");

            return new BaseResponse()
            {
                Success = true,
                Message = "Xóa sản phẩm khỏi danh sách thành công",
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
