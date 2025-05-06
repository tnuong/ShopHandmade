using back_end.Core.Constants;
using back_end.Core.Models;
using back_end.Core.Responses.Report;
using back_end.Core.Responses.Resources;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace back_end.Mappers
{
    public class ApplicationMapper
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public ApplicationMapper(UserManager<NguoiDung> userManager, IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }
        public DanhMucResource MapToCategoryResource(DanhMuc category)
        {
            return new DanhMucResource()
            {
                Id = category.MaDanhMuc,
                Name = category.TenDanhMuc,
                Description = category.MoTa,
                ParentCategory = category.DanhMucCha != null ? MapToCategoryResource(category.DanhMucCha) : null
            };
        }

        public NhaSanXuatResource MapToManufacturerResource(NhaSanXuat manufacturer)
        {
            return new NhaSanXuatResource()
            {
                Id = manufacturer.MaNhaSX,
                Name = manufacturer.TenNhaSX,
                Description = manufacturer.MoTa,
                Address = manufacturer.DiaChi,
                PhoneNumber = manufacturer.SoDienThoai,
                Email = manufacturer.Email
            };
        }

        public NhaCungCapResource MapToSupplierResource(NhaCungCap supplier)
        {
            return new NhaCungCapResource()
            {
                Id = supplier.MaNhaCungCap,
                Name = supplier.TenNhaCungCap,
                Status = supplier.TrangThai,
                Address = supplier.DiaChi,
                PhoneNumber = supplier.SoDienThoai,
                Email = supplier.Email
            };
        }
        public HienThiDanhGiaResource MapToReviewShowResource(HienThiDanhGia reviewShow)
        {
            return new HienThiDanhGiaResource()
            {
                Evaluation = MapToEvaluationResourceWithoutPrincipal(reviewShow.DanhGiaSanPham),
                Id = reviewShow.MaHienThiDanhGia,
            };
        }

        public PhanCapDanhMucResource MapToCategoryLevelResource(DanhMuc category)
        {
            return new PhanCapDanhMucResource()
            {
                Id = category.MaDanhMuc,
                Name = category.TenDanhMuc,
                Description = category.MoTa,
                CategoryChildren = category.DanhSachDanhMucCon != null ? category.DanhSachDanhMucCon.Select(c => MapToCategoryLevelResource(c)).ToList() : null
            };
        }

        public SanPhamResource MapToProductResource(SanPham product)
        {
            return new SanPhamResource()
            {
                Id = product.MaSanPham,
                Name = product.TenSanPham,
                Description = product.MoTa,
                OldPrice = product.GiaCu,
                Quantity = product?.DanhSachBienTheSP != null ? product.DanhSachBienTheSP.Count : 0,
                Price = product.GiaHienTai,
                PurchasePrice = product.GiaNhap,
                ZoomImage = product.HinhPhongTo,
                Thumbnail = product.HinhDaiDien,
                Images = product.DanhSachHinhAnh != null ? product.DanhSachHinhAnh.Select(MapToProductImageResource).ToList() : null,
                Category = product.DanhMuc != null ? MapToCategoryResource(product.DanhMuc) : null,
                Brand = product.NhanHieu != null ? MapToBrandResource(product.NhanHieu) : null,
                Manufacturer = product.NhaSanXuat != null ? MapToManufacturerResource(product.NhaSanXuat) : null
            };
        }

        public HienThiBannerResource MapToBannerResource(HienThiBanner banner)
        {
            return new HienThiBannerResource()
            {
                Id = banner.MaHienThiBanner,
                BackgroundImage = banner.DuongDanhAnhNen,
                BtnTitle = banner.NutHanhDong,
                Description = banner.MoTaNgan,
                Title = banner.TieuDe,
            };
        }

        public ProductReport MapToProductReport(SanPham product, int quantity)
        {
            return new ProductReport
            {
                Product = MapToProductResource(product),
                Quantity = quantity
            };
        }

        public BienTheSanPhamResource MapToVariantResource(BienTheSanPham variant)
        {
            return new BienTheSanPhamResource()
            {
                Id = variant.MaBienTheSanPham,
                InStock = variant.SoLuongTonKho,
                ThumbnailUrl = variant.DuongDanAnh,
                Color = variant.MauSac != null ? MapToColorResource(variant.MauSac) : null,
                Size = variant.KichThuoc != null ? MapToSizeResource(variant.KichThuoc) : null,
                Product = variant.SanPham != null ? MapToProductResource(variant.SanPham) : null,
                Images = variant.DanhSachHinhAnh != null ? variant.DanhSachHinhAnh.Select(MapToProductImageVariantResource).ToList() : null
            };
        }

        public ThuongHieuResource MapToBrandResource(NhanHieu brand) {
            return new ThuongHieuResource()
            {
                Id = brand.MaNhanHieu,
                Name = brand.TenNhanHieu,
                Description = brand.MoTa,
            };
        }

        public BaiVietResource MapToBlogResource(BaiViet blog)
        {
            return new BaiVietResource()
            {
                Id = blog.MaBaiViet,
                Title = blog.TieuDe,
                TextPlain = blog.VanBanTho,
                IsHidden = blog.TrangThaiAn,
                Content = blog.NoiDung,
                CreatedDate = blog.NgayTao,
                Thumbnail = blog.HinhDaiDien,
                User = blog.TacGia != null ? MapToUserResourceWithoutRoles(blog.TacGia) : null,
            };
        }

        public async Task<NguoiDungResource> MapToUserResource(NguoiDung user)
        {
            IList<string> roles = await _userManager.GetRolesAsync(user);
            return new NguoiDungResource()
            {
                Id = user.Id,
                Name = user.HoVaTen,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Username = user.UserName,
                Avatar = user.HinhDaiDien,
                CoverImage = user.HinhAnhBia,
                Roles = roles.ToList(),
                IsOnline = user.TrangThaiHoatDong,
                IsLocked = user.TrangThaiKhoa,
                RecentOnlineTime = user.ThoiGianHoatDongGanDay,
            };
        }

        public KhuyenMaiResource MapToKhuyenMai(KhuyenMai khuyenMai)
        {
            return new KhuyenMaiResource()
            {
                Id = khuyenMai.MaKhuyenMai,
                Name = khuyenMai.TenKhuyenMai,
                Description = khuyenMai.NoiDungKhuyenMai,
                DiscountValue = khuyenMai.GiaTriGiam,
                FromDate = khuyenMai.NgayBatDau,
                PromotionType = khuyenMai.LoaiKhuyenMai,
                Status = khuyenMai.TrangThai,
                ToDate = khuyenMai.NgayKetThuc,
            };
        }

        public async Task<LienHeNguoiDungResource> MapToUserContactResource(NguoiDung user)
        {
            return new LienHeNguoiDungResource()
            {
                User = await MapToUserResource(user),
            };
        }


        public NguoiDungResource MapToUserResourceWithoutRoles(NguoiDung user)
        {
            return new NguoiDungResource()
            {
                Id = user.Id,
                Name = user.HoVaTen,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Username = user.UserName,
                Avatar = user.HinhDaiDien,
                CoverImage = user.HinhAnhBia,
                IsLocked = user.TrangThaiKhoa,
                IsOnline = user.TrangThaiHoatDong,
                RecentOnlineTime = user.ThoiGianHoatDongGanDay,
            };
        }

        public HinhAnhSanPhamResource MapToProductImageResource(HinhAnhSanPham image)
        {
            return new HinhAnhSanPhamResource()
            {
                Id = image.MaHinhAnhSanPham,
                Url = image.DuongDanAnh,
            };
        }

        public HinhAnhSanPhamResource MapToProductImageVariantResource(HinhAnhBienTheSanPham image)
        {
            return new HinhAnhSanPhamResource()
            {
                Id = image.MaHinhAnhBienTheSanPham,
                Url = image.DuongDanAnh,
            };
        }

        public MauSacResource MapToColorResource(MauSac color) { return new MauSacResource() { Id = color.MaMauSac, Name = color.TenMauSac, HexCode = color.MaThapLucPhan }; }

        public KichThuocResource MapToSizeResource(KichThuoc size)
        {
            return new KichThuocResource()
            {
                Id = size.MaKichThuoc,
                ESize = size.TenKichThuoc,
                Description = size.MoTa,
              
            };
        }

        public List<OrderProcessItem> GetOrderProcess(List<LichSuDonHang> histories)
        {

            var result = new List<OrderProcessItem>();

            void AddOrderProcessItem(string status)
            {
                if (!result.Any(item => item.OrderStatus.Equals(status)))
                {
                    result.Add(new OrderProcessItem
                    {
                        OrderStatus = status,
                        IsCompleted = false
                    });
                }
            }

            foreach (var hist in histories)
            {
                result.Add(new OrderProcessItem
                {
                    OrderStatus = hist.TrangThai,
                    ModifyAt = hist.ThoiGianChinhSua,
                    Note = hist.GhiChu,
                    IsCompleted = true,
                });
            }

            var requiredSteps = new List<string>
            {
                OrderStatus.CONFIRMED,
                OrderStatus.DELIVERING,
                OrderStatus.DELIVERED,
                OrderStatus.COMPLETED
            };

            requiredSteps = requiredSteps.SkipWhile(status => result.Any(item => item.OrderStatus.Equals(status))).ToList();

            foreach (var status in requiredSteps)
            {
                AddOrderProcessItem(status);
            }

            return result;
        }

        public DonHangResource MapToOrderResource(DonHang order)
        {
            var orderItems = order.DanhSachChiTietDonHang.ToList();
            var thumbnailUrl = orderItems[0].BienTheSanPham.DuongDanAnh;

            var title = orderItems.Count switch
            {
                0 => string.Empty,
                1 => orderItems[0].BienTheSanPham?.SanPham?.TenSanPham,
                2 => $"{orderItems[0].BienTheSanPham?.SanPham?.TenSanPham} | {orderItems[1].BienTheSanPham?.SanPham?.TenSanPham}",
                _ => $"{orderItems[0].BienTheSanPham?.SanPham?.TenSanPham} | {orderItems[1].BienTheSanPham?.SanPham?.TenSanPham} và {orderItems.Count - 2} sản phẩm khác"
            };

            return new DonHangResource()
            {
                Id = order.MaDonHang,
                CreatedAt = order.NgayTao,
                Note = order.GhiChu,
                TotalPriceAfterDiscount = order.TongTienSauKhuyenMai,
                TotalPriceBeforeDiscount = order.TongTienTruocKhuyenMai,
                TotalDiscount = order.TienKhuyenMai,
                Title = title,
                OrderStatus = order.TrangThai,
                ThumbnailUrl = thumbnailUrl,
                Quantity = order.SoLuong,
                OrderSteps = order.LichSuDonHang != null ? GetOrderProcess(order.LichSuDonHang) : new List<OrderProcessItem>(),
                Items = order.DanhSachChiTietDonHang != null ? order.DanhSachChiTietDonHang.Select(MapToOrderItemResource).ToList() : new List<OrderItemResource>(),
                Payment = order.ThanhToan != null ? MapToPaymentResource(order.ThanhToan) : null,
                AddressOrder = order.DiaChiGiaoHang != null ? MapToAddressOrderResource(order.DiaChiGiaoHang) : null,
                User = order.NguoiDung != null ? MapToUserResourceWithoutRoles(order.NguoiDung) : null
            };
        }

        public TinNhanResource MapToMessageResource(TinNhan message)
        {
            return new TinNhanResource()
            {
                Id = message.MaTinNhan,
                Content = message.NoiDung,
                SentAt = message.ThoiGianGui,
                Sender = message.NguoiGui != null ? MapToUserResourceWithoutRoles(message.NguoiGui) : null,
                Recipient = message.NguoiNhan != null ? MapToUserResourceWithoutRoles(message.NguoiNhan) : null,
                Images = message.DanhSachHinhAnh != null ? message.DanhSachHinhAnh.Select(m => m.DuongDanAnh).ToList() : new List<string>(),
            };
        }

        public OrderHistoryResource MapToOrderHistoryResource(LichSuDonHang orderHistory)
        {
            return new OrderHistoryResource()
            {
                Id = orderHistory.MaLichSuDonHang,
                ModifyAt = orderHistory.ThoiGianChinhSua,
                OrderStatus = orderHistory.TrangThai,
                Note = orderHistory.GhiChu,
            };
        }

        public PaymentResource MapToPaymentResource(ThanhToan payment)
        {
            return new PaymentResource()
            {
                CreatedDate = payment.NgayTao,
                PaymentCode = payment.MaGiaoDich,
                PaymentMethod = payment.PhuongThucThanhToan,
                Status = payment.TrangThai,
            };
        }

        public AddressOrderResource MapToAddressOrderResource(DiaChiGiaoHang addressOrder)
        {
            return new AddressOrderResource()
            {
                Id = addressOrder.MaDCGH,
                Address = addressOrder.DiaChi,
                Email = addressOrder.Email,
                FullName = addressOrder.HoVaTen,
                IsDefault = addressOrder.MacDinh,
                PhoneNumber = addressOrder.SoDienThoai,
            };
        }

        public OrderItemResource MapToOrderItemResource(ChiTietDonHang item)
        {
            return new OrderItemResource()
            {
                Id = item?.MaCTDH,
                Price = item?.DonGia,
                SubTotalAfterDiscount = item.ThanhTienSauKhuyenMai,
                SubTotalBeforeDiscount = item.ThanhTienTruocKhuyenMai,
                SubTotalDiscount = item.TienKhuyenMai,
                Quantity = item?.SoLuong,
                ProductId = item?.BienTheSanPham?.MaSanPham,
                ProductName = item?.BienTheSanPham?.SanPham?.TenSanPham,
                ProductPrice = item?.BienTheSanPham?.SanPham?.GiaHienTai,
                Variant = item?.BienTheSanPham != null ? MapToVariantResource(item.BienTheSanPham) : null,
                
            };
        }

        public DanhGiaResource MapToEvaluationResource(DanhGiaSanPham evaluation, string userId)
        {
            return new DanhGiaResource()
            {
                Id = evaluation.MaDanhGiaSP,
                Content = evaluation.NoiDung,
                CreatedAt = evaluation.NgayTao,
                Stars = evaluation.SoSaoDanhGia,
                Favorites = evaluation.DanhSachNguoiYeuThich != null ? evaluation.DanhSachNguoiYeuThich.Count : 0,
                IsFavoriteIncludeMe = evaluation.DanhSachNguoiYeuThich != null ? evaluation.DanhSachNguoiYeuThich.Any(e => e.Id.Equals(userId)) : false,
                User = evaluation.NguoiDanhGia != null ? MapToUserResourceWithoutRoles(evaluation.NguoiDanhGia) : null
            };
        }

        public DanhGiaResource MapToEvaluationResourceWithoutPrincipal(DanhGiaSanPham evaluation)
        {
            return new DanhGiaResource()
            {
                Id = evaluation.MaDanhGiaSP,
                Content = evaluation.NoiDung,
                CreatedAt = evaluation.NgayTao,
                Stars = evaluation.SoSaoDanhGia,
                Favorites = evaluation.DanhSachNguoiYeuThich != null ? evaluation.DanhSachNguoiYeuThich.Count : 0,
                IsFavoriteIncludeMe = false,
                User = evaluation.NguoiDanhGia != null ? MapToUserResourceWithoutRoles(evaluation.NguoiDanhGia) : null
            };
        }

        public ThongBaoResource MapToNotificationResource(ThongBao notification) {
            return new ThongBaoResource()
            {
                Id = notification.MaThongBao,
                Title = notification.TieuDe,
                Content = notification.NoiDung,
                CreatedAt = notification.NgayTao,
                Recipient = notification.NguoiNhan != null ? MapToUserResourceWithoutRoles(notification.NguoiNhan) : null,
                HaveRead = notification.TrangThaiDoc,
                ReferenceId = notification.MaThamChieu,
                NotificationType = notification.LoaiThongBao,
            };
        }
    }
}
