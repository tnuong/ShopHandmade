using back_end.Core.DTOs;
using back_end.Core.Models;
using back_end.Core.Responses;
using back_end.Core.Responses.Resources;
using back_end.Data;
using back_end.Infrastructures.Cloudinary;
using back_end.Mappers;
using back_end.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace back_end.Services.Implements
{
    public class TinNhanService : ITinNhanService
    {
        private readonly MyStoreDbContext dbContext;
        private readonly ApplicationMapper applicationMapper;
        private readonly IUploadService uploadService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TinNhanService(MyStoreDbContext dbContext, ApplicationMapper applicationMapper, IUploadService uploadService, IHttpContextAccessor httpContextAccessor) { 
            this.dbContext = dbContext;
            this.applicationMapper = applicationMapper;
            this.uploadService = uploadService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<TinNhanResource> CreateNewMessage(MessageDTO messageDTO)
        {
            TinNhan message = new TinNhan();
            message.MaNguoiGui = messageDTO.SenderId;
            message.MaNguoiNhan = messageDTO.RecipientId;
            message.NoiDung = messageDTO.Content;
            message.ThoiGianGui = DateTime.Now;
            message.TrangThaiDoc = false;
            message.DanhSachHinhAnh = new List<TinNhanHinhAnh>();
            if(messageDTO.Images != null)
            {
                List<string> images = await uploadService.UploadMutlipleFilesAsync(messageDTO.Images);
                foreach (string image in images)
                {
                    TinNhanHinhAnh messageImage = new TinNhanHinhAnh();
                    messageImage.DuongDanAnh = image;
                    message.DanhSachHinhAnh.Add(messageImage);
                }
            }

            NhomChat? existedGroup = await dbContext.NhomChats
                .SingleOrDefaultAsync(g => g.MaNhomChat.Equals(messageDTO.GroupName));

            if (existedGroup == null)
            {
                NhomChat group = new NhomChat()
                {
                    MaNhomChat = messageDTO.GroupName,
                    TinNhanGanDay = message,
                    SoTinNhanChuaDoc = 1
                };

                await dbContext.NhomChats.AddAsync(group);
            } else
            {
                existedGroup.TinNhanGanDay = message;
                existedGroup.SoTinNhanChuaDoc += 1;
            }

            
            await dbContext.SaveChangesAsync();

            return applicationMapper.MapToMessageResource(message);
        }

        public async Task<BaseResponse> GetAllMessages(string recipientId)
        {
            var senderId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid);

            List<TinNhan> messages = await dbContext.TinNhans
                .Include(msg => msg.NguoiGui)
                .Include(msg => msg.NguoiNhan)
                .Where(msg =>
                    msg.MaNguoiNhan.Equals(recipientId) && msg.MaNguoiGui.Equals(senderId)
                   || msg.MaNguoiNhan.Equals(senderId) && msg.MaNguoiGui.Equals(recipientId)
                 )
                .ToListAsync();

            var response = new DataResponse<List<TinNhanResource>>();
            response.Message = "Lấy danh sách tin nhắn thành công";
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Success = true;
            response.Data = messages.Select(msg => applicationMapper.MapToMessageResource(msg)).ToList();
            return response;
        }
    }
}
