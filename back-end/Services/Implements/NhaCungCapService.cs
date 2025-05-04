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
    public class NhaCungCapService : INhaCungCapService
    {
        private readonly MyStoreDbContext dbContext;
        private readonly ApplicationMapper applicationMapper;

        public NhaCungCapService(MyStoreDbContext dbContext, ApplicationMapper applicationMapper)
        {
            this.dbContext = dbContext;
            this.applicationMapper = applicationMapper;
        }

        public async Task<BaseResponse> CreateSupplier(CreateSupplierRequest request)
        {
            var findByEmail = await dbContext.NhaCungCaps.FirstOrDefaultAsync(x => x.Email == request.Email);
            if (findByEmail != null)
                throw new NotFoundException("Email đã tồn tại");

            var findByPhoneNumber = await dbContext.NhaCungCaps.FirstOrDefaultAsync(x => x.SoDienThoai == request.PhoneNumber);
            if (findByPhoneNumber != null)
                throw new NotFoundException("SĐT đã tồn tại");

            var supplier = new NhaCungCap()
            {
                TenNhaCungCap = request.Name,
                TrangThai = request.Status,
                SoDienThoai = request.PhoneNumber,
                DiaChi = request.Address,
                Email = request.Email,
            };

            await dbContext.NhaCungCaps.AddAsync(supplier);
            await dbContext.SaveChangesAsync();

            return new BaseResponse()
            {
                Success = true,
                Message = "Thêm nhà cung cấp thành công",
                StatusCode = System.Net.HttpStatusCode.OK,
            };
        }

        public async Task<BaseResponse> DeleteSupplier(int id)
        {
            var supplier = await dbContext.NhaCungCaps
                .SingleOrDefaultAsync(x => x.MaNhaCungCap == id)
                    ?? throw new NotFoundException("Nhà cung cấp không tồn tại");
           
            dbContext.NhaCungCaps.Remove(supplier);
            await dbContext.SaveChangesAsync();

            return new BaseResponse()
            {
                Success = true,
                Message = "Xóa nhà cung cấp thành công",
                StatusCode = System.Net.HttpStatusCode.OK,
            };
        }

        public async Task<BaseResponse> GetAllSuppliers(int pageIndex, int pageSize, string searchString)
        {
            var lowerString = searchString?.ToLower() ?? "";
            var queryable = dbContext.NhaCungCaps
                .Where(c => c.TenNhaCungCap.ToLower().Contains(lowerString));

            List<NhaCungCap> suppliers = await queryable
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = new PaginationResponse<List<NhaCungCapResource>>
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Lấy thông tin nhà cung cấp thành công",
                Success = true,
                Data = suppliers.Select(supplier => applicationMapper.MapToSupplierResource(supplier)).ToList(),

                Pagination = new Pagination()
                {
                    TotalItems = queryable.Count(),
                    TotalPages = (int)Math.Ceiling((double)queryable.Count() / pageSize)
                }
            };

            return response;
        }

        public async Task<BaseResponse> GetSupplierById(int id)
        {
            var supplier = await dbContext.NhaCungCaps
                .SingleOrDefaultAsync(x => x.MaNhaCungCap == id)
                    ?? throw new NotFoundException("Nhà cung cấp không tồn tại");

            var response = new DataResponse<NhaCungCapResource>()
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Lấy thông tin nhà cung cấp thành công",
                Success = true,
                Data = applicationMapper.MapToSupplierResource(supplier)
            };

            return response;
        }

        public async Task<BaseResponse> UpdateSupplier(int id, EditSupplierRequest request)
        {
            var manufacturer = await dbContext.NhaCungCaps
              .SingleOrDefaultAsync(x => x.MaNhaCungCap == id) ?? throw new NotFoundException("Nhà cung cấp không tồn tại");

            var findByEmail = await dbContext.NhaCungCaps.FirstOrDefaultAsync(x => x.MaNhaCungCap != id && x.Email == request.Email);
            if (findByEmail != null)
                throw new NotFoundException("Email đã tồn tại");

            var findByPhoneNumber = await dbContext.NhaCungCaps.FirstOrDefaultAsync(x => x.MaNhaCungCap != id && x.SoDienThoai == request.PhoneNumber);
            if (findByPhoneNumber != null)
                throw new NotFoundException("SĐT đã tồn tại");

            manufacturer.TenNhaCungCap = request.Name;
            manufacturer.SoDienThoai = request.PhoneNumber;
            manufacturer.DiaChi = request.Address;
            manufacturer.Email = request.Email;
            manufacturer.TrangThai = request.Status;

            await dbContext.SaveChangesAsync();

            return new BaseResponse()
            {
                Success = true,
                Message = "Cập nhật nhà cung cấp thành công",
                StatusCode = System.Net.HttpStatusCode.OK,
            };
        }
    }
}
