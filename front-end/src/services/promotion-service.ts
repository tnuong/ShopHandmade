import { assign } from 'markdown-it/lib/common/utils.mjs';
import { BrandRequest } from '../areas/admin/components/modals/CreateBrandModal';
import { PromotionRequest } from '../areas/admin/components/modals/CreatePromotionModal';
import { QueryParams } from '../areas/admin/pages/products/VariantManagement';
import axiosConfig from '../configuration/axiosConfig';
import { BaseResponse, DataResponse, PaginationResponse, PromotionResource } from '../resources';

class PromotionService {

    getAllPromotions(params?: QueryParams) : Promise<PaginationResponse<PromotionResource[]>> {
        const queryString = new URLSearchParams(params as any).toString();
        return axiosConfig.get("/api/KhuyenMai?" + queryString);
    }

    getAllPromotionsNonPagination() : Promise<DataResponse<PromotionResource[]>> {
        return axiosConfig.get("/api/KhuyenMai/hoat-dong");
    }

    createPromotion(payload: PromotionRequest) : Promise<DataResponse<PromotionResource>> {
        return axiosConfig.post("/api/KhuyenMai", payload);
    }

    updatePromotion(id: number, payload: BrandRequest) : Promise<BaseResponse> {
        return axiosConfig.put("/api/KhuyenMai/" + id, payload);
    }

    removePromotion(id: number): Promise<BaseResponse> {
        return axiosConfig.delete("/api/KhuyenMai/" + id);
    }

    removeProductPromotion(productId: number, promotionId: number): Promise<BaseResponse> {
        return axiosConfig.delete("/api/KhuyenMai/" + productId + '/' + promotionId);
    }

    assignPromotionToProducts(promotionIds: number[], productIds: number[]) : Promise<BaseResponse> {
        return axiosConfig.post("/api/KhuyenMai/ap-dung", {
            productIds,
            promotionIds
        });
    }
}

const promotionService = new PromotionService();
export default promotionService;