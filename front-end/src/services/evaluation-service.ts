import { EvaluationRequest } from '../areas/customers/components/OrderItem';
import axiosConfig from '../configuration/axiosConfig';
import { BaseResponse, DataResponse, EvaluationResource, PaginationResponse, ReportEvaluationResource } from "../resources";

class EvaluationService {

    getAllByProductId(productId: number | string) : Promise<PaginationResponse<ReportEvaluationResource>> {
        return axiosConfig.get("/api/DanhGia/" + productId);
    }

    getAllExcellentEvaluations() : Promise<DataResponse<EvaluationResource[]>> {
        return axiosConfig.get('/api/DanhGia/xuat-sac')
    }

    createEvaluation(payload: EvaluationRequest) : Promise<BaseResponse> {
        return axiosConfig.post("/api/DanhGia", payload);
    }

    interactEvaluation(id: number): Promise<void> {
        return axiosConfig.put("/api/DanhGia/" + id)
    }
}

const evaluationService = new EvaluationService();
export default evaluationService;