using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Core.Models
{
    public class BienTheSanPham
    {
        [Key]
        public int MaBienTheSanPham { get; set; }
        public int SoLuongTonKho { get; set; }
        [ForeignKey(nameof(KichThuoc))]
        public int MaKichThuoc { get; set; }
        [ForeignKey(nameof(MauSac))]
        public int MaMauSac { get; set; }
        [ForeignKey(nameof(SanPham))]
        public int MaSanPham { get; set; }
        public string DuongDanAnh { get; set; }
        public KichThuoc? KichThuoc { get; set; }
        public MauSac? MauSac { get; set; }
        public SanPham? SanPham { get; set; }
        public ICollection<ChiTietDonHang>? DanhSachChiTietDonHang { get; set; }
        public ICollection<HinhAnhBienTheSanPham>? DanhSachHinhAnh { get; set; }
        public bool TrangThaiXoa { get; set; } = false;
    }
}
