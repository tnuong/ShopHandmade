import { CreateOrderPaypal } from '../areas/customers/components/modals/PaymentMethodModal';
import { CheckoutType } from '../areas/customers/pages/CheckoutPage';
import { OrderFilter } from '../areas/customers/pages/OrdersPage';
import axiosConfig from '../configuration/axiosConfig';
import { DataResponse, OrderResource, PaginationResponse } from "../resources";
import { CreateOrderResponse } from '../resources/paypal';

class OrderService {

    creaeteOrder(payload: CheckoutType): Promise<DataResponse<OrderResource>> {
        return axiosConfig.post("/api/DonHang", payload);
    }

    creaeteOrderWithPaypal(payload: CreateOrderPaypal): Promise<CreateOrderResponse> {
        return axiosConfig.post("/api/DonHang/paypal", payload);
    }

    getOrderById(id: number | string | undefined): Promise<DataResponse<OrderResource>> {
        return axiosConfig.get("/api/DonHang/" + id);
    }

    getAllOrdersByUser(params?: OrderFilter): Promise<PaginationResponse<OrderResource[]>> {
        const queryUrls = [];

        if (params?.pageIndex) {
            queryUrls.push(`pageIndex=${params?.pageIndex}`)
        }

        if (params?.pageSize) {
            queryUrls.push(`pageSize=${params?.pageSize}`)
        }

        if (params?.status) {
            queryUrls.push(`trangThai=${params?.status}`)
        }

        return axiosConfig.get("/api/DonHang/nguoi-dung?" + queryUrls.join('&'));
    }

    getAllOrders(params?: OrderFilter): Promise<PaginationResponse<OrderResource[]>> {
        const queryString = new URLSearchParams(params as any).toString();
        return axiosConfig.get("/api/DonHang?" + queryString);
    }

    updateConfirmed(id: number): Promise<void> {
        return axiosConfig.put(`/api/DonHang/cap-nhat/${id}/xac-nhan`);
    }

    updateCancelled(id: number): Promise<void> {
        return axiosConfig.put(`/api/DonHang/cap-nhat/${id}/huy-don-hang`);
    }

    updateCompleted(id: number): Promise<void> {
        return axiosConfig.put(`/api/DonHang/cap-nhat/${id}/hoan-thanh`);
    }

    updateRejected(id: number): Promise<void> {
        return axiosConfig.put(`/api/DonHang/cap-nhat/${id}/tu-choi`);
    }

    updateDelivering(id: number): Promise<void> {
        return axiosConfig.put(`/api/DonHang/cap-nhat/${id}/van-chuyen`);
    }

    updateDelivered(id: number): Promise<void> {
        return axiosConfig.put(`/api/DonHang/cap-nhat/${id}/da-giao`);
    }
}

const orderService = new OrderService();
export default orderService;