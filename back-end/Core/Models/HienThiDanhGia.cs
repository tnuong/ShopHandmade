using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Core.Models
{
    public class HienThiDanhGia
    {
        [Key]
        public int MaHienThiDanhGia { get; set; }
        public DanhGiaSanPham DanhGiaSanPham { get; set; }
        [ForeignKey(nameof(DanhGiaSanPham))]
        public int MaDanhGiaSanPham { get; set; }
    }
}
