import { ColorRequest } from '../areas/admin/components/modals/CreateColorModal';
import axiosConfig from '../configuration/axiosConfig';
import { ColorResource, DataResponse } from "../resources";

class ColorService {

    getAllColors() : Promise<DataResponse<ColorResource[]>> {
        return axiosConfig.get("/api/MauSac");
    }

    createColor(payload: ColorRequest) : Promise<DataResponse<ColorResource>> {
        return axiosConfig.post("/api/MauSac", payload);
    }

    updateColor(id: number, payload: ColorRequest) : Promise<void> {
        return axiosConfig.put("/api/MauSac/" + id, payload);
    }

    removeColor(id: number): Promise<void> {
        return axiosConfig.delete("/api/MauSac/" + id);
    }
}

const colorService = new ColorService();
export default colorService;