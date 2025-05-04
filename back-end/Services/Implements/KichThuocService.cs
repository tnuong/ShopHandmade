using back_end.Core.Models;
using back_end.Core.Requests;
using back_end.Core.Responses;
using back_end.Core.Responses.Resources;
using back_end.Data;
using back_end.Exceptions;
using back_end.Mappers;
using back_end.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace back_end.Services.Implements
{
    public class KichThuocService : IKichThuocService
    {
        private readonly MyStoreDbContext myStoreDbContext;
        private readonly ApplicationMapper applicationMapper;

        public KichThuocService(MyStoreDbContext myStoreDbContext, ApplicationMapper applicationMapper)
        {
            this.myStoreDbContext = myStoreDbContext;
            this.applicationMapper = applicationMapper;
        }

        public async Task<BaseResponse> CreateSize(SizeRequest request)
        {
            KichThuoc size = new KichThuoc();
            size.TenKichThuoc = request.ESize;
            size.MoTa = request.Description;
          
            var savedSize = await myStoreDbContext.KichThuocs.AddAsync(size);
            await myStoreDbContext.SaveChangesAsync();

            var response = new DataResponse<KichThuocResource>();
            response.StatusCode = System.Net.HttpStatusCode.Created;
            response.Message = "Thêm kích cỡ thành công";
            response.Success = true;
            response.Data = applicationMapper.MapToSizeResource(savedSize.Entity);
            return response;
        }

        public async Task<BaseResponse> GetAllSizes()
        {
            List<KichThuoc> sizes = await myStoreDbContext.KichThuocs
                .Where(p => p.TrangThaiXoa == false)
                .ToListAsync();

            var response = new DataResponse<List<KichThuocResource>>();
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Message = "Lấy danh sách kích cỡ thành công";
            response.Success = true;
            response.Data = sizes.Select(size => applicationMapper.MapToSizeResource(size)).ToList();
            return response;
        }

        public async Task RemoveSize(int id)
        {
            KichThuoc? size = await myStoreDbContext.KichThuocs
               .Include(s => s.ProductVariants)
               .SingleOrDefaultAsync(c => c.MaKichThuoc == id && c.TrangThaiXoa == false)
                   ?? throw new NotFoundException("Không tìm thấy kích cỡ");

            if(size.ProductVariants != null && size.ProductVariants.Any())
            {
                size.TrangThaiXoa = true;
            } else
            {
                myStoreDbContext.KichThuocs.Remove(size);
            }

            int rows = await myStoreDbContext.SaveChangesAsync();
            if (rows == 0) throw new Exception("Xóa kích cỡ thất bại");
        }

        public async Task<BaseResponse> UpdateSize(int id, SizeRequest request)
        {
            KichThuoc? size = await myStoreDbContext.KichThuocs
               .SingleOrDefaultAsync(c => c.MaKichThuoc == id && !c.TrangThaiXoa)
                   ?? throw new NotFoundException("Không tìm thấy kích cỡ");

            size.TenKichThuoc = request.ESize;
            size.MoTa = request.Description;
         

            await myStoreDbContext.SaveChangesAsync();

            var response = new DataResponse<KichThuocResource>();
            response.StatusCode = System.Net.HttpStatusCode.NoContent;
            response.Message = "Cập nhật kích cỡ thành công";
            response.Success = true;
            response.Data = applicationMapper.MapToSizeResource(size);
            return response;
        }
    }
}
