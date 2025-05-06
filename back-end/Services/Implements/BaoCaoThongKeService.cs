using back_end.Core.Constants;
using back_end.Core.Models;
using back_end.Core.Responses;
using back_end.Core.Responses.Report;
using back_end.Data;
using back_end.Mappers;
using back_end.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace back_end.Services.Implements
{
    public class BaoCaoThongKeService : IBaoCaoThongKeService
    {
        private readonly MyStoreDbContext dbContext;
        private readonly ApplicationMapper applicationMapper;

        public BaoCaoThongKeService(MyStoreDbContext dbContext, ApplicationMapper mapper)
        {
            this.dbContext = dbContext;
            applicationMapper = mapper;
        }

        public async Task<BaseResponse> GetReportData(string type, DateTime? from, DateTime? to)
        {
            DateTime startDate;
            DateTime endDate = DateTime.Now;

            type ??= "";

            switch (type.ToLower())
            {
                case "today":
                    startDate = DateTime.Now.Date;
                    break;
                case "yesterday":
                    startDate = DateTime.Now.Date.AddDays(-1);
                    endDate = startDate.AddDays(1).AddTicks(-1); 
                    break;
                case "week":
                    startDate = DateTime.Now.Date.AddDays(-7);
                    break;
                case "month":
                    startDate = DateTime.Now.Date.AddMonths(-1);
                    break;
                default:
                    if (from.HasValue && to.HasValue)
                    {
                        startDate = from.Value.Date;
                        endDate = to.Value.Date.AddDays(1).AddTicks(-1);
                    }
                    else
                    {
                        startDate = DateTime.MinValue; 
                    }
                    break;
            }

            var orders = await dbContext.DonHangs
                .Where(p => p.NgayTao >= startDate && p.NgayTao <= endDate)
                .ToListAsync();

            var orderItems = await dbContext.ChiTietDonHangs
                .Include(o => o.DonHang)
                .Include(o => o.BienTheSanPham)
                    .ThenInclude(o => o.SanPham)
                .Where(p => p.DonHang.NgayTao >= startDate && p.DonHang.NgayTao <= endDate)
                .ToListAsync();

            int countProducts = orderItems
                .Where(p => p.DonHang.TrangThai != OrderStatus.REJECTED && p.DonHang.TrangThai != OrderStatus.CANCELLED)
                .Sum(s => s.SoLuong);

            var countOrders = orders.Where(p => p.TrangThai != OrderStatus.REJECTED && p.TrangThai != OrderStatus.CANCELLED).Count();

            var revenue = orders
                .Where(p => p.TrangThai == OrderStatus.COMPLETED || p.TrangThai == OrderStatus.DELIVERED)
                .Sum(o => o.TongTienSauKhuyenMai);

            var cost = orderItems
                .Where(p => p.DonHang.TrangThai == OrderStatus.COMPLETED || p.DonHang.TrangThai == OrderStatus.DELIVERED)
                .Sum(s => s.BienTheSanPham.SanPham.GiaNhap * s.SoLuong);

            var profit = revenue - cost;

            var report = new ReportResource()
            {
                Products = countProducts,
                Orders = countOrders,
                TotalRevenue = revenue,
                Profit = profit,
                TotalCost = cost
            };

            var orderQueryable = dbContext.DonHangs
                .Include(o => o.DanhSachChiTietDonHang)
                .ThenInclude(o => o.BienTheSanPham)
                .ThenInclude(o => o.SanPham);

            var newestOrders = orderQueryable
                .Take(Math.Min(5, orders.Count))
                .OrderByDescending(o => o.NgayTao)
                .ToList();

            report.NewestOrders = newestOrders.Select(o => applicationMapper.MapToOrderResource(o)).ToList();
            
            var response = new DataResponse<ReportResource>()
            {
                Data = report,
                Message = "Lấy dữ liệu báo cáo thành công",
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true
            };

            return response;
        }

        public async Task<BaseResponse> GetOrderPercentInRangeYear(int year)
        {
            var queryable = dbContext.DonHangs
                .Where(o => o.NgayTao.Year == year);
                

            var orders = queryable
                .GroupBy(o => o.NgayTao.Month)
                .Select(o => 
                    new OrderReport {
                        Month = o.Key,
                        Percent = (o.Count() * 100) / queryable.Count(),
                        Total = o.Count(),
                }).ToList();

            for(int i = 1; i <= 12; i++)
            {
                if(orders.Any(o => o.Month == i))
                    continue;

                var orderReport = new OrderReport
                {
                    Month = i,
                    Percent = 0,
                    Total = 0
                };

                orders.Add(orderReport);
            }

            var response = new DataResponse<List<OrderReport>>()
            {
                Data = orders.OrderBy(o => o.Month).ToList(),
                Message = "Lấy dữ liệu báo cáo đơn hàng thành công",
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
            };

            return response;
        }

        public async Task<BaseResponse> GetTopFiveBestSellerProducts(DateTime? fromTime, DateTime? toTime)
        {
            int products = await dbContext.SanPhams.CountAsync();

            IQueryable<DonHang> orderQueryable = dbContext.DonHangs
                .Include(o => o.DanhSachChiTietDonHang)
                .ThenInclude(o => o.BienTheSanPham)
                .ThenInclude(o => o.SanPham);

            if (fromTime != null)
            {
                orderQueryable = orderQueryable.Where(o => o.NgayTao >= fromTime && o.NgayTao <= toTime);
            }

            var topBestSellerProducts= dbContext.DonHangs
                .SelectMany(o => o.DanhSachChiTietDonHang)
                .GroupBy(oi => oi.BienTheSanPham.SanPham)
                .Select(g => new ProductReport
                {
                    Product = applicationMapper.MapToProductResource(g.Key),
                    Quantity = g.Sum(oi => oi.SoLuong)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(Math.Min(5, products))
                .ToList();

            var response = new DataResponse<List<ProductReport>>()
            {
                Data = topBestSellerProducts,
                Message = "Lấy dữ liệu top 5 sản phẩm thành công",
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true
            };

            return response;
        }

        public async Task<BaseResponse> GetOrderByMonth(DateTime time)
        {
            var queryable = dbContext.DonHangs
                .Where(o => o.NgayTao.Year == time.Year && o.NgayTao.Month == time.Month);


            var orders = queryable
                .GroupBy(o => o.TrangThai)
                .Select(o =>
                    new OrderReportByMonth
                    {
                        OrderStatus = o.Key,
                        Total = o.Count(),
                        Percent = (o.Count() * 100.0) / queryable.Count()
                    }).ToList();

           
            var response = new DataResponse<List<OrderReportByMonth>>()
            {
                Data = orders,
                Message = "Lấy dữ liệu báo cáo đơn hàng thành công",
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
            };

            return response;
        }
    }
}
