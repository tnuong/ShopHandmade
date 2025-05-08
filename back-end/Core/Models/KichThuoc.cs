using System.ComponentModel.DataAnnotations;

namespace back_end.Core.Models
{
    public class KichThuoc
    {
        [Key]
        public int MaKichThuoc { get; set; }
        public string TenKichThuoc { get; set; }
        public string MoTa { get; set; }

        public bool TrangThaiXoa { get; set; } = false;

        public ICollection<BienTheSanPham>? DanhSachBienTheSanPham { get; set; }
    }
}
