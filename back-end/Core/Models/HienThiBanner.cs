using System.ComponentModel.DataAnnotations;

namespace back_end.Core.Models
{
    public class HienThiBanner
    {
        [Key]
        public int MaHienThiBanner { get; set; }
        public string TieuDe { get; set; }
        public string MoTaNgan { get; set; }
        public string NutHanhDong { get; set; }
        public string DuongDanhAnhNen { get; set; }
        public int SoThuTu { get; set; }
    }
}
