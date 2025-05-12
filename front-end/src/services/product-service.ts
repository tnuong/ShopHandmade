import { EditProductRequest } from '../areas/admin/components/modals/EditProductModal';
import { RemoveImagesRequest } from '../areas/admin/pages/products/ProductDetail';
import { QueryParams } from '../areas/admin/pages/products/VariantManagement';
import { ProductFilter } from '../areas/customers/pages/ShopPage';
import axiosConfig from '../configuration/axiosConfig';
import { BaseResponse, DataResponse, PaginationResponse, ProductResource } from '../resources';

class ProductService {

    getAllProducts(params?: ProductFilter) : Promise<PaginationResponse<ProductResource[]>> {
        const queryUrls = [];

        if(params?.pageIndex) {
            queryUrls.push(`pageIndex=${params?.pageIndex}`)
        }

        if(params?.pageSize) {
            queryUrls.push(`pageSize=${params?.pageSize}`)
        }

        if(params?.minPrice) {
            queryUrls.push(`giaThapNhat=${params?.minPrice}`)
        }

        if(params?.maxPrice) {
            queryUrls.push(`giaCaoNhat=${params?.maxPrice}`)
        }

        if(params?.brandIds) {
            params.brandIds.forEach(param => {
                queryUrls.push(`thuongHieuIds=${param}`)
            })
        }

        if(params?.categoryIds) {
            params.categoryIds.forEach(param => {
                queryUrls.push(`danhMucIds=${param}`)
            })
        }

        if(params?.sizeIds) {
            params.sizeIds.forEach(param => {
                queryUrls.push(`kichThuocIds=${param}`)
            })
        }

        if(params?.colorIds) {
            params.colorIds.forEach(param => {
                queryUrls.push(`mauSacIds=${param}`)
            })
        }

        if(params?.sortBy && params.sortOrder) {
            queryUrls.push(`sapXepTheo=${params?.sortBy}`)
            queryUrls.push(`thuTuSapXep=${params?.sortOrder}`)
        }

        return axiosConfig.get("/api/SanPham?" + queryUrls.join('&'));
    }

    getProductById(id: string | number) : Promise<DataResponse<ProductResource>> {
        return axiosConfig.get("/api/SanPham/" + id);
    }

    getTopFavoriteProducts() : Promise<DataResponse<ProductResource[]>> {
        return axiosConfig.get('/api/SanPham/top-yeu-thich')
    }

    getTopBestSellerProducts() : Promise<DataResponse<ProductResource[]>> {
        return axiosConfig.get('/api/SanPham/top-ban-chay')
    }

    searchProducts(queryParams?: QueryParams) : Promise<PaginationResponse<ProductResource[]>> {
        const queryString = new URLSearchParams(queryParams as any).toString();
        return axiosConfig.get("/api/SanPham/tim-kiem?" + queryString)
    }

    createProduct(payload: FormData) : Promise<DataResponse<ProductResource>> {
        return axiosConfig.post("/api/SanPham", payload);
    }

    updateProduct(id: number, payload: EditProductRequest) : Promise<void> {
        return axiosConfig.put("/api/SanPham/" + id, payload);
    }

    uploadThumbnail(id: number, payload: FormData) : Promise<void> {
        return axiosConfig.put("/api/SanPham/upload/hinh-dai-dien/" + id, payload);
    }

    uploadZoomImage(id: number, payload: FormData) : Promise<void> {
        return axiosConfig.put("/api/SanPham/upload/hinh-phong-to/" + id, payload);
    }

    uploadImages(id: number, payload: FormData) : Promise<void> {
        return axiosConfig.put("/api/SanPham/upload/hinh-anh/" + id, payload);
    }

    removeImages(payload: RemoveImagesRequest) : Promise<void> {
        return axiosConfig.put("/api/SanPham/xoa-hinh-anh", payload);
    }

    removeProduct(id: number): Promise<void> {
        return axiosConfig.delete("/api/SanPham/" + id);
    }

    addWishlist(productId: number) : Promise<BaseResponse> {
        return axiosConfig.put('/api/SanPham/danh-sach-yeu-thich/' + productId)
    }

    removeWishlist(productId: number) : Promise<BaseResponse> {
        return axiosConfig.delete('/api/SanPham/danh-sach-yeu-thich/' + productId)
    }

    getWishlist(pageIndex: number, pageSize: number) : Promise<PaginationResponse<ProductResource[]>> {
        return axiosConfig.get('/api/SanPham/danh-sach-yeu-thich', {
            params: {
                pageIndex, pageSize
            }
        })
    }
}

const productService = new ProductService();
export default productService;