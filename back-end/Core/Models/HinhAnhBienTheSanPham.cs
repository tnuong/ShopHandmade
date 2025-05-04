using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Core.Models
{
    public class HinhAnhBienTheSanPham
    {
        [Key]
        public int MaHinhAnhBienTheSanPham { get; set; }
        public string DuongDanAnh { get; set; }

        [ForeignKey(nameof(BienTheSanPham))]
        public int? MaBienTheSanPham { get; set; }
        public BienTheSanPham BienTheSanPham { get; set; }
    }
}
