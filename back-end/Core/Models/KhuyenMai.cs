using System.ComponentModel.DataAnnotations;

namespace back_end.Core.Models
{
    public class KhuyenMai
    {
        [Key]
        public int MaKhuyenMai { get; set; }
        public string TenKhuyenMai { get; set; }
        public string LoaiKhuyenMai { get; set; }
        public double GiaTriGiam { get; set; }
        public string NoiDungKhuyenMai { get; set; }
        public string TrangThai { get; set;  }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }

        public ICollection<SanPhamKhuyenMai> SanPhamKhuyenMais { get; set; }
    }
}
