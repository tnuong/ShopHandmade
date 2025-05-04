using System.ComponentModel.DataAnnotations;

namespace back_end.Core.Models
{
    public class NhaSanXuat
    {
        [Key]
        public int MaNhaSX { get; set; }
        public string TenNhaSX { get; set; }
        public string MoTa { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }

        public ICollection<SanPham> DanhSachSanPham { get; set; } = new List<SanPham>();
    }
}
