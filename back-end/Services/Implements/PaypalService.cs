using back_end.Core.Constants;
using back_end.Core.Models;
using back_end.Core.Requests;
using back_end.Core.Responses;
using back_end.Core.Responses.Resources;
using back_end.Data;
using back_end.Exceptions;
using back_end.Helpers;
using back_end.Mappers;
using back_end.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace back_end.Services.Implements
{
    public class PaypalService : IPaypalService
    {
        private readonly PaypalClient _paypalClient;    
        private readonly MyStoreDbContext dbContext;
        private readonly ApplicationMapper applicationMapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaypalService(PaypalClient paypalClient, MyStoreDbContext storeContext, ApplicationMapper applicationMapper, IHttpContextAccessor httpContextAccessor)
        {
            _paypalClient = paypalClient;
            dbContext = storeContext;
            this.applicationMapper = applicationMapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse> CaptureOrder(string orderId, OrderRequest request)
        {
            try
            {
                var paypalResponse = await _paypalClient.CaptureOrder(orderId);
                var reference = paypalResponse.purchase_units[0].reference_id;

                DonHang order = await CreateOrder(request);
                order.ThanhToan.MaGiaoDich = reference;
                var savedOrder = await dbContext.DonHangs.AddAsync(order);
                await dbContext.SaveChangesAsync();

                var response = new DataResponse<DonHangResource>();
                response.Message = "Đặt hàng thành công";
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.Success = true;
                response.Data = applicationMapper.MapToOrderResource(savedOrder.Entity);

                return response;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<DonHang> CreateOrder(OrderRequest request)
        {
            DiaChiGiaoHang? addressOrder = await dbContext.DiaChiGiaoHangs
                .SingleOrDefaultAsync(ad => ad.MaDCGH == request.AddressOrderId)
                    ?? throw new NotFoundException("Địa chỉ giao hàng không tồn tại");

            DonHang order = new DonHang();
            order.GhiChu = request.Note;
            order.NgayTao = DateTime.Now;
            order.TrangThai = OrderStatus.CONFIRMED;
            order.DanhSachChiTietDonHang = new List<ChiTietDonHang>();

            order.LichSuDonHang.Add(new LichSuDonHang
            {
                TrangThai = OrderStatus.CONFIRMED,
                ThoiGianChinhSua = DateTime.Now,
                GhiChu = "Bạn đã thanh toán đơn hàng qua Paypal",
            });

            order.MaNguoiDung = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
            order.MaDiaChiGiaoHang = request.AddressOrderId;
            order.ThanhToan = new ThanhToan()
            {
                NgayTao = DateTime.Now,
                PhuongThucThanhToan = "Paypal",
                ThoiGianThanhToan = DateTime.Now,
                TrangThai = false
            };

            double total = 0;

            foreach (var item in request.Items)
            {
                BienTheSanPham? productVariant = await dbContext.BienTheSanPhams
                    .Include(p => p.SanPham)
                    .SingleOrDefaultAsync(p => p.MaBienTheSanPham == item.VariantId)
                        ?? throw new NotFoundException("Không tìm thấy sản phẩm");

                ChiTietDonHang orderItem = new ChiTietDonHang();
                orderItem.MaBienTheSP = item.VariantId;
                orderItem.SoLuong = item.Quantity;
                orderItem.DonGia = productVariant!.SanPham!.GiaHienTai;

                double subTotal = item.Quantity * productVariant.SanPham.GiaHienTai;
                orderItem.ThanhTien = subTotal;
                total += subTotal;
                order.DanhSachChiTietDonHang.Add(orderItem);
            }

            order.TongTien = total;
            order.SoLuong = request.Items.Count;

            return order;
        }
    }
}
