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
    public class MauSacService : IMauSacService
    {
        private readonly MyStoreDbContext dbContext;
        private readonly ApplicationMapper applicationMapper;

        public MauSacService(ApplicationMapper applicationMapper, MyStoreDbContext dbContext)
        {
            this.applicationMapper = applicationMapper;
            this.dbContext = dbContext;
        }

     
        public async Task<BaseResponse> CreateColor(ColorRequest request)
        {
            MauSac color = new MauSac();
            color.MaThapLucPhan = request.HexCode;
            color.TenMauSac = request.Name;

            var savedColor = await dbContext.MauSacs.AddAsync(color);
            await dbContext.SaveChangesAsync();

            var response = new DataResponse<MauSacResource>();
            response.StatusCode = System.Net.HttpStatusCode.Created;
            response.Message = "Thêm màu sắc thành công";
            response.Success = true;
            response.Data = applicationMapper.MapToColorResource(savedColor.Entity);
            return response;
        }

        public async Task<BaseResponse> GetAllColors()
        {
            List<MauSac> colors = await dbContext.MauSacs
                .Where(c => !c.TrangThaiXoa)
                .ToListAsync();

            var response = new DataResponse<List<MauSacResource>>();
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Message = "Lấy danh sách màu sắc thành công";
            response.Success = true;
            response.Data = colors.Select(color => applicationMapper.MapToColorResource(color)).ToList();
            return response;
        }

        public async Task RemoveColor(int id)
        {
            MauSac? color = await dbContext.MauSacs
                .Include(c => c.DanhSachBienTheSP)
                .SingleOrDefaultAsync(c => c.MaMauSac == id && !c.TrangThaiXoa)
                    ?? throw new NotFoundException("Không tìm thấy màu sắc");

            if(color.DanhSachBienTheSP != null && color.DanhSachBienTheSP.Any())
            {
                color.TrangThaiXoa = true;
            } else
            {
                dbContext.MauSacs.Remove(color);
            }

            int rows = await dbContext.SaveChangesAsync();
            if (rows == 0) throw new Exception("Xóa màu sắc thất bại");
        }

        public async Task<BaseResponse> UpdateColor(int id, ColorRequest request)
        {
            MauSac? color = await dbContext.MauSacs
                .SingleOrDefaultAsync(c => c.MaMauSac == id && c.TrangThaiXoa == false)
                    ?? throw new NotFoundException("Không tìm thấy màu sắc");

            color.TenMauSac = request.Name;
            color.MaThapLucPhan = request.HexCode;

            await dbContext.SaveChangesAsync();

            var response = new DataResponse<MauSacResource>();
            response.StatusCode = System.Net.HttpStatusCode.NoContent;
            response.Message = "Cập nhật màu sắc thành công";
            response.Success = true;
            response.Data = applicationMapper.MapToColorResource(color);
            return response;
        }
    }
}
