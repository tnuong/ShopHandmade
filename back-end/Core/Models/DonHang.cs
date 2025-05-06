using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_end.Core.Models
{
    public class DonHang
    {
        [Key]
        public int MaDonHang { get; set; }

        public DateTime NgayTao { get; set; }
        public double TotalAmount { get; set; }        
        public double TienKhuyenMai { get; set; }    
        public double TongTienSauKhuyenMai { get; set; }        
        public double TongTienTruocKhuyenMai { get; set; }

        public int SoLuong { get; set; }
        public string GhiChu { get; set; }

        public string TrangThai { get; set; }
        public bool TrangThaiXoa { get; set; } = false;

        [ForeignKey(nameof(NguoiDung))]
        public string MaNguoiDung { get; set; }
        public NguoiDung NguoiDung { get; set; }

        [ForeignKey(nameof(ThanhToan))]
        public int MaThanhToan { get; set; }
        public ThanhToan ThanhToan { get; set; }
        public List<LichSuDonHang> LichSuDonHang { get; set; } = new List<LichSuDonHang>();
        [ForeignKey(nameof(DiaChiGiaoHang))]
        public int MaDiaChiGiaoHang { get; set; }
        public DiaChiGiaoHang DiaChiGiaoHang { get; set; }
        public ICollection<ChiTietDonHang> DanhSachChiTietDonHang { get; set; }
    }
}
