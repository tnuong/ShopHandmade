import { ManufacturerRequest } from '../areas/admin/components/modals/CreateManufacturerModal';
import { QueryParams } from '../areas/admin/pages/products/VariantManagement';
import axiosConfig from '../configuration/axiosConfig';
import { ManufacturerResource, DataResponse, PaginationResponse, BaseResponse } from '../resources';

class ManufacturerService {

    getAllManufacturers(params?: QueryParams) : Promise<PaginationResponse<ManufacturerResource[]>> {
        const queryString = new URLSearchParams(params as any).toString();
        return axiosConfig.get("/api/NhaSanXuat?" + queryString);
    }

    createManufacturer(payload: ManufacturerRequest) : Promise<DataResponse<ManufacturerResource>> {
        return axiosConfig.post("/api/NhaSanXuat", payload);
    }

    updateManufacturer(id: number, payload: ManufacturerRequest) : Promise<BaseResponse> {
        return axiosConfig.put("/api/NhaSanXuat/" + id, payload);
    }

    removeManufacturer(id: number): Promise<BaseResponse> {
        return axiosConfig.delete("/api/NhaSanXuat/" + id);
    }
}

const manufacturerService = new ManufacturerService();
export default manufacturerService;