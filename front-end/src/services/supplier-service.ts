import { SupplierRequest } from '../areas/admin/components/modals/CreateSupplierModal';
import { QueryParams } from '../areas/admin/pages/products/VariantManagement';
import axiosConfig from '../configuration/axiosConfig';
import { SupplierResource, DataResponse, PaginationResponse, BaseResponse } from '../resources';

class SupplierService {

    getAllSuppliers(params?: QueryParams) : Promise<PaginationResponse<SupplierResource[]>> {
        const queryString = new URLSearchParams(params as any).toString();
        return axiosConfig.get("/api/NhaCungCap?" + queryString);
    }

    createSupplier(payload: SupplierRequest) : Promise<DataResponse<SupplierResource>> {
        return axiosConfig.post("/api/NhaCungCap", payload);
    }

    updateSupplier(id: number, payload: SupplierRequest) : Promise<BaseResponse> {
        return axiosConfig.put("/api/NhaCungCap/" + id, payload);
    }

    removeSupplier(id: number): Promise<BaseResponse> {
        return axiosConfig.delete("/api/NhaCungCap/" + id);
    }
}

const supplierService = new SupplierService();
export default supplierService;