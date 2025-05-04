using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Core.Models
{
    public class ChiTietDonHang
    {
        [Key]
        public int MaCTDH { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }
        public double ThanhTien { get; set; }

        [ForeignKey(nameof(DonHang))]
        public int MaDonHang { get; set; }
        public DonHang DonHang { get; set; }

        [ForeignKey(nameof(BienTheSanPham))]
        public int MaBienTheSP { get; set; }
        public BienTheSanPham BienTheSanPham { get; set; }
    }
}
