using System.ComponentModel.DataAnnotations;

namespace back_end.Core.Models
{
    public class ThanhToan
    {
        [Key]
        public int MaThanhToan { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? ThoiGianThanhToan { get; set; }
        public bool TrangThai { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public string? MaGiaoDich { get; set; }
        public DonHang DonHang { get; set; }
    }
}
