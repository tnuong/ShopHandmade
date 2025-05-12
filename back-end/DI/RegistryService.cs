using back_end.Helpers;
using back_end.Infrastructures.MailSender;
using back_end.Infrastructures.SignalR;
using back_end.Mappers;
using back_end.Middleware;
using back_end.Services.Implements;
using back_end.Services.Interfaces;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using back_end.Infrastructures.FCM;
using back_end.Infrastructures.Cloudinary;
using back_end.Infrastructures.JsonWebToken;


namespace back_end.DI
{
    public static class RegistryService
    {
        public static IServiceCollection AddRegistryService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ApplicationMapper>();
            services.AddScoped<ExceptionHandlerMiddleware>();

            services.AddScoped<IDanhMucService, DanhMucService>();
            services.AddScoped<INhaSanXuatService, NhaSanXuatService>();
            services.AddScoped<INhaCungCapService, NhaCungCapService>();
            services.AddScoped<IXacThucService, XacThucService>();
            services.AddScoped<ISanPhamService, SanPhamService>();
            services.AddScoped<IThuongHieuService, ThuongHieuService>();
            services.AddScoped<IMauSacService, MauSacService>();
            services.AddScoped<IKichThuocService, KichThuocService>();
            services.AddScoped<IBienTheSanPhamService, BienTheSanPhamService>();
            services.AddScoped<IDonHangService, DonHangService>();
            services.AddScoped<ITaiKhoanService, TaiKhoanService>();
            services.AddScoped<IDiaChiGiaoHangService, DiaChiGiaoHangService>();
            services.AddScoped<IDanhGiaService, DanhGiaService>();
            services.AddScoped<IBaoCaoThongKeService, BaoCaoThongKeService>();
            services.AddScoped<IBaiVietService, BaiVietService>();
            services.AddScoped<IHienThiBannerService, HienThiBannerService>();
            services.AddScoped<IHienThiDanhGiaService, HienThiDanhGiaService>();
            services.AddScoped<IKhuyenMaiService, KhuyenMaiService>();
            services.AddScoped<IVaiTroService, VaiTroService>();
            services.AddScoped<ISanPhamYeuThichService, SanPhamYeuThichService>();
            services.AddScoped<IThongBaoService, ThongBaoService>();

            services.AddScoped<IVnpayService, VnpayService>();
            services.AddScoped<IPaypalService, PaypalService>();

            services.AddScoped<ITinNhanService, TinNhanService>();
            services.AddScoped<INhomChatService, NhomChatService>();
            services.AddScoped<IFcmService, FcmService>();

            services.AddSingleton<PresenceTracker>();


            services.AddSingleton(x =>
                new PaypalClient(
                    configuration["PayPalOptions:ClientId"],
                    configuration["PayPalOptions:ClientSecret"],
                    configuration["PayPalOptions:Mode"]
                )
            );

         
            services.AddSingleton<FirebaseMessaging>(provider =>
            {
                if (FirebaseApp.DefaultInstance == null)
                {
                    FirebaseApp.Create(new AppOptions
                    {
                        Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "firebase-service-account.json")),
                    });
                }

                return FirebaseMessaging.DefaultInstance;
            });

            services.AddScoped<IUploadService, UploadService>();
            services.AddScoped<JwtService>();
            services.AddScoped<MailService>();

            services.Configure<CloudinarySettings>(configuration.GetSection(nameof(CloudinarySettings)));

            return services;
        }
    }
}
