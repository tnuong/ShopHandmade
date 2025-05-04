using back_end.Core.Models;
using back_end.Core.Requests;
using back_end.Core.Responses;
using back_end.Core.Responses.Resources;
using back_end.Data;
using back_end.Exceptions;
using back_end.Mappers;
using back_end.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace back_end.Services.Implements
{
    public class NhaSanXuatService : INhaSanXuatService
    {
        private readonly MyStoreDbContext dbContext;
        private readonly ApplicationMapper applicationMapper;

        public NhaSanXuatService(MyStoreDbContext dbContext, ApplicationMapper applicationMapper)
        {
            this.dbContext = dbContext;
            this.applicationMapper = applicationMapper;
        }

        public async Task<BaseResponse> CreateManufacturer(CreateManufacturerRequest request)
        {
            var findByEmail = await dbContext.NhaSanXuats.FirstOrDefaultAsync(x => x.Email == request.Email);
            if (findByEmail != null)
                throw new NotFoundException("Email đã tồn tại");

            var findByPhoneNumber = await dbContext.NhaSanXuats.FirstOrDefaultAsync(x => x.SoDienThoai == request.PhoneNumber);
            if (findByPhoneNumber != null)
                throw new NotFoundException("SĐT đã tồn tại");

            var manufacturer = new NhaSanXuat()
            {
                TenNhaSX = request.Name,
                MoTa = request.Description,
                SoDienThoai = request.PhoneNumber,
                DiaChi = request.Address,
                Email = request.Email
            };

            await dbContext.NhaSanXuats.AddAsync(manufacturer);
            await dbContext.SaveChangesAsync();

            return new BaseResponse()
            {
                Success = true,
                Message = "Thêm nhà sản xuất thành công",
                StatusCode = System.Net.HttpStatusCode.OK,
            };
        }

        public async Task<BaseResponse> DeleteManufacturer(int id)
        {
            var manufacturer = await dbContext.NhaSanXuats
                .Include(x => x.DanhSachSanPham)
                .SingleOrDefaultAsync(s => s.MaNhaSX == id) ?? throw new NotFoundException("Nhà sản xuất không tồn tại");

            if (manufacturer.DanhSachSanPham.Count > 0) throw new Exception("Nhà sản xuất này đang có sản phẩm, không thể xóa");
            
            dbContext.NhaSanXuats.Remove(manufacturer);

            await dbContext.SaveChangesAsync();

            return new BaseResponse()
            {
                Success = true,
                Message = "Xóa nhà sản xuất thành công",
                StatusCode = System.Net.HttpStatusCode.OK,
            };
        }

        public async Task<BaseResponse> GetAllManufacturers(int pageIndex, int pageSize, string searchString = "")
        {
            var lowerString = searchString?.ToLower() ?? "";
            var queryable = dbContext.NhaSanXuats
                .Where(c => c.TenNhaSX.ToLower().Contains(lowerString));

            List<NhaSanXuat> manufacturers = await queryable
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = new PaginationResponse<List<NhaSanXuatResource>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Lấy thông tin nhà sản xuất thành công",
                Success = true,
                Data = manufacturers.Select(manufacturer => applicationMapper.MapToManufacturerResource(manufacturer)).ToList(),

                Pagination = new Pagination()
                {
                    TotalItems = queryable.Count(),
                    TotalPages = (int)Math.Ceiling((double)queryable.Count() / pageSize)
                }
            };

            return response;
        }

        public async Task<BaseResponse> GetManufacturerById(int id)
        {
            var manufacturer = await dbContext.NhaSanXuats
                .SingleOrDefaultAsync(x => x.MaNhaSX == id) ?? throw new NotFoundException("Nhà sản xuất không tồn tại");

            var response = new DataResponse<NhaSanXuatResource>();
            response.StatusCode = HttpStatusCode.OK;
            response.Message = "Lấy thông tin nhà sản xuất thành công";
            response.Success = true;
            response.Data = applicationMapper.MapToManufacturerResource(manufacturer);

            return response;
        }

        public async Task<BaseResponse> UpdateManufacturer(int id, EditManufacturerRequest request)
        {
            var manufacturer = await dbContext.NhaSanXuats
              .SingleOrDefaultAsync(x => x.MaNhaSX == id) ?? throw new NotFoundException("Nhà sản xuất không tồn tại");

            var findByEmail = await dbContext.NhaSanXuats.FirstOrDefaultAsync(x => x.MaNhaSX != id && x.Email == request.Email);
            if (findByEmail != null)
                throw new NotFoundException("Email đã tồn tại");

            var findByPhoneNumber = await dbContext.NhaSanXuats.FirstOrDefaultAsync(x => x.MaNhaSX != id && x.SoDienThoai == request.PhoneNumber);
            if (findByPhoneNumber != null)
                throw new NotFoundException("SĐT đã tồn tại");

            manufacturer.TenNhaSX = request.Name;
            manufacturer.MoTa = request.Description;
            manufacturer.SoDienThoai = request.PhoneNumber;
            manufacturer.DiaChi = request.Address;
            manufacturer.Email = request.Email;

            await dbContext.SaveChangesAsync();

            return new BaseResponse()
            {
                Success = true,
                Message = "Cập nhật nhà sản xuất thành công",
                StatusCode = System.Net.HttpStatusCode.OK,
            };
        }
    }
}
