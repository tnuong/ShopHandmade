import { CheckoutType } from '../areas/customers/pages/CheckoutPage';
import axiosConfig from '../configuration/axiosConfig';
import { DataResponse } from "../resources";

class PaymentService {

    createVnpayPaymentUrl(payload: CheckoutType) : Promise<DataResponse<string>> {
        return axiosConfig.post("/api/ThanhToan/tao-thanh-toan-vnpay", payload);
    }

    payPalCapture(orderId: string, payload: CheckoutType) : Promise<DataResponse<string>> {
        return axiosConfig.post("/api/ThanhToan/capture/" + orderId, payload);
    }

}

const paymentService = new PaymentService();
export default paymentService;