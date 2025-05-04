using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Core.Models
{
    public class TinNhanHinhAnh
    {
        [Key]
        public int MaTinNhanHinhAnh { get; set; }
        public string DuongDanAnh { get; set; }
        [ForeignKey(nameof(TinNhan))]
        public int MaTinNhan { get; set; }
        public TinNhan TinNhan { get; set; }
    }
}
