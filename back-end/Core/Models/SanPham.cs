using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Core.Models
{
    public class SanPham
    {
        [Key]
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string MoTa { get; set; }
        public double GiaCu { get; set; }
        public double GiaNhap { get; set; }
        public double GiaHienTai { get; set; }
        public string HinhDaiDien { get; set; }
        public string HinhPhongTo { get; set; }
        [ForeignKey(nameof(DanhMuc))]
        public int? MaDanhMuc { get; set; }
        public DanhMuc? DanhMuc { get; set; }
        [ForeignKey(nameof(NhanHieu))]
        public int? MaNhanHieu { get; set; }
        public NhanHieu? NhanHieu { get; set; }
        [ForeignKey(nameof(NhaSanXuat))]
        public int MaNhaSanXuat { get; set; }
        public NhaSanXuat NhaSanXuat { get; set; }

        public ICollection<HinhAnhSanPham>? DanhSachHinhAnh { get; set; }
        public ICollection<BienTheSanPham> DanhSachBienTheSP { get; set; }
        public ICollection<DanhGiaSanPham> DanhSachDanhGia { get; set; }
        public DateTime? NgayTao { get; set; } = DateTime.Now;

        public bool TrangThaiXoa { get; set; } = false;
        public ICollection<DanhSachYeuThich> DanhSachYeuThich { get; set; }
    }
}
