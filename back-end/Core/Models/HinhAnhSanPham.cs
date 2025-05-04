using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Core.Models
{
    public class HinhAnhSanPham
    {
        [Key]
        public int MaHinhAnhSanPham { get; set; }
        public string DuongDanAnh { get; set; }

        [ForeignKey(nameof(SanPham))]
        public int? MaSanPham { get; set; }
        public SanPham SanPham { get; set; }
    }
}
