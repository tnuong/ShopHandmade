using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Core.Models
{
    public class ThongBao
    {
        [Key]
        public int MaThongBao { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayTao { get; set; }
        [ForeignKey(nameof(NguoiDung))]
        public string MaNguoiNhan { get; set; }
        public NguoiDung NguoiNhan { get; set; }
        public bool TrangThaiDoc {  get; set; }
        public int? MaThamChieu { get; set; }
        public string LoaiThongBao { get; set; }
    }
}
