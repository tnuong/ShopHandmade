using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Core.Models
{
    public class DanhMuc
    {
        [Key]
        public int MaDanhMuc { get; set; }

        public string TenDanhMuc { get; set; }

        public string MoTa { get; set; }

        public bool TrangThaiXoa { get; set; } = false;

        [ForeignKey(nameof(DanhMuc))]
        public int? MaDanhMucCha { get; set; }
        public DanhMuc? DanhMucCha { set; get; }
        public ICollection<DanhMuc>? DanhSachDanhMucCon { get; set; }
        public ICollection<SanPham>? DanhSachSanPham { get; set; }
    }
}
