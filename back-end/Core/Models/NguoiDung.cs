using Microsoft.AspNetCore.Identity;

namespace back_end.Core.Models
{
    public class NguoiDung : IdentityUser
    {
        public string HoVaTen { get; set; }
        public string? HinhDaiDien { get; set; }
        public string? HinhAnhBia { get; set; }
        public ICollection<DonHang> DanhSachDonHang { get; set; }
        public List<DiaChiGiaoHang> DanhSachDiaChiGiaoHang { get; set; }
        public List<DanhGiaSanPham> DanhSachDanhGia { get; set; }
        public List<DanhGiaSanPham> DanhSachDanhGiaYeuThich { get; set; }
        public bool TrangThaiHoatDong { get; set; }
        public DateTime ThoiGianHoatDongGanDay { get; set; }
        public List<ThietBiThongBao> DanhSachThietBiThongBao { get; set; }
        public List<BaiViet> DanhSachBaiViet { get; set; }
        public List<TinNhan> DanhSachTinNhanDaGui { get; set; }
        public List<TinNhan> DanhSachTinNhanDaNhan { get; set; }
        public List<ThongBao> DanhSachThongBao { get; set; }
        public DanhSachYeuThich DanhSachYeuThich { get; set; }
        public DateTime? NgayTao { get; set; } = DateTime.Now;
        public bool TrangThaiKhoa { get; set; } = false;
    }
}
