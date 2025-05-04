using back_end.Core.Models;
using back_end.Core.Requests;
using back_end.Core.Responses;
using back_end.Core.Responses.Resources;
using back_end.Data;
using back_end.Exceptions;
using back_end.Extensions;
using back_end.Mappers;
using back_end.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace back_end.Services.Implements
{
    public class DanhGiaService : IDanhGiaService
    {
        private readonly MyStoreDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationMapper applicationMapper;
        private readonly UserManager<NguoiDung> userManager;

        public DanhGiaService(MyStoreDbContext dbContext, IHttpContextAccessor httpContextAccessor, ApplicationMapper applicationMapper, UserManager<NguoiDung> userManager)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
            this.applicationMapper = applicationMapper;
            this.userManager = userManager;
        }

        public async Task<BaseResponse> CreateEvaluation(EvaluationRequest request)
        {
            SanPham? product = await dbContext.SanPhams
                .SingleOrDefaultAsync(p => p.MaSanPham == request.ProductId)
                    ?? throw new NotFoundException("Sản phẩm không tồn tại");

            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value
                ?? throw new BadCredentialsException("Vui lòng đăng nhập lại");

            DanhGiaSanPham evaluation = new DanhGiaSanPham();
            evaluation.SoSaoDanhGia = request.Stars;
            evaluation.NoiDung = request.Content;
            evaluation.NgayTao = DateTime.Now;
            evaluation.MaSanPham = request.ProductId;
            evaluation.MaNguoiDanhGia = userId;

            await dbContext.DanhGiaSanPhams.AddAsync(evaluation);
            await dbContext.SaveChangesAsync();

            var response = new BaseResponse();
            response.StatusCode = System.Net.HttpStatusCode.Created;
            response.Message = "Đánh giá sản phẩm thành công";
            response.Success = true;
            return response;

        }

        public async Task<BaseResponse> GetAllByProductId(int productId, int pageIndex, int pageSize)
        {
            var queryable = dbContext.DanhGiaSanPhams
                .Where(e => e.MaSanPham == productId)
                .AsQueryable();

            int total = await queryable.CountAsync();
            double averageStar = total > 0 ? await queryable.AverageAsync(e => e.SoSaoDanhGia) : 0;

            var starPercents = await queryable
                .GroupBy(e => e.SoSaoDanhGia) 
                .Select(g => new TiLeSao
                {
                    Star = g.Key, 
                    TotalEvaluation = g.Count(),
                    Percent = ((double)g.Count() / total) * 100
                })
                .ToListAsync();

            var evaluations = await queryable
                .Include(p => p.NguoiDanhGia)
                .Include(p => p.DanhSachNguoiYeuThich)
                .Skip((pageIndex - 1) * pageSize)   
                .Take(pageSize)
                .ToListAsync();

            var resources = new List<DanhGiaResource>();
            if(httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = httpContextAccessor.HttpContext.User.GetUserId();
                resources = evaluations.Select(e => applicationMapper.MapToEvaluationResource(e, userId)).ToList();
            } else
            {
                resources = evaluations.Select(e => applicationMapper.MapToEvaluationResourceWithoutPrincipal(e)).ToList();
            }
            var response = new PaginationResponse<ReportEvaluationResource>();
            response.Message = "Lấy danh sách đánh giá thành công";
            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Data = new ReportEvaluationResource
            {
                Report = new PhanTichDanhGiaResource
                {
                    AverageStar = averageStar,
                    StarsPercents = starPercents,
                    TotalEvaluation = total,
                },
                Results = resources
            };

            response.Pagination = new Pagination
            {
                TotalItems = total,
                TotalPages = (int)Math.Ceiling((double)total / pageSize),
            };

            return response;
        }

        public async Task<BaseResponse> GetAllExcellentEvaluation()
        {
            var evaluations = await dbContext.DanhGiaSanPhams
                .Include(e => e.NguoiDanhGia)
                .Where(e => e.SoSaoDanhGia >= 4)
                .Take(3)
                .ToListAsync();

            var response = new DataResponse<List<DanhGiaResource>>();
            response.Success = true;
            response.Data = evaluations.Select(e => applicationMapper.MapToEvaluationResourceWithoutPrincipal(e)).ToList();
            response.StatusCode= System.Net.HttpStatusCode.OK;
            return response;
        }

        public async Task InteractEvaluation(int id)
        {
            DanhGiaSanPham? evaluation = await dbContext.DanhGiaSanPhams
                .Include(e => e.DanhSachNguoiYeuThich)
                .SingleOrDefaultAsync(p => p.MaDanhGiaSP == id)
                    ?? throw new NotFoundException("Đánh giá không tồn tại");

            var userId = httpContextAccessor.HttpContext.User.GetUserId();

            NguoiDung user = await userManager.FindByIdAsync(userId)
                ?? throw new BadCredentialsException("Vui lòng đăng nhập lại");

            if (evaluation.DanhSachNguoiYeuThich != null && evaluation.DanhSachNguoiYeuThich.Any(item => item.Id.Equals(userId)))
            {
                evaluation.DanhSachNguoiYeuThich.Remove(user);
            } else evaluation.DanhSachNguoiYeuThich.Add(user);

            int rows = await dbContext.SaveChangesAsync();
            if (rows == 0) throw new Exception("Tương tác thất bại");
        }
    }
}
