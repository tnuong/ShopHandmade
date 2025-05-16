using back_end.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace back_end.Infrastructures.OneSignal
{
    public class OnesignalService
    {
        private readonly ILogger<OnesignalService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IThongBaoService notificationService;

        public OnesignalService(ILogger<OnesignalService> logger, IConfiguration configuration, IThongBaoService notificationService) {
            _logger = logger;
            _configuration = configuration;
            this.notificationService = notificationService;
        }
        public async Task SendNotification(string title, string body, string recipientId, int referenceId, string notificationType, string toRole = "ADMIN")
        {
            var oneSignalKey = _configuration["OneSignal:Key"];
            var appId = _configuration["OneSignal:AppId"];

            Core.Models.ThongBao notification = new Core.Models.ThongBao()
            {
                NgayTao = DateTime.Now,
                NoiDung = body,
                TieuDe = title,
                TrangThaiDoc = false,
                LoaiThongBao = notificationType,
                MaNguoiNhan = recipientId,
                MaThamChieu = referenceId
            };

            await notificationService.CreateNotification(notification);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Key", oneSignalKey);

            var data = new
            {
                app_id = appId,
                headings = new { en = title ?? "Thông báo mới" },
                contents = new { en = body },
                include_external_user_ids = new[] { recipientId },
            };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.onesignal.com/notifications?c=push", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Notification sent successfully: " + result);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Failed to send notification: " + error);
            }
        }
    }
}
