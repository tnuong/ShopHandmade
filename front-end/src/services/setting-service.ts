import { BaseResponse, DataResponse, ReviewShowResource, SlideShowResource } from "../resources";
import axiosConfig from '../configuration/axiosConfig';
import { ReviewShowRequest } from "../areas/admin/components/modals/CreateReviewShowModal";

class SettingService {
    getAllSlideShows() : Promise<DataResponse<SlideShowResource[]>> {
        return axiosConfig.get('/api/CaiDat/HienThiBanner');
    }

    getAllReviewShows() : Promise<DataResponse<ReviewShowResource[]>> {
        return axiosConfig.get('/api/CaiDat/HienThiDanhGia');
    }

    createSlideShow(payload: FormData) : Promise<BaseResponse> {
        return axiosConfig.post('/api/CaiDat/HienThiBanner', payload)
    }

    editSlideShow(id: number | string, payload: FormData) : Promise<void> {
        return axiosConfig.put('/api/CaiDat/HienThiBanner/'+ id, payload)
    }

    removeSlideShow(id: number | string) : Promise<void> {
        return axiosConfig.delete('/api/CaiDat/HienThiBanner/' + id)
    }

    createReviewShow(payload: ReviewShowRequest) : Promise<BaseResponse> {
        return axiosConfig.post('/api/CaiDat/HienThiDanhGia', payload)
    }

    removeReviewShow(id: number | string) : Promise<void> {
        return axiosConfig.delete('/api/CaiDat/HienThiDanhGia/' + id)
    }
}

const settingService = new SettingService();
export default settingService;