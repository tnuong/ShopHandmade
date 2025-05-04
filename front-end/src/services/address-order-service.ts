import { CreateAddressOrderRequest } from '../areas/customers/components/modals/CreateAddressOrderModal';
import axiosConfig from '../configuration/axiosConfig';
import { AddressOrderResource, BaseResponse, DataResponse } from '../resources';

class AddressOrderService {

    getAllAddressOrders() : Promise<DataResponse<AddressOrderResource[]>> {
        return axiosConfig.get("/api/DiaChiGiaoHang");
    }

    createAddressOrder(payload: CreateAddressOrderRequest) : Promise<DataResponse<AddressOrderResource>> {
        return axiosConfig.post("/api/DiaChiGiaoHang", payload);
    }

    setStatusAddressOrder(id: number) : Promise<BaseResponse> {
        return axiosConfig.put("/api/DiaChiGiaoHang/" + id);
    }

}

const addressOrderService = new AddressOrderService();
export default addressOrderService;