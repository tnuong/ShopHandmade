using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Core.Models
{
    public class DanhSachYeuThich
    {
        [Key]
        public int MaDanhSachYeuThich { get; set; }
        public ICollection<SanPham> DanhSachSanPham { get; set; }
        [ForeignKey(nameof(NguoiDung))]
        public string MaNguoiDung { get; set; }
        public NguoiDung NguoiDung { get; set; }
    }
}
