using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace back_end.Migrations
{
    /// <inheritdoc />
    public partial class initdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DanhMucs",
                columns: table => new
                {
                    MaDanhMuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDanhMuc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThaiXoa = table.Column<bool>(type: "bit", nullable: false),
                    MaDanhMucCha = table.Column<int>(type: "int", nullable: true),
                    DanhMucChaMaDanhMuc = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMucs", x => x.MaDanhMuc);
                    table.ForeignKey(
                        name: "FK_DanhMucs_DanhMucs_DanhMucChaMaDanhMuc",
                        column: x => x.DanhMucChaMaDanhMuc,
                        principalTable: "DanhMucs",
                        principalColumn: "MaDanhMuc");
                });

            migrationBuilder.CreateTable(
                name: "HienThiBanners",
                columns: table => new
                {
                    MaHienThiBanner = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TieuDe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTaNgan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NutHanhDong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DuongDanhAnhNen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoThuTu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HienThiBanners", x => x.MaHienThiBanner);
                });

            migrationBuilder.CreateTable(
                name: "KhuyenMais",
                columns: table => new
                {
                    MaKhuyenMai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenKhuyenMai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoaiKhuyenMai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GiaTriGiam = table.Column<double>(type: "float", nullable: false),
                    NoiDungKhuyenMai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhuyenMais", x => x.MaKhuyenMai);
                });

            migrationBuilder.CreateTable(
                name: "KichThuocs",
                columns: table => new
                {
                    MaKichThuoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenKichThuoc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThaiXoa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KichThuocs", x => x.MaKichThuoc);
                });

            migrationBuilder.CreateTable(
                name: "MauSacs",
                columns: table => new
                {
                    MaMauSac = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenMauSac = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaThapLucPhan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThaiXoa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MauSacs", x => x.MaMauSac);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDungs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HoVaTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HinhDaiDien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HinhAnhBia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThaiHoatDong = table.Column<bool>(type: "bit", nullable: false),
                    ThoiGianHoatDongGanDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThaiKhoa = table.Column<bool>(type: "bit", nullable: false),
                    TenDangNhap = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenDangNhapChuanHoa = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailChuanHoa = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    XacThucEmail = table.Column<bool>(type: "bit", nullable: false),
                    MatKhauMaHoa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaBaoMat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaDongBo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    XacThucSoDienThoai = table.Column<bool>(type: "bit", nullable: false),
                    KichHoat2Lop = table.Column<bool>(type: "bit", nullable: false),
                    ThoiGianKhoa = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ChoPhepKhoa = table.Column<bool>(type: "bit", nullable: false),
                    SoLanDangNhapThatBai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDungs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCaps",
                columns: table => new
                {
                    MaNhaCungCap = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhaCungCap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaCungCaps", x => x.MaNhaCungCap);
                });

            migrationBuilder.CreateTable(
                name: "NhanHieus",
                columns: table => new
                {
                    MaNhanHieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhanHieu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThaiXoa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanHieus", x => x.MaNhanHieu);
                });

            migrationBuilder.CreateTable(
                name: "NhaSanXuats",
                columns: table => new
                {
                    MaNhaSX = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhaSX = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaSanXuats", x => x.MaNhaSX);
                });

            migrationBuilder.CreateTable(
                name: "ThanhToans",
                columns: table => new
                {
                    MaThanhToan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianThanhToan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false),
                    PhuongThucThanhToan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaGiaoDich = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhToans", x => x.MaThanhToan);
                });

            migrationBuilder.CreateTable(
                name: "VaiTros",
                columns: table => new
                {
                    MaVaiTro = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenVaiTro = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenVaiTroChuanHoa = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    MaDongBo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaiTros", x => x.MaVaiTro);
                });

            migrationBuilder.CreateTable(
                name: "BaiViets",
                columns: table => new
                {
                    MaBaiViet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TieuDe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HinhDaiDien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VanBanTho = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThaiXoa = table.Column<bool>(type: "bit", nullable: false),
                    TrangThaiAn = table.Column<bool>(type: "bit", nullable: false),
                    MaTacGia = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaiViets", x => x.MaBaiViet);
                    table.ForeignKey(
                        name: "FK_BaiViets_NguoiDungs_MaTacGia",
                        column: x => x.MaTacGia,
                        principalTable: "NguoiDungs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DanhSachYeuThichs",
                columns: table => new
                {
                    MaDanhSachYeuThich = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhSachYeuThichs", x => x.MaDanhSachYeuThich);
                    table.ForeignKey(
                        name: "FK_DanhSachYeuThichs_NguoiDungs_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiaChiGiaoHangs",
                columns: table => new
                {
                    MaDCGH = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoVaTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MacDinh = table.Column<bool>(type: "bit", nullable: false),
                    MaNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaChiGiaoHangs", x => x.MaDCGH);
                    table.ForeignKey(
                        name: "FK_DiaChiGiaoHangs_NguoiDungs_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThietBiThongBaos",
                columns: table => new
                {
                    MaThietBi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThietBiThongBaos", x => x.MaThietBi);
                    table.ForeignKey(
                        name: "FK_ThietBiThongBaos_NguoiDungs_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThongBaos",
                columns: table => new
                {
                    MaThongBao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TieuDe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaNguoiNhan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TrangThaiDoc = table.Column<bool>(type: "bit", nullable: false),
                    MaThamChieu = table.Column<int>(type: "int", nullable: true),
                    LoaiThongBao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaos", x => x.MaThongBao);
                    table.ForeignKey(
                        name: "FK_ThongBaos_NguoiDungs_MaNguoiNhan",
                        column: x => x.MaNguoiNhan,
                        principalTable: "NguoiDungs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TinNhans",
                columns: table => new
                {
                    MaTinNhan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiGui = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaNguoiNhan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThoiGianGui = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThaiDoc = table.Column<bool>(type: "bit", nullable: false),
                    ThoiGianDoc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinNhans", x => x.MaTinNhan);
                    table.ForeignKey(
                        name: "FK_TinNhans_NguoiDungs_MaNguoiGui",
                        column: x => x.MaNguoiGui,
                        principalTable: "NguoiDungs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TinNhans_NguoiDungs_MaNguoiNhan",
                        column: x => x.MaNguoiNhan,
                        principalTable: "NguoiDungs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SanPhams",
                columns: table => new
                {
                    MaSanPham = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenSanPham = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GiaCu = table.Column<double>(type: "float", nullable: false),
                    GiaNhap = table.Column<double>(type: "float", nullable: false),
                    GiaHienTai = table.Column<double>(type: "float", nullable: false),
                    HinhDaiDien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HinhPhongTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaDanhMuc = table.Column<int>(type: "int", nullable: true),
                    MaNhanHieu = table.Column<int>(type: "int", nullable: true),
                    MaNhaSanXuat = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThaiXoa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhams", x => x.MaSanPham);
                    table.ForeignKey(
                        name: "FK_SanPhams_DanhMucs_MaDanhMuc",
                        column: x => x.MaDanhMuc,
                        principalTable: "DanhMucs",
                        principalColumn: "MaDanhMuc");
                    table.ForeignKey(
                        name: "FK_SanPhams_NhaSanXuats_MaNhaSanXuat",
                        column: x => x.MaNhaSanXuat,
                        principalTable: "NhaSanXuats",
                        principalColumn: "MaNhaSX",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SanPhams_NhanHieus_MaNhanHieu",
                        column: x => x.MaNhanHieu,
                        principalTable: "NhanHieus",
                        principalColumn: "MaNhanHieu");
                });

            migrationBuilder.CreateTable(
                name: "NguoiDungVaiTros",
                columns: table => new
                {
                    MaNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaVaiTro = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDungVaiTros", x => new { x.MaNguoiDung, x.MaVaiTro });
                    table.ForeignKey(
                        name: "FK_NguoiDungVaiTros_NguoiDungs_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NguoiDungVaiTros_VaiTros_MaVaiTro",
                        column: x => x.MaVaiTro,
                        principalTable: "VaiTros",
                        principalColumn: "MaVaiTro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonHangs",
                columns: table => new
                {
                    MaDonHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TienKhuyenMai = table.Column<double>(type: "float", nullable: false),
                    TongTienSauKhuyenMai = table.Column<double>(type: "float", nullable: false),
                    TongTienTruocKhuyenMai = table.Column<double>(type: "float", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThaiXoa = table.Column<bool>(type: "bit", nullable: false),
                    MaNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaThanhToan = table.Column<int>(type: "int", nullable: false),
                    MaDiaChiGiaoHang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHangs", x => x.MaDonHang);
                    table.ForeignKey(
                        name: "FK_DonHangs_DiaChiGiaoHangs_MaDiaChiGiaoHang",
                        column: x => x.MaDiaChiGiaoHang,
                        principalTable: "DiaChiGiaoHangs",
                        principalColumn: "MaDCGH",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonHangs_NguoiDungs_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DonHangs_ThanhToans_MaThanhToan",
                        column: x => x.MaThanhToan,
                        principalTable: "ThanhToans",
                        principalColumn: "MaThanhToan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HinhAnhTinNhans",
                columns: table => new
                {
                    MaTinNhanHinhAnh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DuongDanAnh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaTinNhan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhAnhTinNhans", x => x.MaTinNhanHinhAnh);
                    table.ForeignKey(
                        name: "FK_HinhAnhTinNhans_TinNhans_MaTinNhan",
                        column: x => x.MaTinNhan,
                        principalTable: "TinNhans",
                        principalColumn: "MaTinNhan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NhomChats",
                columns: table => new
                {
                    MaNhomChat = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaTinNhan = table.Column<int>(type: "int", nullable: false),
                    SoTinNhanChuaDoc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhomChats", x => x.MaNhomChat);
                    table.ForeignKey(
                        name: "FK_NhomChats_TinNhans_MaTinNhan",
                        column: x => x.MaTinNhan,
                        principalTable: "TinNhans",
                        principalColumn: "MaTinNhan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BienTheSanPhams",
                columns: table => new
                {
                    MaBienTheSanPham = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoLuongTonKho = table.Column<int>(type: "int", nullable: false),
                    MaKichThuoc = table.Column<int>(type: "int", nullable: false),
                    MaMauSac = table.Column<int>(type: "int", nullable: false),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    DuongDanAnh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThaiXoa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BienTheSanPhams", x => x.MaBienTheSanPham);
                    table.ForeignKey(
                        name: "FK_BienTheSanPhams_KichThuocs_MaKichThuoc",
                        column: x => x.MaKichThuoc,
                        principalTable: "KichThuocs",
                        principalColumn: "MaKichThuoc",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BienTheSanPhams_MauSacs_MaMauSac",
                        column: x => x.MaMauSac,
                        principalTable: "MauSacs",
                        principalColumn: "MaMauSac",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BienTheSanPhams_SanPhams_MaSanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DanhGiaSanPhams",
                columns: table => new
                {
                    MaDanhGiaSP = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoSaoDanhGia = table.Column<int>(type: "int", nullable: false),
                    MaNguoiDanhGia = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGiaSanPhams", x => x.MaDanhGiaSP);
                    table.ForeignKey(
                        name: "FK_DanhGiaSanPhams_NguoiDungs_MaNguoiDanhGia",
                        column: x => x.MaNguoiDanhGia,
                        principalTable: "NguoiDungs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DanhGiaSanPhams_SanPhams_MaSanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DanhSachYeuThichSanPham",
                columns: table => new
                {
                    DanhSachSanPhamMaSanPham = table.Column<int>(type: "int", nullable: false),
                    DanhSachYeuThichMaDanhSachYeuThich = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhSachYeuThichSanPham", x => new { x.DanhSachSanPhamMaSanPham, x.DanhSachYeuThichMaDanhSachYeuThich });
                    table.ForeignKey(
                        name: "FK_DanhSachYeuThichSanPham_DanhSachYeuThichs_DanhSachYeuThichMaDanhSachYeuThich",
                        column: x => x.DanhSachYeuThichMaDanhSachYeuThich,
                        principalTable: "DanhSachYeuThichs",
                        principalColumn: "MaDanhSachYeuThich",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DanhSachYeuThichSanPham_SanPhams_DanhSachSanPhamMaSanPham",
                        column: x => x.DanhSachSanPhamMaSanPham,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HinhAnhSanPhams",
                columns: table => new
                {
                    MaHinhAnhSanPham = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DuongDanAnh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaSanPham = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhAnhSanPhams", x => x.MaHinhAnhSanPham);
                    table.ForeignKey(
                        name: "FK_HinhAnhSanPhams_SanPhams_MaSanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham");
                });

            migrationBuilder.CreateTable(
                name: "SanPhamKhuyenMais",
                columns: table => new
                {
                    MaSanPhamKhuyenMai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKhuyenMai = table.Column<int>(type: "int", nullable: false),
                    MaSanPham = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhamKhuyenMais", x => x.MaSanPhamKhuyenMai);
                    table.ForeignKey(
                        name: "FK_SanPhamKhuyenMais_KhuyenMais_MaKhuyenMai",
                        column: x => x.MaKhuyenMai,
                        principalTable: "KhuyenMais",
                        principalColumn: "MaKhuyenMai",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SanPhamKhuyenMais_SanPhams_MaSanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPhams",
                        principalColumn: "MaSanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LichSuDonHangs",
                columns: table => new
                {
                    MaLichSuDonHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThoiGianChinhSua = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaDonHang = table.Column<int>(type: "int", nullable: false),
                    MaNhanVien = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuDonHangs", x => x.MaLichSuDonHang);
                    table.ForeignKey(
                        name: "FK_LichSuDonHangs_DonHangs_MaDonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHangs",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LichSuDonHangs_NguoiDungs_MaNhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NguoiDungs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHangs",
                columns: table => new
                {
                    MaCTDH = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonGia = table.Column<double>(type: "float", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    TienKhuyenMai = table.Column<double>(type: "float", nullable: false),
                    ThanhTienTruocKhuyenMai = table.Column<double>(type: "float", nullable: false),
                    ThanhTienSauKhuyenMai = table.Column<double>(type: "float", nullable: false),
                    MaDonHang = table.Column<int>(type: "int", nullable: false),
                    MaBienTheSP = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHangs", x => x.MaCTDH);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_BienTheSanPhams_MaBienTheSP",
                        column: x => x.MaBienTheSP,
                        principalTable: "BienTheSanPhams",
                        principalColumn: "MaBienTheSanPham",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_DonHangs_MaDonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHangs",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HinhAnhBienTheSanPhams",
                columns: table => new
                {
                    MaHinhAnhBienTheSanPham = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DuongDanAnh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaBienTheSanPham = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhAnhBienTheSanPhams", x => x.MaHinhAnhBienTheSanPham);
                    table.ForeignKey(
                        name: "FK_HinhAnhBienTheSanPhams_BienTheSanPhams_MaBienTheSanPham",
                        column: x => x.MaBienTheSanPham,
                        principalTable: "BienTheSanPhams",
                        principalColumn: "MaBienTheSanPham");
                });

            migrationBuilder.CreateTable(
                name: "DanhGiaSanPhamNguoiDung",
                columns: table => new
                {
                    DanhSachDanhGiaYeuThichMaDanhGiaSP = table.Column<int>(type: "int", nullable: false),
                    DanhSachNguoiYeuThichId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGiaSanPhamNguoiDung", x => new { x.DanhSachDanhGiaYeuThichMaDanhGiaSP, x.DanhSachNguoiYeuThichId });
                    table.ForeignKey(
                        name: "FK_DanhGiaSanPhamNguoiDung_DanhGiaSanPhams_DanhSachDanhGiaYeuThichMaDanhGiaSP",
                        column: x => x.DanhSachDanhGiaYeuThichMaDanhGiaSP,
                        principalTable: "DanhGiaSanPhams",
                        principalColumn: "MaDanhGiaSP",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DanhGiaSanPhamNguoiDung_NguoiDungs_DanhSachNguoiYeuThichId",
                        column: x => x.DanhSachNguoiYeuThichId,
                        principalTable: "NguoiDungs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HienThiDanhGias",
                columns: table => new
                {
                    MaHienThiDanhGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDanhGiaSanPham = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HienThiDanhGias", x => x.MaHienThiDanhGia);
                    table.ForeignKey(
                        name: "FK_HienThiDanhGias_DanhGiaSanPhams_MaDanhGiaSanPham",
                        column: x => x.MaDanhGiaSanPham,
                        principalTable: "DanhGiaSanPhams",
                        principalColumn: "MaDanhGiaSP",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "NguoiDungs",
                columns: new[] { "Id", "SoLanDangNhapThatBai", "MaDongBo", "Email", "XacThucEmail", "HinhAnhBia", "HinhDaiDien", "HoVaTen", "ChoPhepKhoa", "ThoiGianKhoa", "NgayTao", "EmailChuanHoa", "TenDangNhapChuanHoa", "MatKhauMaHoa", "SoDienThoai", "XacThucSoDienThoai", "MaBaoMat", "ThoiGianHoatDongGanDay", "TrangThaiHoatDong", "TrangThaiKhoa", "KichHoat2Lop", "TenDangNhap" },
                values: new object[] { "6da79932-5ac6-4975-9298-cdd60c15b17e", 0, "a7237b3c-2a4c-4631-b954-868ac1ecd4bb", "admin@gmail.com", true, null, null, "Thanh Nương", false, null, new DateTime(2025, 5, 17, 8, 46, 26, 446, DateTimeKind.Local).AddTicks(6514), "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAEFi3neF5YSQnPqnjXb2QOn+1TqINqJJpRzcPW3auUsdeg3/M7MOtlwjLEqsJnET3WA==", "0123456789", false, "690311ec-5933-4985-aeb4-786cfaf95a04", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, false, "admin" });

            migrationBuilder.InsertData(
                table: "VaiTros",
                columns: new[] { "MaVaiTro", "MaDongBo", "TenVaiTro", "TenVaiTroChuanHoa" },
                values: new object[,]
                {
                    { "c958449b-6abc-48cc-8f13-39a9d4ec9ad5", "1", "EMPLOYEE", "EMPLOYEE" },
                    { "de03b0e4-d35e-4312-9c38-e1b125aa4926", "1", "ADMIN", "ADMIN" },
                    { "ff183039-1c37-4e90-9cf7-be8724dd7544", "1", "CUSTOMER", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "NguoiDungVaiTros",
                columns: new[] { "MaVaiTro", "MaNguoiDung" },
                values: new object[] { "de03b0e4-d35e-4312-9c38-e1b125aa4926", "6da79932-5ac6-4975-9298-cdd60c15b17e" });

            migrationBuilder.CreateIndex(
                name: "IX_BaiViets_MaTacGia",
                table: "BaiViets",
                column: "MaTacGia");

            migrationBuilder.CreateIndex(
                name: "IX_BienTheSanPhams_MaKichThuoc",
                table: "BienTheSanPhams",
                column: "MaKichThuoc");

            migrationBuilder.CreateIndex(
                name: "IX_BienTheSanPhams_MaMauSac",
                table: "BienTheSanPhams",
                column: "MaMauSac");

            migrationBuilder.CreateIndex(
                name: "IX_BienTheSanPhams_MaSanPham",
                table: "BienTheSanPhams",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_MaBienTheSP",
                table: "ChiTietDonHangs",
                column: "MaBienTheSP");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_MaDonHang",
                table: "ChiTietDonHangs",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaSanPhamNguoiDung_DanhSachNguoiYeuThichId",
                table: "DanhGiaSanPhamNguoiDung",
                column: "DanhSachNguoiYeuThichId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaSanPhams_MaNguoiDanhGia",
                table: "DanhGiaSanPhams",
                column: "MaNguoiDanhGia");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaSanPhams_MaSanPham",
                table: "DanhGiaSanPhams",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_DanhMucs_DanhMucChaMaDanhMuc",
                table: "DanhMucs",
                column: "DanhMucChaMaDanhMuc");

            migrationBuilder.CreateIndex(
                name: "IX_DanhSachYeuThichs_MaNguoiDung",
                table: "DanhSachYeuThichs",
                column: "MaNguoiDung",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DanhSachYeuThichSanPham_DanhSachYeuThichMaDanhSachYeuThich",
                table: "DanhSachYeuThichSanPham",
                column: "DanhSachYeuThichMaDanhSachYeuThich");

            migrationBuilder.CreateIndex(
                name: "IX_DiaChiGiaoHangs_MaNguoiDung",
                table: "DiaChiGiaoHangs",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_MaDiaChiGiaoHang",
                table: "DonHangs",
                column: "MaDiaChiGiaoHang");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_MaNguoiDung",
                table: "DonHangs",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_MaThanhToan",
                table: "DonHangs",
                column: "MaThanhToan",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HienThiDanhGias_MaDanhGiaSanPham",
                table: "HienThiDanhGias",
                column: "MaDanhGiaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_HinhAnhBienTheSanPhams_MaBienTheSanPham",
                table: "HinhAnhBienTheSanPhams",
                column: "MaBienTheSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_HinhAnhSanPhams_MaSanPham",
                table: "HinhAnhSanPhams",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_HinhAnhTinNhans_MaTinNhan",
                table: "HinhAnhTinNhans",
                column: "MaTinNhan");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuDonHangs_MaDonHang",
                table: "LichSuDonHangs",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuDonHangs_MaNhanVien",
                table: "LichSuDonHangs",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "NguoiDungs",
                column: "EmailChuanHoa");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "NguoiDungs",
                column: "TenDangNhapChuanHoa",
                unique: true,
                filter: "[TenDangNhapChuanHoa] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDungVaiTros_MaVaiTro",
                table: "NguoiDungVaiTros",
                column: "MaVaiTro");

            migrationBuilder.CreateIndex(
                name: "IX_NhomChats_MaTinNhan",
                table: "NhomChats",
                column: "MaTinNhan",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamKhuyenMais_MaKhuyenMai",
                table: "SanPhamKhuyenMais",
                column: "MaKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamKhuyenMais_MaSanPham",
                table: "SanPhamKhuyenMais",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_MaDanhMuc",
                table: "SanPhams",
                column: "MaDanhMuc");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_MaNhanHieu",
                table: "SanPhams",
                column: "MaNhanHieu");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhams_MaNhaSanXuat",
                table: "SanPhams",
                column: "MaNhaSanXuat");

            migrationBuilder.CreateIndex(
                name: "IX_ThietBiThongBaos_MaNguoiDung",
                table: "ThietBiThongBaos",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaos_MaNguoiNhan",
                table: "ThongBaos",
                column: "MaNguoiNhan");

            migrationBuilder.CreateIndex(
                name: "IX_TinNhans_MaNguoiGui",
                table: "TinNhans",
                column: "MaNguoiGui");

            migrationBuilder.CreateIndex(
                name: "IX_TinNhans_MaNguoiNhan",
                table: "TinNhans",
                column: "MaNguoiNhan");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "VaiTros",
                column: "TenVaiTroChuanHoa",
                unique: true,
                filter: "[TenVaiTroChuanHoa] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaiViets");

            migrationBuilder.DropTable(
                name: "ChiTietDonHangs");

            migrationBuilder.DropTable(
                name: "DanhGiaSanPhamNguoiDung");

            migrationBuilder.DropTable(
                name: "DanhSachYeuThichSanPham");

            migrationBuilder.DropTable(
                name: "HienThiBanners");

            migrationBuilder.DropTable(
                name: "HienThiDanhGias");

            migrationBuilder.DropTable(
                name: "HinhAnhBienTheSanPhams");

            migrationBuilder.DropTable(
                name: "HinhAnhSanPhams");

            migrationBuilder.DropTable(
                name: "HinhAnhTinNhans");

            migrationBuilder.DropTable(
                name: "LichSuDonHangs");

            migrationBuilder.DropTable(
                name: "NguoiDungVaiTros");

            migrationBuilder.DropTable(
                name: "NhaCungCaps");

            migrationBuilder.DropTable(
                name: "NhomChats");

            migrationBuilder.DropTable(
                name: "SanPhamKhuyenMais");

            migrationBuilder.DropTable(
                name: "ThietBiThongBaos");

            migrationBuilder.DropTable(
                name: "ThongBaos");

            migrationBuilder.DropTable(
                name: "DanhSachYeuThichs");

            migrationBuilder.DropTable(
                name: "DanhGiaSanPhams");

            migrationBuilder.DropTable(
                name: "BienTheSanPhams");

            migrationBuilder.DropTable(
                name: "DonHangs");

            migrationBuilder.DropTable(
                name: "VaiTros");

            migrationBuilder.DropTable(
                name: "TinNhans");

            migrationBuilder.DropTable(
                name: "KhuyenMais");

            migrationBuilder.DropTable(
                name: "KichThuocs");

            migrationBuilder.DropTable(
                name: "MauSacs");

            migrationBuilder.DropTable(
                name: "SanPhams");

            migrationBuilder.DropTable(
                name: "DiaChiGiaoHangs");

            migrationBuilder.DropTable(
                name: "ThanhToans");

            migrationBuilder.DropTable(
                name: "DanhMucs");

            migrationBuilder.DropTable(
                name: "NhaSanXuats");

            migrationBuilder.DropTable(
                name: "NhanHieus");

            migrationBuilder.DropTable(
                name: "NguoiDungs");
        }
    }
}
