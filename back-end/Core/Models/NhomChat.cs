using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Core.Models
{
    public class NhomChat
    {
        [Key]
        public string MaNhomChat { get; set; }
        public TinNhan TinNhanGanDay { get; set; }
        [ForeignKey(nameof(TinNhan))]
        public int MaTinNhan { get; set; }
        public int SoTinNhanChuaDoc { get; set; }
    }
}
