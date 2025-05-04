using back_end.Core.Models;
using back_end.Core.Requests;
using back_end.Core.Responses;
using back_end.Core.Responses.Resources;
using back_end.Data;
using back_end.Exceptions;
using back_end.Infrastructures.Cloudinary;
using back_end.Mappers;
using back_end.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace back_end.Services.Implements
{
    public class BienTheSanPhamService : IBienTheSanPhamService
    {
        private readonly MyStoreDbContext dbContext;
        private readonly IUploadService uploadService;
        private readonly ApplicationMapper applicationMapper;

        public BienTheSanPhamService(MyStoreDbContext dbContext, IUploadService uploadService, ApplicationMapper applicationMapper)
        {
            this.dbContext = dbContext;
            this.uploadService = uploadService;
            this.applicationMapper = applicationMapper;
        }

        public async Task<BaseResponse> CreateVariant(VariantRequest request)
        {
            SanPham? product = await dbContext.SanPhams.SingleOrDefaultAsync(p => p.MaSanPham == request.ProductId)
                ?? throw new NotFoundException("Không tìm thấy sản phẩm");

            KichThuoc? size = await dbContext.KichThuocs.SingleOrDefaultAsync(s => s.MaKichThuoc == request.SizeId)
                ?? throw new NotFoundException("Không tìm thấy kích cỡ");

            MauSac? color = await dbContext.MauSacs.SingleOrDefaultAsync(c => c.MaMauSac == request.ColorId)
                ?? throw new NotFoundException("Không tìm thấy màu sắc");

            BienTheSanPham? checkVariant = await dbContext.BienTheSanPhams
                .SingleOrDefaultAsync(p => p.MaKichThuoc == request.SizeId && p.MaMauSac == request.ColorId && p.MaSanPham == request.ProductId);

            if (checkVariant != null)
                throw new Exception("Biến thể này đã tồn tại");

            BienTheSanPham variant = new BienTheSanPham();
            variant.MaSanPham = request.ProductId;
            variant.MaKichThuoc = request.SizeId;
            variant.MaMauSac = request.ColorId;
            variant.SoLuongTonKho = request.InStock;
            variant.DanhSachHinhAnh = new List<HinhAnhBienTheSanPham>();

            try
            {
                var thumbnailUrl = await uploadService.UploadSingleFileAsync(request.ThumbnailUrl[0]);
                variant.DuongDanAnh = thumbnailUrl;
                var images = await uploadService.UploadMutlipleFilesAsync(request.Images ?? new List<IFormFile>());

                foreach (var image in images)
                {
                    HinhAnhBienTheSanPham imageVariant = new HinhAnhBienTheSanPham();
                    imageVariant.DuongDanAnh = image;
                    variant.DanhSachHinhAnh.Add(imageVariant);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            var savedVariant = await dbContext.BienTheSanPhams.AddAsync(variant);
            await dbContext.SaveChangesAsync();

            var response = new DataResponse<BienTheSanPhamResource>();
            response.StatusCode = System.Net.HttpStatusCode.Created;
            response.Success = true;
            response.Message = "Thêm sản phẩm thành công";
            response.Data = applicationMapper.MapToVariantResource(savedVariant.Entity);

            return response;
               
        }

        public async Task<BaseResponse> GetAllVariants(int pageIndex, int pageSize, string searchString)
        {
            string lowerString = searchString?.ToLower() ?? string.Empty;

            var queryable = dbContext.BienTheSanPhams
                .AsNoTracking()
                .Include(p => p.SanPham)
                .Where(v => !v.TrangThaiXoa && (string.IsNullOrEmpty(lowerString) || v.SanPham.TenSanPham.ToLower().Contains(lowerString)));

            int totalItems = await queryable.CountAsync();

            if(lowerString != null && lowerString.Length > 1)
            {
                pageIndex = 1;
            }

            List<BienTheSanPham> variants = await queryable
                .Include(p => p.MauSac)
                .Include(p => p.KichThuoc)
                .Include(p => p.DanhSachHinhAnh)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = new PaginationResponse<List<BienTheSanPhamResource>>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = "Lấy danh sách sản phẩm thành công",
                Data = variants.Select(variant => applicationMapper.MapToVariantResource(variant)).ToList(),
                Pagination = new Pagination()
                {
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                }
            };

            return response;
        }

        public async Task<BaseResponse> GetAllVariantsByProductId(int productId, int pageIndex, int pageSize, string searchString)
        {
            string lowerString = searchString?.ToLower() ?? string.Empty;

            var queryable = dbContext.BienTheSanPhams
                .AsNoTracking()
                .Include(p => p.SanPham)
                .Where(v => v.MaSanPham == productId && !v.TrangThaiXoa && (string.IsNullOrEmpty(lowerString) || v.SanPham.TenSanPham.ToLower().Contains(lowerString)));

            int totalItems = await queryable.CountAsync();

            if (lowerString != null && lowerString.Length > 1)
            {
                pageIndex = 1;
            }

            List<BienTheSanPham> variants = await queryable
                .Include(p => p.MauSac)
                .Include(p => p.KichThuoc)
                .Include(p => p.DanhSachHinhAnh)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = new PaginationResponse<List<BienTheSanPhamResource>>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = "Lấy danh sách sản phẩm thành công",
                Data = variants.Select(variant => applicationMapper.MapToVariantResource(variant)).ToList(),
                Pagination = new Pagination()
                {
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                }
            };

            return response;
            //List<ProductVariant> variants = await dbContext.ProductVariants
            //    .Include(p => p.Color)
            //    .Include(p => p.Size)
            //    .Where(p => p.ProductId == productId && !p.IsDeleted).ToListAsync();

            //var response = new DataResponse<List<VariantResource>>();
            //response.StatusCode = System.Net.HttpStatusCode.OK;
            //response.Success = true;
            //response.Message = "Lấy danh sách sản phẩm thành công";
            //response.Data = variants.Select(variant => applicationMapper.MapToVariantResource(variant)).ToList();

            //return response;
        }

        public async Task<BaseResponse> GetAllVariantsByProductIdAndColorId(int productId, int colorId)
        {
            var variants = await dbContext.BienTheSanPhams
             .Where(p => p.MaSanPham == productId && p.MaMauSac == colorId && !p.TrangThaiXoa)
             .Include(p => p.MauSac)
             .Include(p => p.KichThuoc)
             .ToListAsync();

            var response = new DataResponse<List<BienTheSanPhamResource>>();
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Success = true;
            response.Message = "Lấy danh sách sản phẩm thành công";
            response.Data = variants.Select(variant => applicationMapper.MapToVariantResource(variant)).ToList();

            return response;
        }

        public async Task<BaseResponse> GetUniqueColorVariantsByProductId(int productId)
        {
            var variants = await dbContext.BienTheSanPhams
             .Where(p => p.MaSanPham == productId && !p.TrangThaiXoa)
             .Include(p => p.MauSac)
             .Include(p => p.KichThuoc)
             .GroupBy(p => p.MaMauSac)  
             .Select(g => g.First())  
             .ToListAsync();

            var response = new DataResponse<List<BienTheSanPhamResource>>();
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Success = true;
            response.Message = "Lấy danh sách sản phẩm thành công";
            response.Data = variants.Select(variant => applicationMapper.MapToVariantResource(variant)).ToList();

            return response;
        }

        public async Task<BaseResponse> GetUniqueSizeVariantsByProductId(int productId)
        {
            var variants = await dbContext.BienTheSanPhams
             .Where(p => p.MaSanPham == productId && !p.TrangThaiXoa)
             .Include(p => p.MauSac)
             .Include(p => p.KichThuoc)
             .GroupBy(p => p.MaKichThuoc)
             .Select(g => g.First())
             .ToListAsync();

            var response = new DataResponse<List<BienTheSanPhamResource>>();
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Success = true;
            response.Message = "Lấy danh sách sản phẩm thành công";
            response.Data = variants.Select(variant => applicationMapper.MapToVariantResource(variant)).ToList();

            return response;
        }

        public async Task<BaseResponse> GetVariantById(int id)
        {
            var variant = await dbContext.BienTheSanPhams
                .Include(v => v.KichThuoc)
                .Include(v => v.MauSac)
                .Include(v => v.DanhSachHinhAnh)
                .Include(v => v.SanPham)
                .SingleOrDefaultAsync(v => v.MaBienTheSanPham == id && !v.TrangThaiXoa)
                    ?? throw new NotFoundException("Không tìm thấy sản phẩm nào");

            var response = new DataResponse<BienTheSanPhamResource>();
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Success = true;
            response.Message = "Lấy danh sách sản phẩm thành công";
            response.Data = applicationMapper.MapToVariantResource(variant);

            return response;
        }

        public async Task RemoveImages(List<int> ids)
        {
            foreach (int item in ids)
            {
                HinhAnhBienTheSanPham? productImage = await dbContext.HinhAnhBienTheSanPhams
                    .FindAsync(item);

                if (productImage == null)
                    continue;

                dbContext.HinhAnhBienTheSanPhams.Remove(productImage);
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task RemoveVariant(int id)
        {
            var variant = await dbContext.BienTheSanPhams
               .Include(v => v.DanhSachHinhAnh)
               .Include (v => v.DanhSachChiTietDonHang)
               .SingleOrDefaultAsync(v => v.MaBienTheSanPham == id && !v.TrangThaiXoa)
                   ?? throw new NotFoundException("Không tìm thấy sản phẩm nào");

            if(variant.DanhSachChiTietDonHang != null && variant.DanhSachChiTietDonHang.Any())
            {
                variant.TrangThaiXoa = true;
            } else
            {
                if (variant.DanhSachHinhAnh != null && variant.DanhSachHinhAnh.Any())
                {
                    dbContext.HinhAnhBienTheSanPhams.RemoveRange(variant.DanhSachHinhAnh);
                }
                else
                {
                    dbContext.BienTheSanPhams.Remove(variant);
                }
            }

            int rows = await dbContext.SaveChangesAsync();

            if (rows == 0) throw new Exception("Xóa sản phẩm thất bại");
        }

        public async Task UpdateVariant(int id, EditVariantRequest request)
        {
            var variant = await dbContext.BienTheSanPhams
                .SingleOrDefaultAsync(v => v.MaBienTheSanPham == id && !v.TrangThaiXoa)
                    ?? throw new NotFoundException("Không tìm thấy sản phẩm nào");

            SanPham? product = await dbContext.SanPhams.SingleOrDefaultAsync(p => p.MaSanPham == request.ProductId)
                ?? throw new NotFoundException("Không tìm thấy sản phẩm");

            KichThuoc? size = await dbContext.KichThuocs.SingleOrDefaultAsync(s => s.MaKichThuoc == request.SizeId)
                ?? throw new NotFoundException("Không tìm thấy kích cỡ");

            MauSac? color = await dbContext.MauSacs.SingleOrDefaultAsync(c => c.MaMauSac == request.ColorId)
                ?? throw new NotFoundException("Không tìm thấy màu sắc");

            variant.MaSanPham = request.ProductId;
            variant.MaKichThuoc = request.SizeId;
            variant.MaMauSac = request.ColorId;
            variant.SoLuongTonKho = request.InStock;

            int rows = await dbContext.SaveChangesAsync();

            if (rows == 0) throw new Exception("Cập nhật sản phẩm thất bại");
        }

        public async Task UploadImages(int id, List<IFormFile> files)
        {
            var variant = await dbContext.BienTheSanPhams
                .Include(v => v.DanhSachHinhAnh)
                .SingleOrDefaultAsync(v => v.MaBienTheSanPham == id && !v.TrangThaiXoa)
                    ?? throw new NotFoundException("Không tìm thấy sản phẩm nào");

            List<string> otherImages = await uploadService.UploadMutlipleFilesAsync(files);
            foreach (var image in otherImages)
            {
                HinhAnhBienTheSanPham productImage = new HinhAnhBienTheSanPham();
                productImage.DuongDanAnh = image;
                variant.DanhSachHinhAnh.Add(productImage);
            }

            int rows = await dbContext.SaveChangesAsync();

            if (rows == 0) throw new Exception("Thêm các ảnh sản phẩm thất bại");
        }

        public async Task UploadThumbnail(int id, IFormFile file)
        {
            var variant = await dbContext.BienTheSanPhams
                .SingleOrDefaultAsync(v => v.MaBienTheSanPham == id && !v.TrangThaiXoa)
                    ?? throw new NotFoundException("Không tìm thấy sản phẩm nào");

            string thumbnail = await uploadService.UploadSingleFileAsync(file);
            variant.DuongDanAnh = thumbnail;
            int rows = await dbContext.SaveChangesAsync();

            if (rows == 0) throw new Exception("Cập nhật ảnh sản phẩm thất bại");
        }
    }
}
