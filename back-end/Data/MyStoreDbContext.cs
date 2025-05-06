using back_end.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace back_end.Data
{
    public class MyStoreDbContext : IdentityDbContext<NguoiDung>
    {
        public MyStoreDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<NhaSanXuat> NhaSanXuats { get; set; }  
        public DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public DbSet<HinhAnhSanPham> HinhAnhSanPhams { get; set; }
        public DbSet<BienTheSanPham> BienTheSanPhams { get; set; }
        public DbSet<HinhAnhBienTheSanPham> HinhAnhBienTheSanPhams { get; set; }
        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<KhuyenMai> KhuyenMais { get; set; }    
        public DbSet<SanPhamKhuyenMai> SanPhamKhuyenMais { get; set; }
        public DbSet<MauSac> MauSacs { get; set; }
        public DbSet<KichThuoc> KichThuocs { get; set; }
        public DbSet<NhanHieu> NhanHieus { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<LichSuDonHang> LichSuDonHangs { get; set; }
        public DbSet<DiaChiGiaoHang> DiaChiGiaoHangs { get; set; }
        public DbSet<ThanhToan> ThanhToans { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public DbSet<DanhGiaSanPham> DanhGiaSanPhams { get; set; }
        public DbSet<TinNhan> TinNhans { get; set; }
        public DbSet<NhomChat> NhomChats { get; set; }
        public DbSet<TinNhanHinhAnh> HinhAnhTinNhans { get; set; }
        public DbSet<ThietBiThongBao> ThietBiThongBaos { get; set; }
        public DbSet<BaiViet> BaiViets { get; set; }
        public DbSet<ThongBao> ThongBaos { get; set; }
        public DbSet<DanhSachYeuThich> DanhSachYeuThichs { get; set; }
        public DbSet<HienThiBanner> HienThiBanners { get; set; }
        public DbSet<HienThiDanhGia> HienThiDanhGias { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName!.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            builder.Ignore<IdentityUserLogin<string>>();
            builder.Ignore<IdentityUserClaim<string>>();
            builder.Ignore<IdentityUserToken<string>>();
            builder.Ignore<IdentityRoleClaim<string>>();

            builder.Entity<NguoiDung>(entity =>
            {
                entity.ToTable("NguoiDungs");
                entity.Property(u => u.UserName).HasColumnName("TenDangNhap");
                entity.Property(u => u.NormalizedUserName).HasColumnName("TenDangNhapChuanHoa");
                entity.Property(u => u.Email).HasColumnName("Email");
                entity.Property(u => u.NormalizedEmail).HasColumnName("EmailChuanHoa");
                entity.Property(u => u.EmailConfirmed).HasColumnName("XacThucEmail");
                entity.Property(u => u.PasswordHash).HasColumnName("MatKhauMaHoa");
                entity.Property(u => u.SecurityStamp).HasColumnName("MaBaoMat");
                entity.Property(u => u.ConcurrencyStamp).HasColumnName("MaDongBo");
                entity.Property(u => u.PhoneNumber).HasColumnName("SoDienThoai");
                entity.Property(u => u.PhoneNumberConfirmed).HasColumnName("XacThucSoDienThoai");
                entity.Property(u => u.TwoFactorEnabled).HasColumnName("KichHoat2Lop");
                entity.Property(u => u.LockoutEnd).HasColumnName("ThoiGianKhoa");
                entity.Property(u => u.LockoutEnabled).HasColumnName("ChoPhepKhoa");
                entity.Property(u => u.AccessFailedCount).HasColumnName("SoLanDangNhapThatBai");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable("VaiTros");

                entity.Property(r => r.Id).HasColumnName("MaVaiTro");
                entity.Property(r => r.Name).HasColumnName("TenVaiTro");
                entity.Property(r => r.NormalizedName).HasColumnName("TenVaiTroChuanHoa");
                entity.Property(r => r.ConcurrencyStamp).HasColumnName("MaDongBo");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("NguoiDungVaiTros");

                entity.Property(ur => ur.UserId).HasColumnName("MaNguoiDung");
                entity.Property(ur => ur.RoleId).HasColumnName("MaVaiTro");
            });



            builder.Entity<DonHang>()
                .HasOne(o => o.NguoiDung)
                .WithMany(u => u.DanhSachDonHang)
                .HasForeignKey(o => o.MaNguoiDung)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DanhMuc>()
                .HasOne(c => c.DanhMucCha)
                .WithMany(c => c.DanhSachDanhMucCon);

            builder.Entity<DanhGiaSanPham>()
                .HasOne(o => o.NguoiDanhGia)
                .WithMany(o => o.DanhSachDanhGia)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DanhGiaSanPham>()
                .HasMany(o => o.DanhSachNguoiYeuThich)
                .WithMany(o => o.DanhSachDanhGiaYeuThich);


            builder.Entity<TinNhan>()
                .HasOne(u => u.NguoiNhan)
                .WithMany(m => m.DanhSachTinNhanDaNhan)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TinNhan>()
                .HasOne(u => u.NguoiGui)
                .WithMany(m => m.DanhSachTinNhanDaGui)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<NhomChat>()
               .HasOne(n => n.TinNhanGanDay)  
               .WithOne(t => t.NhomChat)  
               .HasForeignKey<NhomChat>(n => n.MaTinNhan);

            SeedingData(builder);
        }

        public void SeedingData(ModelBuilder builder)
        {

            var roles = new List<IdentityRole>()
            {
                new IdentityRole() { Name = "ADMIN", ConcurrencyStamp = "1", NormalizedName = "ADMIN" },
                new IdentityRole() { Name = "CUSTOMER", ConcurrencyStamp = "1", NormalizedName = "CUSTOMER" },
                new IdentityRole() { Name = "EMPLOYEE", ConcurrencyStamp = "1", NormalizedName = "EMPLOYEE" }
            };

            // ROLE
            builder.Entity<IdentityRole>().HasData(roles);

            // USER
            var appUser = new NguoiDung
            {
                HoVaTen = "Thanh Nương",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                EmailConfirmed = true,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                PhoneNumber = "0123456789",
            };

            PasswordHasher<NguoiDung> hashedPassword = new PasswordHasher<NguoiDung>();
            appUser.PasswordHash = hashedPassword.HashPassword(appUser, "12345678");

            builder.Entity<NguoiDung>().HasData(appUser);

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { UserId = appUser.Id, RoleId = roles[0].Id }
            );

        }
    }
}
