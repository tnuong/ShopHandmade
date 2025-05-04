using back_end.Core.Models;
using back_end.Core.Requests;
using back_end.Data;
using back_end.Extensions;
using back_end.Mappers;
using back_end.Services.Interfaces;
using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace back_end.Infrastructures.FCM
{
    public class FcmService : IFcmService
    {
        private readonly MyStoreDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly FirebaseMessaging _messaging;
        private readonly IThongBaoService notificationService;
        private readonly ApplicationMapper applicationMapper;


        public FcmService(MyStoreDbContext dbContext, IHttpContextAccessor httpContextAccessor, FirebaseMessaging firebaseMessaging, IThongBaoService notificationService, ApplicationMapper applicationMapper)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
            _messaging = firebaseMessaging;
            this.notificationService = notificationService;
            this.applicationMapper = applicationMapper;
        }

        public async Task<List<string>> GetAllDeviceTokensByUserId(string userId)
        {
            var tokens = await dbContext.ThietBiThongBaos
                .Where(x => x.MaNguoiDung == userId)
                .Select(x => x.MaToken)
                .ToListAsync();

            return tokens;
        }

        public async Task SaveTokenDevice(DeviceTokenRequest request)
        {
            var userId = httpContextAccessor.HttpContext.User.GetUserId();
            var existToken = await dbContext.ThietBiThongBaos
                .Where(token => token.MaToken == request.DeviceToken && token.MaNguoiDung != userId) 
                .OrderByDescending(token => token.ThoiGianTao)
                .ToListAsync();

            if(existToken != null)
            {
                dbContext.ThietBiThongBaos.RemoveRange(existToken);
            }

            var isUserIncludeToken = dbContext
                .ThietBiThongBaos.Any(u => u.MaToken == request.DeviceToken && u.MaNguoiDung == userId);

            if(!isUserIncludeToken)
            {
                var newTokenDevice = new ThietBiThongBao()
                {
                    ThoiGianTao = DateTime.Now,
                    MaNguoiDung = userId,
                    MaToken = request.DeviceToken,
                };

                await dbContext.ThietBiThongBaos.AddAsync(newTokenDevice);
            }

            
            await dbContext.SaveChangesAsync();
        }

        public async Task SendNotification(string title, string content, string recipientId, int referenceId, string notificationType, string toRole = "ADMIN")
        {
            try
            {
                var tokens = await dbContext.ThietBiThongBaos
                .Where(x => x.MaNguoiDung == recipientId)
                .Select(x => x.MaToken)
                .ToListAsync();

                if(tokens.Count > 0)
                {
                    Core.Models.ThongBao notification = new Core.Models.ThongBao()
                    {
                        NgayTao = DateTime.Now,
                        NoiDung = content,
                        TieuDe = title,
                        TrangThaiDoc = false,
                        LoaiThongBao = notificationType,
                        MaNguoiNhan = recipientId,
                        MaThamChieu = referenceId
                    };

                    var savedNotification = await notificationService.CreateNotification(notification);

                    var resource = applicationMapper.MapToNotificationResource(savedNotification);

                    var recipient = await dbContext.Users
                        .SingleOrDefaultAsync(s => s.Id == savedNotification.MaNguoiNhan) ?? new();
                    var mapRecipient = applicationMapper.MapToUserResourceWithoutRoles(recipient);
                    var recipientJson = JsonConvert.SerializeObject(mapRecipient);

                    var message = new FirebaseAdmin.Messaging.MulticastMessage()
                    {
                        Tokens = tokens,
                        Data = new Dictionary<string, string>()
                        {
                            { "id", resource.Id.ToString() }, 
                            { "title", resource.Title },
                            { "content", resource.Content },
                            { "referenceId", resource.ReferenceId?.ToString() },
                            { "recipient", recipientJson }, 
                            { "createdAt", savedNotification.NgayTao.ToString("O") }, 
                            { "haveRead", savedNotification.TrangThaiDoc.ToString() },     
                            { "notificationType", savedNotification.LoaiThongBao }
                        }
                    };

                    var response = await _messaging.SendEachForMulticastAsync(message);

                    if (response.FailureCount > 0)
                    {
                        var failedTokens = new List<string>();
                        for (int i = 0; i < response.Responses.Count; i++)
                        {
                            if (!response.Responses[i].IsSuccess)
                            {
                                failedTokens.Add(tokens[i]);
                            }
                        }

                        throw new Exception($"Có {response.FailureCount} lỗi xảy ra khi gửi thông báo tới các tokens: {string.Join(", ", failedTokens)}");
                    }
                }

            }
            catch (FirebaseMessagingException ex)
            {
                throw new Exception($"Lỗi firebase: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
