using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Core.Models
{
    public class TinNhan
    {
        [Key]
        public int MaTinNhan { get; set; }

        public string MaNguoiGui { get; set; }

        [ForeignKey(nameof(MaNguoiGui))]
        public NguoiDung NguoiGui { get; set; }

        public string MaNguoiNhan { get; set; }

        [ForeignKey(nameof(MaNguoiNhan))]
        public NguoiDung NguoiNhan { get; set; }

        public string NoiDung { get; set; }
        public List<TinNhanHinhAnh>? DanhSachHinhAnh { get; set; }
        public DateTime ThoiGianGui { get; set; } = DateTime.Now;
        public bool TrangThaiDoc { get; set; }
        public DateTime ThoiGianDoc { get; set; }
        public NhomChat NhomChat { get; set; }
    }

}
