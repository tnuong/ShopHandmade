import { BrandRequest } from '../areas/admin/components/modals/CreateBrandModal';
import { QueryParams } from '../areas/admin/pages/products/VariantManagement';
import axiosConfig from '../configuration/axiosConfig';
import { BrandResource, DataResponse, PaginationResponse } from '../resources';

class BrandService {

    getAllBrands(params?: QueryParams) : Promise<PaginationResponse<BrandResource[]>> {
        const queryString = new URLSearchParams(params as any).toString();
        return axiosConfig.get("/api/ThuongHieu?" + queryString);
    }

    createBrand(payload: BrandRequest) : Promise<DataResponse<BrandResource>> {
        return axiosConfig.post("/api/ThuongHieu", payload);
    }

    updateBrand(id: number, payload: BrandRequest) : Promise<void> {
        return axiosConfig.put("/api/ThuongHieu/" + id, payload);
    }

    removeBrand(id: number): Promise<void> {
        return axiosConfig.delete("/api/ThuongHieu/" + id);
    }
}

const brandService = new BrandService();
export default brandService;