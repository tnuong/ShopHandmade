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
    public class DanhMucService : IDanhMucService
    {
        private readonly MyStoreDbContext dbContext;
        private readonly ApplicationMapper applicationMapper;

        public DanhMucService(MyStoreDbContext dbContext, ApplicationMapper applicationMapper)
        {
            this.dbContext = dbContext;
            this.applicationMapper = applicationMapper;
        }


        public async Task<BaseResponse> CreateCategory(CategoryRequest request)
        {
            DanhMuc? checkParentCategory;
            var checkNotNull = request.ParentCategoryId != null && request.ParentCategoryId != 0;

            if (checkNotNull)
            {
                checkParentCategory = await dbContext.DanhMucs
                .SingleOrDefaultAsync(cate => cate.MaDanhMuc == request.ParentCategoryId)
                    ?? throw new NotFoundException("Danh mục cha không tồn tại");
            }

            DanhMuc category = new DanhMuc();
            category.TenDanhMuc = request.Name;
            category.MoTa = request.Description;
            if(checkNotNull)
                category.MaDanhMucCha = request.ParentCategoryId;

            var savedCategory = await dbContext.DanhMucs.AddAsync(category);
            await dbContext.SaveChangesAsync();

            var response = new DataResponse<DanhMucResource>();
            response.StatusCode = HttpStatusCode.Created;
            response.Message = "Thêm danh mục thành công";
            response.Success = true;
            response.Data = applicationMapper.MapToCategoryResource(savedCategory.Entity);

            return response;

        }

        public async Task<BaseResponse> GetAllCategories(int pageIndex, int pageSize, string searchString)
        {
            var lowerString = searchString.ToLower();
            var queryable = dbContext.DanhMucs
                .Where(c => !c.TrangThaiXoa && c.TenDanhMuc.ToLower().Contains(lowerString));

            List<DanhMuc> categories = await queryable
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var response = new PaginationResponse<List<DanhMucResource>>();
            response.StatusCode = HttpStatusCode.OK;
            response.Message = "Lấy thông tin danh mục thành công";
            response.Success = true;
            response.Data = categories.Select(category => applicationMapper.MapToCategoryResource(category)).ToList();
            response.Pagination = new Pagination()
            {
                TotalItems = queryable.Count(),
                TotalPages = (int)Math.Ceiling((double)queryable.Count() / pageSize)
            };

            return response;
        }

        public async Task<BaseResponse> GetAllCategoriesByLevel()
        {
            List<DanhMuc> categories = await dbContext.DanhMucs
                .Include(c => c.DanhSachDanhMucCon)
                .Where(c => c.MaDanhMucCha == null && !c.TrangThaiXoa)
                .ToListAsync();
            var response = new DataResponse<List<PhanCapDanhMucResource>>();
            response.StatusCode = HttpStatusCode.OK;
            response.Message = "Lấy thông tin danh mục thành công";
            response.Success = true;
            response.Data = categories.Select(category => applicationMapper.MapToCategoryLevelResource(category)).ToList();

            return response;
        }

        public async Task<BaseResponse> GetCategoryById(int id)
        {
            DanhMuc? category = await dbContext.DanhMucs
                .SingleOrDefaultAsync(ca => ca.MaDanhMuc == id && !ca.TrangThaiXoa)
                    ?? throw new NotFoundException("Không tìm thấy danh mục");

            var response = new DataResponse<DanhMucResource>();
            response.StatusCode = HttpStatusCode.OK;
            response.Message = "Lấy thông tin danh mục thành công";
            response.Success = true;
            response.Data = applicationMapper.MapToCategoryResource(category);

            return response;
        }

        public async Task RemoveCategory(int id)
        {
            DanhMuc? category = await dbContext.DanhMucs
                .Include(c => c.DanhSachSanPham)
                .Include(c => c.DanhSachDanhMucCon)
                .SingleOrDefaultAsync(ca => ca.MaDanhMuc == id && !ca.TrangThaiXoa)
                    ?? throw new NotFoundException("Không tìm thấy danh mục");

            if(category.DanhSachSanPham != null && category.DanhSachSanPham.Any())
            {
                category.TrangThaiXoa = true;
            } else
            {
                if(category.DanhSachDanhMucCon != null && category.DanhSachDanhMucCon.Any())
                {
                    foreach(var child in category.DanhSachDanhMucCon)
                    {
                        child.DanhMucCha = null;
                    }
                }

                dbContext.DanhMucs.Remove(category);
            }

            int rows = await dbContext.SaveChangesAsync();
            if (rows == 0) throw new Exception("Xóa danh mục thất bại");
        }

        public async Task<BaseResponse> UpdateCategory(int id, CategoryRequest request)
        {
            DanhMuc? category = await dbContext.DanhMucs
                .SingleOrDefaultAsync(ca => ca.MaDanhMuc == id && !ca.TrangThaiXoa)
                    ?? throw new NotFoundException("Không tìm thấy danh mục");

            DanhMuc? checkParentCategory;
            var checkNotNull = request.ParentCategoryId != null && request.ParentCategoryId != 0;

            if (checkNotNull)
            {
                checkParentCategory = await dbContext.DanhMucs
                .SingleOrDefaultAsync(cate => cate.MaDanhMuc == request.ParentCategoryId)
                    ?? throw new NotFoundException("Danh mục cha không tồn tại");
            }

            category.TenDanhMuc = request.Name;
            category.MoTa = request.Description;

            if (checkNotNull)
                category.MaDanhMucCha = request.ParentCategoryId;

            await dbContext.SaveChangesAsync();

            var response = new BaseResponse();
            response.StatusCode = HttpStatusCode.NoContent;
            response.Message = "Cập nhật danh mục thành công";
            response.Success = true;

            return response;
        }
    }
}
