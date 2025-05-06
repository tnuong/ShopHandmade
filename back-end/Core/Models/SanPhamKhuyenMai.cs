using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Core.Models
{
    public class SanPhamKhuyenMai
    {
        [Key]
        public int MaSanPhamKhuyenMai { get; set; }

        [ForeignKey(nameof(KhuyenMai))]
        public int MaKhuyenMai { get; set; }
        public KhuyenMai KhuyenMai { get; set; }
        [ForeignKey(nameof(SanPham))]
        public int MaSanPham { get; set; }
        public SanPham SanPham { get; set; }
    }
}
