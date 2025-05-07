using back_end.Core.Constants;
using back_end.Core.Models;
using back_end.Core.Requests;
using back_end.Core.Responses;
using back_end.Data;
using back_end.Exceptions;
using back_end.Helpers;
using back_end.Mappers;
using back_end.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace back_end.Services.Implements
{
    public class VnpayService : IVnpayService
    {
        private readonly IConfiguration _config;
        private readonly MyStoreDbContext dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationMapper _applicationMapper;

        public VnpayService(IConfiguration config, MyStoreDbContext dbContext, IHttpContextAccessor httpContextAccessor, ApplicationMapper applicationMapper)
        {
            _config = config;
            this.dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _applicationMapper = applicationMapper;
        }

        public async Task<BaseResponse> CreatePaymentUrl(HttpContext context, OrderRequest request)
        {
            DonHang order = await createOrder(request);
            var tick = DateTime.Now.Ticks.ToString();

            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", (order.TongTienSauKhuyenMai * 100).ToString()); 

            vnpay.AddRequestData("vnp_CreateDate", order.NgayTao.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);

            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán cho đơn hàng:" + order.MaDonHang);
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", _config["VnPay:PaymentBackReturnUrl"]);

            vnpay.AddRequestData("vnp_TxnRef", order.MaDonHang + "#" + tick); 

            var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecret"]);


            var response = new DataResponse<string>();
            response.Message = "Tạo đơn thanh toán thành công";
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Success = true;
            response.Data = paymentUrl;

            return response;
        }

        private async Task<DonHang> createOrder(OrderRequest request)
        {
            DiaChiGiaoHang? addressOrder = await dbContext.DiaChiGiaoHangs
                .SingleOrDefaultAsync(ad => ad.MaDCGH == request.AddressOrderId)
                    ?? throw new NotFoundException("Địa chỉ giao hàng không tồn tại");

            DonHang order = new DonHang();
            order.GhiChu = request.Note;
            order.NgayTao = DateTime.Now;
            order.TrangThai = OrderStatus.WAITING_PAYMENT;
            order.LichSuDonHang.Add(new LichSuDonHang
            {
                TrangThai = OrderStatus.WAITING_PAYMENT,
                ThoiGianChinhSua = DateTime.Now,
                GhiChu = "Bạn đã đặt một đơn hàng mới",
            });

          
            order.DanhSachChiTietDonHang = new List<ChiTietDonHang>();

            order.MaNguoiDung = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
            order.MaDiaChiGiaoHang = request.AddressOrderId;
            order.ThanhToan = new ThanhToan()
            {
                NgayTao = DateTime.Now,
                PhuongThucThanhToan = "VNPAY",
                MaGiaoDich = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value,
                TrangThai = false
            };

            double totalBeforeDiscount = 0;
            double totalAfterDiscount = 0;
            double totalOrderDiscount = 0.0;
            int totalQuantity = 0;
            order.DanhSachChiTietDonHang = new List<ChiTietDonHang>();

            foreach (var item in request.Items)
            {
                BienTheSanPham? productVariant = await dbContext.BienTheSanPhams
                    .Include(p => p.SanPham)
                    .SingleOrDefaultAsync(p => p.MaBienTheSanPham == item.VariantId)
                        ?? throw new NotFoundException("Không tìm thấy sản phẩm");

                var khuyenMais = await dbContext.SanPhamKhuyenMais
                    .Include(km => km.KhuyenMai)
                    .Where(km => km.MaSanPham == productVariant.MaSanPham).ToListAsync();

                productVariant.SoLuongTonKho -= item.Quantity;

                ChiTietDonHang orderItem = new ChiTietDonHang();
                orderItem.MaBienTheSP = item.VariantId;
                orderItem.SoLuong = item.Quantity;

                double discountValue = 0.0;
                double unitPriceDiscount = productVariant!.SanPham!.GiaHienTai;
                double totalDiscountValue = 0.0;

                foreach (var km in khuyenMais)
                {
                    if (km.KhuyenMai.NgayKetThuc.Date >= DateTime.Now.Date && km.KhuyenMai.TrangThai == PromotionStatus.ACTIVE)
                    {
                        discountValue = km.KhuyenMai.GiaTriGiam;

                        if (km.KhuyenMai.LoaiKhuyenMai == PromotionDiscountType.FIXED_AMOUNT)
                        {
                            unitPriceDiscount = productVariant.SanPham.GiaHienTai - discountValue;
                            totalDiscountValue += discountValue;
                        }
                        else
                        {
                            unitPriceDiscount = productVariant.SanPham.GiaHienTai - (productVariant.SanPham.GiaHienTai * discountValue / 100);
                            totalDiscountValue += (productVariant.SanPham.GiaHienTai * discountValue / 100);
                        }
                    }
                }

                unitPriceDiscount = unitPriceDiscount < 0 ? 0 : unitPriceDiscount;
                totalDiscountValue = totalDiscountValue > productVariant.SanPham.GiaHienTai ? productVariant.SanPham.GiaHienTai : totalDiscountValue;

                orderItem.DonGia = productVariant.SanPham.GiaHienTai;
                var subTotalDiscount = item.Quantity * unitPriceDiscount;

                orderItem.ThanhTienTruocKhuyenMai = item.Quantity * productVariant.SanPham.GiaHienTai;
                orderItem.ThanhTienSauKhuyenMai = subTotalDiscount;
                orderItem.TienKhuyenMai = totalDiscountValue * item.Quantity;

                totalBeforeDiscount += orderItem.ThanhTienTruocKhuyenMai;
                totalAfterDiscount += subTotalDiscount;
                totalOrderDiscount += totalDiscountValue;
                totalQuantity += item.Quantity;
                order.DanhSachChiTietDonHang.Add(orderItem);
            }

            order.TongTienTruocKhuyenMai = totalBeforeDiscount;
            order.TongTienSauKhuyenMai = totalAfterDiscount;
            order.TienKhuyenMai = totalOrderDiscount;
            order.SoLuong = request.Items.Count;

            var savedOrder = await dbContext.DonHangs.AddAsync(order);
            await dbContext.SaveChangesAsync();

            return savedOrder.Entity;
        }

        public async Task<DonHang> PaymentExecute(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var vnp_orderId = vnpay.GetResponseData("vnp_TxnRef");
            var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]);
            if (!checkSignature || vnp_ResponseCode != "00")
                throw new Exception($"Lỗi thanh toán VNPAY {vnp_ResponseCode}");

            var orderId = int.Parse(vnp_orderId.Split("#")[0]);
            var order = await dbContext.DonHangs
                .Include(o => o.ThanhToan)
                .Include(o => o.DiaChiGiaoHang)
                .Include(o => o.NguoiDung)
                .SingleOrDefaultAsync(o => o.MaDonHang == orderId)
                    ?? throw new NotFoundException("Đơn hàng không tồn tại");

            order.ThanhToan.TrangThai = true;
            order.LichSuDonHang.Add(new LichSuDonHang
            {
                TrangThai = OrderStatus.CONFIRMED,
                ThoiGianChinhSua = DateTime.Now,
                GhiChu = "Bạn đã thanh toán đơn hàng qua VNPAY",
            });
            order.TrangThai = OrderStatus.CONFIRMED;
            order.ThanhToan.MaGiaoDich = vnp_TransactionId.ToString();
            order.ThanhToan.PhuongThucThanhToan = "VNPAY";
            order.ThanhToan.ThoiGianThanhToan = DateTime.Now;

            await dbContext.SaveChangesAsync();
            return order;
        }
    }
}
