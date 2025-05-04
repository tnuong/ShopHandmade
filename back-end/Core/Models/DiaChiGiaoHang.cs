using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Core.Models
{
    public class DiaChiGiaoHang
    {
        [Key]
        public int MaDCGH { get; set; }
        public string HoVaTen { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public bool MacDinh { get; set; }
        public NguoiDung NguoiDung { get; set; }
        [ForeignKey(nameof(NguoiDung))]
        public string MaNguoiDung { get; set; }
        public List<DonHang> DonHangs { get; set; }
    }
}
