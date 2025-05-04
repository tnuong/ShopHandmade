using back_end.Core.DTOs;
using back_end.Core.Models;
using back_end.Core.Responses;
using back_end.Core.Responses.Resources;
using back_end.Data;
using back_end.Extensions;
using back_end.Mappers;
using back_end.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace back_end.Services.Implements
{
    public class ThongBaoService : IThongBaoService
    {
        private readonly MyStoreDbContext dbContext;
        private readonly ApplicationMapper applicationMapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ThongBaoService(MyStoreDbContext dbContext, ApplicationMapper applicationMapper, IHttpContextAccessor httpContextAccessor) { 
            this.dbContext = dbContext;
            this.applicationMapper = applicationMapper;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<ThongBao> CreateNotification(ThongBao notification)
        {
            var savedNotification = await dbContext.AddAsync(notification);
            int rows = await dbContext.SaveChangesAsync();
            if (rows == 0) throw new Exception("Thất bại khi tạo thông báo");
            await dbContext.Entry(savedNotification.Entity).Reference(n => n.NguoiNhan).LoadAsync();
            return savedNotification.Entity;
        }

        public async Task<BaseResponse> GetAllNotifications()
        {
            var userId = httpContextAccessor.HttpContext.User.GetUserId();
            var notifications = await dbContext.ThongBaos
                .Include(n => n.NguoiNhan)
                .Where(n => n.MaNguoiNhan.Equals(userId))
                .OrderByDescending(o => o.NgayTao)
                .Select(n => applicationMapper.MapToNotificationResource(n)).ToListAsync();

            return new DataResponse<List<ThongBaoResource>>
            {
                Data = notifications,
                Message = "Lấy thông báo thành công",
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true
            };
        }
    }
}
