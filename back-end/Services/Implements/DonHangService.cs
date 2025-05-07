using back_end.Core.Constants;
using back_end.Core.Models;
using back_end.Core.Requests;
using back_end.Core.Responses;
using back_end.Core.Responses.Resources;
using back_end.Data;
using back_end.Exceptions;
using back_end.Extensions;
using back_end.Helpers;
using back_end.Infrastructures.FCM;
using back_end.Infrastructures.SignalR;
using back_end.Mappers;
using back_end.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace back_end.Services.Implements
{
    public class DonHangService : IDonHangService
    {
        private const double UsdPrice = 23500;
        private readonly MyStoreDbContext dbContext;
        private readonly ApplicationMapper applicationMapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly PaypalClient _paypalClient;
        private readonly IConfiguration _configuration;
        private readonly IFcmService fcmService;
        private readonly UserManager<NguoiDung> _userManager;
        private readonly PresenceTracker presenceTracker;
        private readonly IHubContext<ServerHub> hubContext;

        public DonHangService(MyStoreDbContext dbContext, IHttpContextAccessor httpContextAccessor, ApplicationMapper mapper, PaypalClient paypalClient, IConfiguration configuration, IFcmService fcmService, UserManager<NguoiDung> userManager, PresenceTracker presenceTracker, IHubContext<ServerHub> hubContext)
        {
            this.dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            applicationMapper = mapper;
            _paypalClient = paypalClient;
            _configuration = configuration;
            this.fcmService = fcmService;
            _userManager = userManager;
            this.presenceTracker = presenceTracker;
            this.hubContext = hubContext;
        }

        public async Task<BaseResponse> CreateOrder(OrderRequest request)
        {
            DiaChiGiaoHang? addressOrder = await dbContext.DiaChiGiaoHangs
                .SingleOrDefaultAsync(ad => ad.MaDCGH == request.AddressOrderId)
                    ?? throw new NotFoundException("Địa chỉ giao hàng không tồn tại");

            DonHang order = new DonHang();
            order.GhiChu = request.Note;
            order.NgayTao = DateTime.Now;
            order.TrangThai = OrderStatus.PENDING;
            order.MaNguoiDung = _httpContextAccessor.HttpContext.User.GetUserId();
            order.MaDiaChiGiaoHang = request.AddressOrderId;

            order.LichSuDonHang.Add(new LichSuDonHang
            {
                TrangThai = OrderStatus.PENDING,
                ThoiGianChinhSua = DateTime.Now,
                GhiChu = "Bạn đã đặt một đơn hàng",
            });
            
            order.ThanhToan = new ThanhToan()
            {
                NgayTao = DateTime.Now,
                PhuongThucThanhToan = "CASH",
                MaGiaoDich = _httpContextAccessor.HttpContext.User.GetUserId(),
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
                        } else
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
                totalOrderDiscount += totalDiscountValue * item.Quantity;
                totalQuantity += item.Quantity;
                order.DanhSachChiTietDonHang.Add(orderItem);
            }

            order.TongTienTruocKhuyenMai = totalBeforeDiscount;
            order.TongTienSauKhuyenMai = totalAfterDiscount;
            order.TienKhuyenMai = totalOrderDiscount;

            order.SoLuong = totalQuantity;

            var savedOrder = await dbContext.DonHangs.AddAsync(order);
            await dbContext.SaveChangesAsync();

            var fullName = _httpContextAccessor.HttpContext?.User.GetGivenName();

            var adminUsers = await _userManager.GetUsersInRoleAsync("ADMIN");
            
            foreach(var admin in adminUsers)
            {
                await fcmService.SendNotification(
                    "Đơn hàng mới",
                    $"{fullName} đã đặt một đơn hàng mới",
                    admin.Id,
                    order.MaDonHang,
                    NotificationType.ORDER
                );
            }

            var response = new DataResponse<DonHangResource>();
            response.Message = "Đặt hàng thành công";
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Success = true;
            response.Data = applicationMapper.MapToOrderResource(savedOrder.Entity);

            return response;
        }


        public async Task<CreateOrderResponse> CreateOrderWithPaypal(PaypalOrderRequest request)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
              
                var price = Math.Round(request.TotalPrice / UsdPrice, 2).ToString();
                var currency = "USD";
                var reference = "DH" + DateTime.Now.Ticks.ToString();
                var response = await _paypalClient.CreateOrder(price, currency, reference);

                return response;
            }
            catch (Exception e)
            {
                throw new Exception($"Có lỗi xảy ra: {e.Message}");
            }
        }

        public async Task<BaseResponse> GetAllOrdersByUser(int pageIndex, int pageSize, string status)
        {
            var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var totalItems = await dbContext.DonHangs.Where(o => o.MaNguoiDung == userId).CountAsync();
            var queryable = dbContext.DonHangs
                .OrderByDescending(o => o.MaDonHang)
                .Where(o => o.MaNguoiDung == userId)
                .AsQueryable();

            if(status != null && status != "Tất cả")
            {
                queryable = queryable.Where(o => o.TrangThai.Equals(status));
            }

            var orders = await queryable
                .Include(o => o.DiaChiGiaoHang)
                .Include(o => o.ThanhToan)
                .Include(o => o.NguoiDung)
                .Include(o => o.DanhSachChiTietDonHang)
                    .ThenInclude(o => o.BienTheSanPham)
                        .ThenInclude(o => o.SanPham)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            var response = new PaginationResponse<List<DonHangResource>>()
            {
                Data = orders.Select(order => applicationMapper.MapToOrderResource(order)).ToList(),
                Message = "Lấy danh sách đơn hàng thành công",
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Pagination = new Pagination
                {
                    TotalItems = totalItems,
                    TotalPages = (int) Math.Ceiling((double) totalItems / pageSize),
                }
            };

            return response;
        }

        public async Task<BaseResponse> GetAllOrders(int pageIndex, int pageSize, string status, string name)
        {
            
            var queryable = dbContext.DonHangs
                .Include(o => o.NguoiDung)
                .OrderByDescending(o => o.MaDonHang)
                .AsQueryable();

            var totalItems = queryable.Count();


            if (status != null && status != "Tất cả")
            {
                queryable = queryable.Where(o => o.TrangThai.Equals(status));
            }

            if(!string.IsNullOrEmpty(name))
            {
                 queryable = queryable.Where(o => o.NguoiDung.HoVaTen.ToLower().Contains(name.ToLower()));
            }

            var orders = await queryable
                .Include(o => o.DiaChiGiaoHang)
                .Include(o => o.ThanhToan)
                .Include(o => o.DanhSachChiTietDonHang)
                    .ThenInclude(o => o.BienTheSanPham)
                        .ThenInclude(o => o.SanPham)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            var response = new PaginationResponse<List<DonHangResource>>()
            {
                Data = orders.Select(order => applicationMapper.MapToOrderResource(order)).ToList(),
                Message = "Lấy danh sách đơn hàng thành công",
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Pagination = new Pagination
                {
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                }
            };

            return response;
        }

        public async Task<BaseResponse> GetOrderById(int id)
        {
            DonHang? order = await dbContext.DonHangs
                .Include(o => o.DiaChiGiaoHang)
                .Include(o => o.LichSuDonHang)
                .Include(o => o.ThanhToan)
                .Include(o => o.NguoiDung)
                .Include(o => o.DanhSachChiTietDonHang)
                    .ThenInclude(o => o.BienTheSanPham)
                        .ThenInclude(o => o.SanPham)
                .Include(o => o.DanhSachChiTietDonHang)
                    .ThenInclude(o => o.BienTheSanPham)
                        .ThenInclude(o => o.KichThuoc)
                .Include(o => o.DanhSachChiTietDonHang)
                    .ThenInclude(o => o.BienTheSanPham)
                        .ThenInclude(o => o.MauSac)
                .SingleOrDefaultAsync(o => o.MaDonHang == id)
                    ?? throw new NotFoundException("Không tìm thấy đơn hàng nào");


            var response = new DataResponse<DonHangResource>();
            response.Message = "Lấy thông tin đơn hàng thành công";
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Success = true;
            response.Data = applicationMapper.MapToOrderResource(order);

            return response;
        }

        public async Task UpdateStatusOrderToConfirmed(int id)
        {
            DonHang? order = await dbContext.DonHangs
                .Include(o => o.LichSuDonHang)
                .SingleOrDefaultAsync(o => o.MaDonHang == id)
                    ?? throw new NotFoundException("Không tìm thấy đơn hàng nào");


            if (order.TrangThai.Equals(OrderStatus.PENDING))
            {

                var userId = _httpContextAccessor.HttpContext.User.GetUserId();
                order.TrangThai = OrderStatus.CONFIRMED;
                order.LichSuDonHang.Add(new LichSuDonHang
                {
                    MaNhanVien = userId,
                    TrangThai = OrderStatus.CONFIRMED,
                    ThoiGianChinhSua = DateTime.Now,
                    GhiChu = "Đơn hàng đã được tiếp nhận và đang trong quá trình xử lí",
                });
                await dbContext.SaveChangesAsync();
                await fcmService.SendNotification("Thông báo mới", $"Đơn hàng {order.MaDonHang} của bạn đã được tiếp nhận", order.MaNguoiDung, order.MaDonHang, NotificationType.ORDER);
            }
            else throw new Exception("Cập nhật đơn hàng thất bại");
            
        }

        public async Task UpdateStatusOrderToRejected(int id)
        {
            DonHang? order = await dbContext.DonHangs
                .Include(o => o.DanhSachChiTietDonHang)
                    .ThenInclude(o => o.BienTheSanPham)
                .Include(o => o.LichSuDonHang)
                .SingleOrDefaultAsync(o => o.MaDonHang == id)
                    ?? throw new NotFoundException("Không tìm thấy đơn hàng nào");

            if (order.TrangThai.Equals(OrderStatus.PENDING) || order.TrangThai.Equals(OrderStatus.WAITING_PAYMENT))
            {
                var userId = _httpContextAccessor.HttpContext.User.GetUserId();

                order.TrangThai = OrderStatus.REJECTED;
                order.LichSuDonHang.Add(new LichSuDonHang
                {
                    MaNhanVien= userId,
                    TrangThai = OrderStatus.REJECTED,
                    ThoiGianChinhSua = DateTime.Now,
                    GhiChu = "Shop đã từ chối đơn hàng của bạn",
                });

                foreach(var item in order.DanhSachChiTietDonHang)
                {
                    item.BienTheSanPham.SoLuongTonKho += item.SoLuong;
                }

                await dbContext.SaveChangesAsync();
                await fcmService.SendNotification("Thông báo mới", $"Đơn hàng {order.MaDonHang} của bạn đã bị từ chối", order.MaNguoiDung, order.MaDonHang, NotificationType.ORDER);
            }
            else throw new Exception("Cập nhật đơn hàng thất bại");
        }

        public async Task UpdateStatusOrderToDelivering(int id)
        {
            DonHang? order = await dbContext.DonHangs
                .Include(o => o.LichSuDonHang)
                .SingleOrDefaultAsync(o => o.MaDonHang == id)
                    ?? throw new NotFoundException("Không tìm thấy đơn hàng nào");

            if (order.TrangThai.Equals(OrderStatus.CONFIRMED))
            {
                var userId = _httpContextAccessor.HttpContext.User.GetUserId();
                order.TrangThai = OrderStatus.DELIVERING;
                order.LichSuDonHang.Add(new LichSuDonHang
                {
                    MaNhanVien = userId,
                    TrangThai = OrderStatus.DELIVERING,
                    ThoiGianChinhSua = DateTime.Now,
                    GhiChu = "Đơn hàng đang được vận chuyển tới địa chỉ của bạn",
                });
                await dbContext.SaveChangesAsync();
                await fcmService.SendNotification("Thông báo mới", $"Đơn hàng {order.MaDonHang} của bạn đã bắt đầu vận chuyển", order.MaNguoiDung, order.MaDonHang, NotificationType.ORDER);
            }
            else throw new Exception("Cập nhật đơn hàng thất bại");
        }

        public async Task UpdateStatusOrderToDelivered(int id)
        {
            DonHang? order = await dbContext.DonHangs
                .Include(o => o.LichSuDonHang)
                .Include(o => o.ThanhToan)
                .SingleOrDefaultAsync(o => o.MaDonHang == id)
                    ?? throw new NotFoundException("Không tìm thấy đơn hàng nào");
           
            if (order.TrangThai.Equals(OrderStatus.DELIVERING))
            {

                var userId = _httpContextAccessor.HttpContext.User.GetUserId();
                order.TrangThai = OrderStatus.DELIVERED;
                order.ThanhToan.TrangThai = true;
                order.ThanhToan.ThoiGianThanhToan = DateTime.Now;
                order.LichSuDonHang.Add(new LichSuDonHang
                {
                    MaNhanVien=userId,
                    TrangThai = OrderStatus.DELIVERED,
                    ThoiGianChinhSua = DateTime.Now,
                    GhiChu = "Đơn hàng đã được giao tới địa chỉ của bạn",
                });
                await dbContext.SaveChangesAsync();
                await fcmService.SendNotification("Thông báo mới", $"Đơn hàng {order.MaDonHang} của bạn đã được giao thành công", order.MaNguoiDung, order.MaDonHang, NotificationType.ORDER);
            }
            else throw new Exception("Cập nhật đơn hàng thất bại");
        }

        public async Task UpdateStatusOrderToCancelled(int id)
        {
            var isCustomer = _httpContextAccessor.HttpContext.User.IsCustomer();

            if(!isCustomer)
            {
                throw new NotFoundException("Bạn không có quyền thực hiện hành động này");
            }

            DonHang? order = await dbContext.DonHangs
                .Include(o => o.LichSuDonHang)
                .SingleOrDefaultAsync(o => o.MaDonHang == id)
                    ?? throw new NotFoundException("Không tìm thấy đơn hàng nào");


            if (order.TrangThai.Equals(OrderStatus.PENDING))
            {
                order.TrangThai = OrderStatus.CANCELLED;
                order.LichSuDonHang.Add(new LichSuDonHang
                {
                    TrangThai = OrderStatus.CANCELLED,
                    ThoiGianChinhSua = DateTime.Now,
                    GhiChu = "Đơn hàng đã bị khách hàng hủy đơn",
                });
                await dbContext.SaveChangesAsync();
                await fcmService.SendNotification("Thông báo mới", $"Đơn hàng {order.MaDonHang} của bạn đã được tiếp nhận", order.MaNguoiDung, order.MaDonHang, NotificationType.ORDER);
            }
            else throw new Exception("Cập nhật đơn hàng thất bại");
        }

        public async Task UpdateStatusOrderToCompleted(int id)
        {
            var isCustomer = _httpContextAccessor.HttpContext.User.IsCustomer();

            if (!isCustomer)
            {
                throw new NotFoundException("Bạn không có quyền thực hiện hành động này");
            }

            DonHang? order = await dbContext.DonHangs
                .Include(o => o.LichSuDonHang)
                .SingleOrDefaultAsync(o => o.MaDonHang == id)
                    ?? throw new NotFoundException("Không tìm thấy đơn hàng nào");


            if (order.TrangThai.Equals(OrderStatus.DELIVERED))
            {
                order.TrangThai = OrderStatus.COMPLETED;
                order.LichSuDonHang.Add(new LichSuDonHang
                {
                    TrangThai = OrderStatus.COMPLETED,
                    ThoiGianChinhSua = DateTime.Now,
                    GhiChu = "Khách hàng đã xác nhận được đơn hàng",
                });
                await dbContext.SaveChangesAsync();
                await fcmService.SendNotification("Thông báo mới", $"Đơn hàng {order.MaDonHang} của bạn đã được tiếp nhận", order.MaNguoiDung, order.MaDonHang, NotificationType.ORDER);
            }
            else throw new Exception("Cập nhật đơn hàng thất bại");
        }
    }
}
