using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Core.Models
{
    public class BaiViet
    {
        [Key]
        public int MaBaiViet { get; set; }
        public string TieuDe { get; set; }
        public string HinhDaiDien { get; set; }
        public string VanBanTho { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayTao { get; set; }
        public bool TrangThaiXoa { get; set; }
        public bool TrangThaiAn { get; set; } = false;
        public NguoiDung TacGia { get; set; }
        [ForeignKey(nameof(TacGia))]
        public string MaTacGia { get; set; }

    }
}
