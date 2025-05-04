using System.ComponentModel.DataAnnotations;

namespace back_end.Core.Models
{
    public class NhanHieu
    {
        [Key]
        public int MaNhanHieu { get; set; }

        public string TenNhanHieu { get; set; }

        public string MoTa { get; set; }

        public bool TrangThaiXoa { get; set; } = false;

        public ICollection<SanPham>? SanPhams { get; set; }
    }
}
