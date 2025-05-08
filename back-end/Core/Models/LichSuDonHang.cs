using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Core.Models
{
    public class LichSuDonHang
    {
        [Key]
        public int MaLichSuDonHang { get; set; }
        public DateTime? ThoiGianChinhSua { get; set; }
        public string TrangThai { get; set; }
        public string? GhiChu { get; set; }
        [ForeignKey(nameof(DonHang))]
        public int MaDonHang { get; set; }
        public DonHang DonHang { get; set; }
        [ForeignKey(nameof(NhanVien))]
        public string? MaNhanVien { get; set; }
        public NguoiDung? NhanVien { get; set; }
    }
}
