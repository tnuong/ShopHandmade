using back_end.Core.Constants;
using back_end.Core.Models;
using back_end.Core.Requests;
using back_end.Core.Responses;
using back_end.Core.Responses.Resources;
using back_end.Data;
using back_end.Exceptions;
using back_end.Extensions;
using back_end.Infrastructures.Cloudinary;
using back_end.Mappers;
using back_end.Services.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace back_end.Services.Implements
{
    public class SanPhamService : ISanPhamService
    {
        private readonly MyStoreDbContext dbContext;
        private readonly IUploadService uploadService;
        private readonly ApplicationMapper applicationMapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public SanPhamService(MyStoreDbContext dbContext, IUploadService uploadService, ApplicationMapper applicationMapper, IHttpContextAccessor contextAccessor)
        {
            this.dbContext = dbContext;
            this.uploadService = uploadService;
            this.applicationMapper = applicationMapper;
            this.httpContextAccessor = contextAccessor;
        }

        public async Task UploadImages(int id, List<IFormFile> files)
        {
            SanPham? product = await dbContext.SanPhams
                .Include(p => p.DanhSachHinhAnh)
                .SingleOrDefaultAsync(p => p.MaSanPham == id && !p.TrangThaiXoa)
                    ?? throw new NotFoundException("Không tìm thấy sản phẩm");

            List<string> otherImages = await uploadService.UploadMutlipleFilesAsync(files);
            foreach (var image in otherImages)
            {
                HinhAnhSanPham productImage = new HinhAnhSanPham();
                productImage.DuongDanAnh = image;
                product.DanhSachHinhAnh.Add(productImage);
            }

            int rows = await dbContext.SaveChangesAsync();

            if (rows == 0) throw new Exception("Thêm các ảnh sản phẩm thất bại");
        }

        public async Task<BaseResponse> CreateProduct(CreateProductRequest request)
        {
            NhanHieu? brand = await dbContext.NhanHieus
                    .SingleOrDefaultAsync(b => b.MaNhanHieu == request.BrandId)
                        ?? throw new NotFoundException("Không tìm thấy thương hiệu");

            DanhMuc? category = await dbContext.DanhMucs
                .SingleOrDefaultAsync(b => b.MaDanhMuc == request.CategoryId)
                    ?? throw new NotFoundException("Không tìm thấy danh mục");

            NhaSanXuat? manufacturer = await dbContext.NhaSanXuats
               .SingleOrDefaultAsync(b => b.MaNhaSX == request.ManufacturerId)
                   ?? throw new NotFoundException("Không tìm thấy nhà sản xuất");

            try
            {
                
                string thumbnail = await uploadService.UploadSingleFileAsync(request.Thumbnail[0]);
                string zoomImage = await uploadService.UploadSingleFileAsync(request.ZoomImage[0]);
               

                SanPham product = new SanPham();
                product.HinhDaiDien = thumbnail;
                product.HinhPhongTo = zoomImage;
                product.TenSanPham = request.Name;
                product.MoTa = request.Description;
                product.GiaCu = request.OldPrice;
                product.GiaHienTai = request.Price;
                product.GiaNhap = request.PurchasePrice;
                product.MaNhanHieu = request.BrandId;
                product.MaDanhMuc = request.CategoryId;
                product.MaNhaSanXuat = request.ManufacturerId;    
                product.DanhSachHinhAnh = new List<HinhAnhSanPham>();

                if(request.OtherImages != null)
                {
                    List<string> otherImages = await uploadService.UploadMutlipleFilesAsync(request.OtherImages!);
                    foreach (var image in otherImages)
                    {
                        HinhAnhSanPham productImage = new HinhAnhSanPham();
                        productImage.DuongDanAnh = image;
                        product.DanhSachHinhAnh.Add(productImage);
                    }
                }
               
                await dbContext.SanPhams.AddAsync(product);
                await dbContext.SaveChangesAsync();

                var response = new DataResponse<SanPhamResource>();
                response.Message = "Thêm sản phẩm mới thành công";
                response.Success = true;
                response.StatusCode = System.Net.HttpStatusCode.Created;
                response.Data = applicationMapper.MapToProductResource(product);

                return response;

            } catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse> GetAllProducts(int pageIndex, int pageSize, double minPrice, double maxPrice, List<int> brandIds, List<int> categoryIds, List<int> colorIds, List<int> sizeIds, string sortBy, string sortOrder)
        {
          
            var productsQuery = dbContext.SanPhams.Where(p => p.TrangThaiXoa == false)
                .Include(p => p.DanhMuc)
                .Include(p => p.NhanHieu)
                .Include(p => p.NhaSanXuat)
                .Include(p => p.DanhSachHinhAnh)
                .Include(p => p.DanhSachBienTheSP)
                    .ThenInclude(pv => pv.KichThuoc)
                .Include(p => p.DanhSachBienTheSP)
                    .ThenInclude(pv => pv.MauSac)
                .AsQueryable();

            if (categoryIds != null && categoryIds.Any())
            {
                productsQuery = productsQuery.Where(p => categoryIds.Contains(p.MaDanhMuc.Value));
            }

            if (brandIds != null && brandIds.Any())
            {
                productsQuery = productsQuery.Where(p => brandIds.Contains(p.MaNhanHieu.Value));
            }

            if(maxPrice > 0 && minPrice <= maxPrice)
            {
                productsQuery = productsQuery.Where(p => p.GiaHienTai >= minPrice && p.GiaHienTai <= maxPrice);
            }

            if(sortBy != null && sortOrder != null)
            {
                productsQuery = sortBy switch
                {
                    "price" => sortOrder.ToLower() == "asc"
                        ? productsQuery.OrderBy(p => p.GiaHienTai)
                        : productsQuery.OrderByDescending(p => p.GiaHienTai),
                    "name" => sortOrder.ToLower() == "asc"
                        ? productsQuery.OrderBy(p => p.TenSanPham)
                        : productsQuery.OrderByDescending(p => p.TenSanPham),
                    _ => productsQuery 
                };
            }

            if ((sizeIds != null && sizeIds.Any()) || (colorIds != null && colorIds.Any()))
            {
                productsQuery = productsQuery.Where(p => p.DanhSachBienTheSP.Any(pv =>
                    (sizeIds == null || !sizeIds.Any() || sizeIds.Contains(pv.MaKichThuoc)) &&
                    (colorIds == null || !colorIds.Any() || colorIds.Contains(pv.MaMauSac))));
            }

            var totalItems = await productsQuery.CountAsync();

            var products = await productsQuery
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userId = httpContextAccessor.HttpContext.User.GetUserId();
            var resources = new List<SanPhamResource>();
            foreach (var product in products) {
                var promotions = await dbContext.SanPhamKhuyenMais
                    .Include(p => p.KhuyenMai)
                    .Where(p => p.MaSanPham == product.MaSanPham && p.KhuyenMai.TrangThai == PromotionStatus.ACTIVE && p.KhuyenMai.NgayKetThuc >= DateTime.Now.Date)
                    .ToListAsync();

                var promotionResources = promotions.Select(p => applicationMapper.MapToKhuyenMai(p.KhuyenMai)).ToList();
                var productResource = applicationMapper.MapToProductResource(product);
                productResource.HasWishlist = await dbContext.DanhSachYeuThichs
                    .Include(s => s.DanhSachSanPham)
                    .AnyAsync(s => s.MaNguoiDung == userId && s.DanhSachSanPham.Any(t => t.MaSanPham == product.MaSanPham));
                productResource.Promotions = promotionResources;
                resources.Add(productResource);
            }

            var response = new PaginationResponse<List<SanPhamResource>>
            {
                Success = true,
                Message = "Lấy danh sách sản phẩm thành công",
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = resources,
                Pagination = new Pagination
                {
                    TotalItems = totalItems,
                    TotalPages = products.Count > 0 ? (int)Math.Ceiling((double)totalItems / pageSize) : 0,
                }
            };

            return response;

        }

        public async Task<BaseResponse> GetProductById(int id)
        {
            SanPham? product = await dbContext.SanPhams
                .Include (p => p.DanhMuc)
                .Include(p => p.NhanHieu)
                .Include(p => p.NhaSanXuat)
                .Include(p => p.DanhSachHinhAnh)
                .Include(p => p.DanhSachBienTheSP)
                .SingleOrDefaultAsync(p => p.MaSanPham == id && p.TrangThaiXoa == false)
                    ?? throw new NotFoundException("Không tìm thấy sản phẩm");

            var promotions = await dbContext.SanPhamKhuyenMais
                   .Include(p => p.KhuyenMai)
                   .Where(p => p.MaSanPham == product.MaSanPham && p.KhuyenMai.TrangThai == PromotionStatus.ACTIVE && p.KhuyenMai.NgayKetThuc >= DateTime.Now.Date)
                   .ToListAsync();

            var userId = httpContextAccessor.HttpContext.User.GetUserId();
            var resource = applicationMapper.MapToProductResource(product);
            resource.Promotions = promotions.Select(p => applicationMapper.MapToKhuyenMai(p.KhuyenMai)).ToList();
            resource.HasWishlist = await dbContext.DanhSachYeuThichs
                  .Include(s => s.DanhSachSanPham)
                  .AnyAsync(s => s.MaNguoiDung == userId && s.DanhSachSanPham.Any(t => t.MaSanPham == product.MaSanPham));

            var response = new DataResponse<SanPhamResource>();
            response.Success = true;
            response.Message = "Lấy danh sách sản phẩm thành công";
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Data = resource;
            return response;

        }

        public async Task UpdateProduct(int id, EditProductRequest request)
        {
            SanPham? product = await dbContext.SanPhams
                .SingleOrDefaultAsync(p => p.MaSanPham == id && !p.TrangThaiXoa)
                    ?? throw new NotFoundException("Không tìm thấy sản phẩm");

            NhanHieu? brand = await dbContext.NhanHieus
                    .SingleOrDefaultAsync(b => b.MaNhanHieu == request.BrandId)
                        ?? throw new NotFoundException("Không tìm thấy thương hiệu");

            DanhMuc? category = await dbContext.DanhMucs
                .SingleOrDefaultAsync(b => b.MaDanhMuc == request.CategoryId)
                    ?? throw new NotFoundException("Không tìm thấy danh mục");

            NhaSanXuat? manufacturer = await dbContext.NhaSanXuats
                .SingleOrDefaultAsync(b => b.MaNhaSX == request.ManufacturerId)
                    ?? throw new NotFoundException("Không tìm thấy nhà sản xuất");

            product.TenSanPham = request.Name;
            product.MoTa = request.Description;
            product.GiaCu = request.OldPrice;
            product.GiaHienTai = request.Price;
            product.GiaNhap = request.PurchasePrice;
            product.MaNhanHieu = request.BrandId;
            product.MaDanhMuc = request.CategoryId;
            product.NgayTao = DateTime.Now;
            product.MaNhaSanXuat = request.ManufacturerId;

            int rows = await dbContext.SaveChangesAsync();

            if (rows == 0) throw new Exception("Cập nhật sản phẩm thất bại");
        }

        public async Task UploadThumbnail(int id, IFormFile file)
        {
            SanPham? product = await dbContext.SanPhams
                .SingleOrDefaultAsync(p => p.MaSanPham == id && !p.TrangThaiXoa)
                    ?? throw new NotFoundException("Không tìm thấy sản phẩm");

            string thumbnail = await uploadService.UploadSingleFileAsync(file);
            product.HinhDaiDien = thumbnail;
            int rows = await dbContext.SaveChangesAsync();

            if (rows == 0) throw new Exception("Cập nhật ảnh sản phẩm thất bại");
        }

        public async Task UploadZoomImage(int id, IFormFile file)
        {
            SanPham? product = await dbContext.SanPhams
               .SingleOrDefaultAsync(p => p.MaSanPham == id && !p.TrangThaiXoa)
                   ?? throw new NotFoundException("Không tìm thấy sản phẩm");

            string zoomImage = await uploadService.UploadSingleFileAsync(file);
            product.HinhPhongTo = zoomImage;
            int rows = await dbContext.SaveChangesAsync();

            if (rows == 0) throw new Exception("Cập nhật ảnh sản phẩm thất bại");
        }

        public async Task RemoveImages(List<int> ids)
        {
            foreach(int item in ids)
            {
                HinhAnhSanPham? productImage = await dbContext.HinhAnhSanPhams
                    .FindAsync(item);

                if (productImage == null)
                    continue;

                dbContext.HinhAnhSanPhams.Remove(productImage);
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task RemoveProduct(int id)
        {
            SanPham? product = await dbContext.SanPhams
               .Include(p => p.DanhSachHinhAnh)
               .Include(p => p.DanhSachBienTheSP)
               .Include(p => p.DanhSachDanhGia)
               .SingleOrDefaultAsync(p => p.MaSanPham == id && !p.TrangThaiXoa)
                   ?? throw new NotFoundException("Không tìm thấy sản phẩm");

            if(
                (product.DanhSachBienTheSP == null || product.DanhSachBienTheSP.Count == 0)
                && (product.DanhSachDanhGia == null || product.DanhSachDanhGia.Count == 0)
            )
            {
                if(product.DanhSachHinhAnh != null && product.DanhSachHinhAnh.Count > 0)
                {
                    dbContext.HinhAnhSanPhams.RemoveRange(product.DanhSachHinhAnh);
                }

                dbContext.SanPhams.Remove(product);
            } else
            {
                product.TrangThaiXoa = true;
            }

            int rows = await dbContext.SaveChangesAsync();
            if (rows == 0) throw new Exception("Xóa sản phẩm thất bại");
        }

        public async Task<BaseResponse> SearchProduct(string searchValue, int pageIndex, int pageSize)
        {
            string lowerCaseValue = searchValue?.ToLower() ?? "";
            var queryable = dbContext.SanPhams
                .Where(p => p.TenSanPham.ToLower().Contains(lowerCaseValue));

            var totalItems = await queryable.CountAsync();
            var products = await queryable
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = new PaginationResponse<List<SanPhamResource>>
            {
                Data = products.Select(p => applicationMapper.MapToProductResource(p)).ToList(),
                Message = $"Tìm thấy {products.Count} cho {searchValue}",
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Pagination = new Pagination
                {
                    TotalItems = totalItems,
                    TotalPages = (int) Math.Ceiling((double)totalItems / pageSize),
                }
            };

            return response;
        }

        public async Task<BaseResponse> GetBestSellerProducts()
        {
            var productQuantities = await dbContext.ChiTietDonHangs
                .Include(o => o.BienTheSanPham)
                        .ThenInclude(o => o.SanPham)
                .GroupBy(o => o.BienTheSanPham.SanPham)
                .Select(o => new
                {
                    Product = o.Key,
                    Quantity = o.Sum(o => o.SoLuong),
                })
                .OrderByDescending(o => o.Quantity)
                .Take(8)
                .ToListAsync();

            var userId = httpContextAccessor.HttpContext.User.GetUserId();
            var resources = new List<SanPhamResource>();
            foreach (var productQuantity in productQuantities)
            {
                var promotions = await dbContext.SanPhamKhuyenMais
                    .Include(p => p.KhuyenMai)
                    .Where(p => p.MaSanPham == productQuantity.Product.MaSanPham && p.KhuyenMai.TrangThai == PromotionStatus.ACTIVE && p.KhuyenMai.NgayKetThuc >= DateTime.Now.Date)
                    .ToListAsync();

                var promotionResources = promotions.Select(p => applicationMapper.MapToKhuyenMai(p.KhuyenMai)).ToList();
                var productResource = applicationMapper.MapToProductResource(productQuantity.Product);
                productResource.Promotions = promotionResources;
                productResource.Quantity = await dbContext.BienTheSanPhams.CountAsync(p => p.MaSanPham == productQuantity.Product.MaSanPham);
                productResource.HasWishlist = await dbContext.DanhSachYeuThichs
                 .Include(s => s.DanhSachSanPham)
                 .AnyAsync(s => s.MaNguoiDung == userId && s.DanhSachSanPham.Any(t => t.MaSanPham == productQuantity.Product.MaSanPham));
                resources.Add(productResource);
            }

            var response = new DataResponse<List<SanPhamResource>>();
            response.Data = resources;
            response.Message = "Lấy top sản phẩm bán chạy nhất thành công";
            response.StatusCode = HttpStatusCode.OK;
            response.Success = true;

            return response;
        }

        public async Task<BaseResponse> GetMostFavoriteProducts()
        {
            var productQuantities = await dbContext.DanhGiaSanPhams
                .Include(e => e.SanPham)
               
                .GroupBy(e => e.SanPham)
                .Select(e => new
                {
                    Product = e.Key,
                    Quantity = e.Count(),
                    Stars = e.Average(i => i.SoSaoDanhGia),
                })
                .OrderByDescending(e => e.Stars)
                .ThenByDescending(e => e.Quantity)
                .Take(8)
                .ToListAsync();

            var userId = httpContextAccessor.HttpContext.User.GetUserId();
            var resources = new List<SanPhamResource>();
            foreach (var productQuantity in productQuantities)
            {
                var promotions = await dbContext.SanPhamKhuyenMais
                    .Include(p => p.KhuyenMai)
                    .Where(p => p.MaSanPham == productQuantity.Product.MaSanPham && p.KhuyenMai.TrangThai == PromotionStatus.ACTIVE && p.KhuyenMai.NgayKetThuc >= DateTime.Now.Date)
                    .ToListAsync();

                var promotionResources = promotions.Select(p => applicationMapper.MapToKhuyenMai(p.KhuyenMai)).ToList();
                var productResource = applicationMapper.MapToProductResource(productQuantity.Product);
                productResource.Promotions = promotionResources;
                productResource.Quantity = await dbContext.BienTheSanPhams.CountAsync(p => p.MaSanPham == productQuantity.Product.MaSanPham);
                productResource.HasWishlist = await dbContext.DanhSachYeuThichs
                 .Include(s => s.DanhSachSanPham)
                 .AnyAsync(s => s.MaNguoiDung == userId && s.DanhSachSanPham.Any(t => t.MaSanPham == productQuantity.Product.MaSanPham));
                resources.Add(productResource);
            }

            var response = new DataResponse<List<SanPhamResource>>();
            response.Data = resources;
            response.Message = "Lấy top sản phẩm yêu thích nhất thành công";
            response.StatusCode = HttpStatusCode.OK;
            response.Success = true;

            return response;
        }
    }
}
