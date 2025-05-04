using back_end.Core.Models;
using back_end.Core.Requests;
using back_end.Core.Responses;
using back_end.Core.Responses.Resources;
using back_end.Data;
using back_end.Mappers;
using back_end.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace back_end.Services.Implements
{
    public class ThuongHieuService : IThuongHieuService
    {
        private readonly MyStoreDbContext dbContext;
        private readonly ApplicationMapper applicationMapper;

        public ThuongHieuService(MyStoreDbContext dbContext, ApplicationMapper applicationMapper)
        {
            this.dbContext = dbContext;
            this.applicationMapper = applicationMapper;
        }

        public async Task<BaseResponse> CreateBrand(BrandRequest request)
        {
            NhanHieu brand = new NhanHieu();
            brand.TenNhanHieu = request.Name;
            brand.MoTa = request.Description;

            await dbContext.NhanHieus.AddAsync(brand);
            await dbContext.SaveChangesAsync();

            var response = new DataResponse<ThuongHieuResource>();
            response.Message = "Thêm thương hiệu mới thành công";
            response.Success = true;
            response.StatusCode = HttpStatusCode.Created;
            response.Data = applicationMapper.MapToBrandResource(brand);

            return response;
        }

        public async Task<BaseResponse> GetAllBrands(int pageIndex, int pageSize, string searchString)
        {
            var lowerString = searchString?.ToLower() ?? "";
            var queryable = dbContext.NhanHieus
                .Where(br => br.TrangThaiXoa == false && br.TenNhanHieu.ToLower().Contains(lowerString));

            List<NhanHieu> brands = await queryable
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = new PaginationResponse<List<ThuongHieuResource>>();
            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Message = "Lấy thông tin thương hiệu thành công";
            response.Data = brands.Select(br => applicationMapper.MapToBrandResource(br)).ToList();
            response.Pagination = new Pagination
            {
                TotalItems = queryable.Count(),
                TotalPages = (int)Math.Ceiling((double)queryable.Count() / pageSize),
            };
            return response;
        }

        public async Task<BaseResponse> GetBrandById(int id)
        {
            NhanHieu? brand = await dbContext.NhanHieus
                .SingleOrDefaultAsync(br => br.MaNhanHieu == id && br.TrangThaiXoa == false)
                    ?? throw new DirectoryNotFoundException("Không tìm thấy thương hiệu");

            var response = new DataResponse<ThuongHieuResource>();
            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Message = "Lấy thông tin thương hiệu thành công";
            response.Data = applicationMapper.MapToBrandResource(brand);

            return response;
        }

        public async Task RemoveBrand(int id)
        {
            NhanHieu? brand = await dbContext.NhanHieus
                .Include(b => b.SanPhams)
                .SingleOrDefaultAsync(br => br.MaNhanHieu == id && br.TrangThaiXoa == false)
                    ?? throw new DirectoryNotFoundException("Không tìm thấy thương hiệu");

            if(brand.SanPhams != null && brand.SanPhams.Any())
            {
                brand.TrangThaiXoa = true;
            } else
            {
                dbContext.NhanHieus.Remove(brand);
            }

            int rows = await dbContext.SaveChangesAsync();
            if (rows == 0) throw new Exception("Xóa danh mục thất bại");
        }

        public async Task<BaseResponse> UpdateBrand(int id, BrandRequest request)
        {
            NhanHieu? brand = await dbContext.NhanHieus
                .SingleOrDefaultAsync(br => br.MaNhanHieu == id && br.TrangThaiXoa == false)
                    ?? throw new DirectoryNotFoundException("Không tìm thấy thương hiệu");
            
            brand.TenNhanHieu = request.Name;
            brand.MoTa = request.Description;

            await dbContext.SaveChangesAsync();

            var response = new DataResponse<ThuongHieuResource>();
            response.Success = true;
            response.StatusCode = HttpStatusCode.NoContent;
            response.Message = "Cập nhật thông tin thương hiệu thành công";
            response.Data = applicationMapper.MapToBrandResource(brand);

            return response;

        }
    }
}
