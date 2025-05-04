using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace back_end.Core.Models
{
    public class MauSac
    {
        [Key]
        public int MaMauSac { get; set; }
        public string TenMauSac { get; set; }
        public string MaThapLucPhan { get; set; }
        public bool TrangThaiXoa { get; set; } = false;
        public ICollection<BienTheSanPham>? DanhSachBienTheSP { get; set; }
    }
}
