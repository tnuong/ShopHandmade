using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Core.Models
{
    public class ThietBiThongBao
    {
        [Key]
        public int MaThietBi { get; set; }
        public string MaToken { get; set; }
        public DateTime ThoiGianTao { get; set; }
        [ForeignKey(nameof(NguoiDung))]
        public string MaNguoiDung { get; set; }
        public NguoiDung NguoiDung { get; set; }
    }
}
