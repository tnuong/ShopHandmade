import { SizeRequest } from '../areas/admin/components/modals/CreateSizeModal';
import axiosConfig from '../configuration/axiosConfig';
import { DataResponse, SizeResource } from "../resources";

class SizeService {

    getAllSizes() : Promise<DataResponse<SizeResource[]>> {
        return axiosConfig.get("/api/KichThuoc");
    }

    createSize(payload: SizeRequest) : Promise<DataResponse<SizeResource>> {
        return axiosConfig.post("/api/KichThuoc", payload);
    }

    updateSize(id: number, payload: SizeRequest) : Promise<void> {
        return axiosConfig.put("/api/KichThuoc/" + id, payload);
    }

    removeSize(id: number): Promise<void> {
        return axiosConfig.delete("/api/KichThuoc/" + id);
    }
}

const sizeService = new SizeService();
export default sizeService;