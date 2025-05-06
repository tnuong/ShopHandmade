using back_end.Core.Constants;
using back_end.Core.Models;
using back_end.Core.Requests;
using back_end.Core.Responses;
using back_end.Core.Responses.Resources;
using back_end.Data;
using back_end.Exceptions;
using back_end.Mappers;
using back_end.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace back_end.Services.Implements
{
    public class KhuyenMaiService : IKhuyenMaiService
    {
        private readonly MyStoreDbContext dbContext;
        private readonly ApplicationMapper applicationMapper;

        public KhuyenMaiService(MyStoreDbContext dbContext, ApplicationMapper applicationMapper)
        {
            this.dbContext = dbContext;
            this.applicationMapper = applicationMapper;
        }

        public async Task<BaseResponse> AssignPromotion(AssignPromotionRequest request)
        {
            // Load trước danh sách sản phẩm và khuyến mãi cần thiết
            var products = await dbContext.SanPhams
                .Where(p => request.ProductIds.Contains(p.MaSanPham))
                .ToListAsync();

            if (products.Count != request.ProductIds.Count)
                throw new NotFoundException("Vui lòng kiểm tra lại danh sách sản phẩm");

            var promotions = await dbContext.KhuyenMais
                .Where(p => request.PromotionIds.Contains(p.MaKhuyenMai))
                .ToListAsync();

            // Kiểm tra khuyến mãi không hợp lệ
            var invalidPromotions = promotions.Where(p =>
                p.TrangThai == PromotionStatus.INACTIVE ||
                p.TrangThai == PromotionStatus.EXPIRED ||
                p.NgayKetThuc < DateTime.Now.Date).ToList();

            if (invalidPromotions.Any())
                throw new NotFoundException("Vui lòng kiểm tra lại danh sách khuyến mại");

            // Lấy danh sách đã gán để tránh trùng
            var existingMappings = await dbContext.SanPhamKhuyenMais
                .Where(sp => request.ProductIds.Contains(sp.MaSanPham) && request.PromotionIds.Contains(sp.MaKhuyenMai))
                .ToListAsync();

            var existingSet = new HashSet<string>(
                existingMappings.Select(x => $"{x.MaSanPham}-{x.MaKhuyenMai}")
            );

            var newMappings = new List<SanPhamKhuyenMai>();

            foreach (var productId in request.ProductIds)
            {
                foreach (var promotionId in request.PromotionIds)
                {
                    var key = $"{productId}-{promotionId}";
                    if (!existingSet.Contains(key))
                    {
                        newMappings.Add(new SanPhamKhuyenMai
                        {
                            MaSanPham = productId,
                            MaKhuyenMai = promotionId
                        });
                    }
                }
            }

            if (newMappings.Any())
            {
                await dbContext.SanPhamKhuyenMais.AddRangeAsync(newMappings);
                await dbContext.SaveChangesAsync();
            }

            return new BaseResponse
            {
                Message = "Áp dụng khuyến mãi cho sản phẩm thành công",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<BaseResponse> CreatePromotion(PromotionRequest request)
        {

            if(request.StartDate.Date > request.EndDate.Date)
                throw new Exception("Ngày bắt đầu không được lớn hơn ngày kết thúc");

            if(request.EndDate.Date < DateTime.Now.Date)
                throw new Exception("Ngày kết thúc phải lớn hơn hôm nay");

            if (request.PromotionType != PromotionDiscountType.PERCENTAGE && request.PromotionType != PromotionDiscountType.FIXED_AMOUNT)
                throw new Exception("Loại khuyến mại không hợp lệ");

            var promotion = new KhuyenMai()
            {
                GiaTriGiam = request.DiscountValue,
                LoaiKhuyenMai = request.PromotionType,
                NgayBatDau = request.StartDate,
                NgayKetThuc = request.EndDate,
                NoiDungKhuyenMai = request.Description,
                TenKhuyenMai = request.Name,
                TrangThai = request.IsActive ? PromotionStatus.ACTIVE : PromotionStatus.INACTIVE
            };

            await dbContext.KhuyenMais.AddAsync(promotion);
            await dbContext.SaveChangesAsync();

            return new BaseResponse()
            {
                Success = true,
                Message = "Thêm khuyến mại mới thành công",
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<BaseResponse> DeletePromotion(int promotionId)
        {
            var khuyenMai = await dbContext.KhuyenMais
                .Include(km => km.SanPhamKhuyenMais)
                .SingleOrDefaultAsync(km => km.MaKhuyenMai == promotionId)
                    ?? throw new NotFoundException("Khuyến mại không tồn tại");

            if(khuyenMai.SanPhamKhuyenMais.Count > 0)
            {
                dbContext.SanPhamKhuyenMais.RemoveRange(khuyenMai.SanPhamKhuyenMais);
            }

            dbContext.KhuyenMais.Remove(khuyenMai);

            await dbContext.SaveChangesAsync();

            return new BaseResponse()
            {
                Success = true,
                Message = "Xóa khuyến mại thành công",
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<BaseResponse> GetAllPromotions(int pageIndex, int pageSize, string searchString)
        {
            var lowerString = searchString?.ToLower() ?? "";
            var queryable = dbContext.KhuyenMais
                .Where(br =>br.TenKhuyenMai.ToLower().Contains(lowerString));

            List<KhuyenMai> khuyenMais = await queryable
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var khuyenMaiResources = khuyenMais.Select(km => applicationMapper.MapToKhuyenMai(km)).ToList();

            var response = new PaginationResponse<List<KhuyenMaiResource>>();
            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Message = "Lấy thông tin khuyến mại thành công";
            response.Data = khuyenMaiResources;
            response.Pagination = new Pagination
            {
                TotalItems = queryable.Count(),
                TotalPages = (int)Math.Ceiling((double)queryable.Count() / pageSize),
            };

            return response;
           
        }


        public async Task<BaseResponse> GetAllPromotions()
        {
            List<KhuyenMai> khuyenMais = await dbContext.KhuyenMais
                .ToListAsync();

            var khuyenMaiResources = khuyenMais.Select(km => applicationMapper.MapToKhuyenMai(km)).ToList();

            var response = new DataResponse<List<KhuyenMaiResource>>();
            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Message = "Lấy thông tin khuyến mại thành công";
            response.Data = khuyenMaiResources;
            return response;

        }
     
        public async Task<BaseResponse> GetPromotionById(int id)
        {
            var khuyenMai = await dbContext.KhuyenMais
                .SingleOrDefaultAsync(km => km.MaKhuyenMai == id)
                    ?? throw new NotFoundException("Không tìm thấy thông tin khuyến mại");

            return new DataResponse<KhuyenMaiResource>()
            {
                Data = applicationMapper.MapToKhuyenMai(khuyenMai),
                Message = "Lấy thông tin khuyến mại thành công",
                StatusCode = System.Net.HttpStatusCode.OK,
            };
        }

        public Task<BaseResponse> UpdatePromotion(int promotionId, PromotionRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
