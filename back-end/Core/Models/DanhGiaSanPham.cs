using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Core.Models
{
    public class DanhGiaSanPham
    {
        [Key]
        public int MaDanhGiaSP { get; set; }
        public string NoiDung { get; set; }
        public int SoSaoDanhGia { get; set; }
        [ForeignKey(nameof(NguoiDung))]
        public string MaNguoiDanhGia { get; set; }
        public NguoiDung NguoiDanhGia { get; set; }
        [ForeignKey(nameof(SanPham))]
        public int MaSanPham { get; set; }
        public SanPham SanPham { get; set; }
        public ICollection<NguoiDung> DanhSachNguoiYeuThich { get; set; } = new List<NguoiDung>();
        public DateTime NgayTao { get; set; }
    }
}
