import { EditVariantRequest } from '../areas/admin/components/modals/EditVariantModal';
import { RemoveImagesRequest } from '../areas/admin/pages/products/VariantDetails';
import { QueryParams } from '../areas/admin/pages/products/VariantManagement';
import axiosConfig from '../configuration/axiosConfig';
import { DataResponse, PaginationResponse, VariantResource } from '../resources';

class VariantService {

    getAllVariants(params?: QueryParams): Promise<PaginationResponse<VariantResource[]>> {
        const queryString = new URLSearchParams(params as any).toString();
        return axiosConfig.get(`/api/BienTheSanPham?${queryString}`);
    }

    getVariantById(id: number | string): Promise<DataResponse<VariantResource>> {
        return axiosConfig.get("/api/BienTheSanPham/" + id);
    }

    getAllVariantsByProductId(productId: number | string, params?: QueryParams): Promise<PaginationResponse<VariantResource[]>> {
        const queryString = new URLSearchParams(params as any).toString();
        return axiosConfig.get("/api/BienTheSanPham/san-pham/" + productId + '/?' + queryString);
    }

    getAllVariantsByProductIdAndColorId(productId: number | string, colorId: number | string): Promise<DataResponse<VariantResource[]>> {
        return axiosConfig.get("/api/BienTheSanPham/san-pham/" + productId + "/" + colorId);
    }

    getUniqueColorVariantsByProductId(productId: number | string): Promise<DataResponse<VariantResource[]>> {
        return axiosConfig.get("/api/BienTheSanPham/san-pham-mau-sac-duy-nhat/" + productId);
    }

    getUniqueSizeVariantsByProductId(productId: number | string): Promise<DataResponse<VariantResource[]>> {
        return axiosConfig.get("/api/BienTheSanPham/san-pham-kich-thuoc-duy-nhat/" + productId);
    }

    createVariant(payload: FormData): Promise<DataResponse<VariantResource>> {
        return axiosConfig.post("/api/BienTheSanPham", payload);
    }

    updateVariant(id: number, payload: EditVariantRequest): Promise<void> {
        return axiosConfig.put("/api/BienTheSanPham/" + id, payload);
    }

    uploadThumbnail(id: number, payload: FormData): Promise<void> {
        return axiosConfig.put("/api/BienTheSanPham/upload/hinh-dai-dien/" + id, payload);
    }

    uploadImages(id: number, payload: FormData): Promise<void> {
        return axiosConfig.put("/api/BienTheSanPham/upload/hinh-anh/" + id, payload);
    }

    removeImages(payload: RemoveImagesRequest): Promise<void> {
        return axiosConfig.put("/api/BienTheSanPham/xoa-hinh-anh", payload);
    }

    removeVariant(id: number): Promise<void> {
        return axiosConfig.delete("/api/BienTheSanPham/" + id);
    }
}

const variantService = new VariantService();
export default variantService;