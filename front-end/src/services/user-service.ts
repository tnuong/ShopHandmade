
import { AccountRequest } from '../areas/admin/components/modals/CreateAccountModal';
import { EditAccountRequest } from '../areas/admin/components/modals/EditAccountModal';
import { QueryParams } from '../areas/admin/pages/products/VariantManagement';
import { ChangePasswordRequest, ProfileRequest } from '../areas/customers/pages/ProfilePage';
import { ResetPasswordRequest, ValidateTokenRequest } from '../areas/customers/pages/ResetPassword';
import axiosConfig from '../configuration/axiosConfig';
import { BaseResponse, DataResponse, PaginationResponse, UserContactResource, UserResource } from "../resources";

class AccountService {

    createAccount(payload: AccountRequest) : Promise<BaseResponse> {
        return axiosConfig.post('/api/TaiKhoan/tao-tai-khoan', payload)
    }

    editAccount(id: string, payload: EditAccountRequest) : Promise<BaseResponse> {
        return axiosConfig.put('/api/TaiKhoan/cap-nhat-tai-khoan/' + id, payload)
    }

    getAllAccounts(queryParams?: QueryParams): Promise<PaginationResponse<UserResource[]>> {
        const queryString = new URLSearchParams(queryParams as any).toString()
        return axiosConfig.get('/api/TaiKhoan?' + queryString);
    }

    getUserDetails() : Promise<DataResponse<UserResource>> {
        return axiosConfig.get("/api/TaiKhoan/user-dang-nhap");
    }

    getUserById(id: string) : Promise<DataResponse<UserResource>> {
        return axiosConfig.get('/api/TaiKhoan/' + id)
    }

    getAllExceptLoggedInUser() : Promise<DataResponse<UserContactResource[]>> {
        return axiosConfig.get("/api/TaiKhoan/ngoai-tru-user-dang-nhap");
    }

    getAllAdmins() : Promise<DataResponse<UserContactResource[]>> {
        return axiosConfig.get("/api/TaiKhoan/quan-tri-vien");
    }

    changePassword(payload: ChangePasswordRequest) : Promise<BaseResponse> {
        return axiosConfig.post("/api/TaiKhoan/doi-mat-khau", payload)
    }

    requestResetPassword(email: string) : Promise<BaseResponse> {
        return axiosConfig.get("/api/TaiKhoan/yeu-cau-khoi-phuc-mat-khau?email=" + email)
    }

    resetPassword(payload: ResetPasswordRequest) : Promise<BaseResponse> {
        return axiosConfig.post("/api/TaiKhoan/khoi-phuc-mat-khau", payload)
    }

    validateToken(payload: ValidateTokenRequest) : Promise<BaseResponse> {
        return axiosConfig.post("/api/TaiKhoan/xac-thuc-token", payload)
    }

    updateProfile(payload: ProfileRequest) : Promise<DataResponse<UserResource>> {
        return axiosConfig.post("/api/TaiKhoan/cap-nhat-ho-so", payload);
    }

    uploadAvatar(payload: FormData) : Promise<DataResponse<UserResource>> {
        return axiosConfig.post("/api/TaiKhoan/cap-nhat-hinh-dai-dien", payload);
    }

    uploadCoverImage(payload: FormData) : Promise<DataResponse<UserResource>> {
        return axiosConfig.post("/api/TaiKhoan/cap-nhat-hinh-bia", payload);
    }

    lockAccount(userId: string) : Promise<BaseResponse> {
        return axiosConfig.put('/api/TaiKhoan/khoa/' + userId)
    }

    unlock(userId: string) : Promise<BaseResponse> {
        return axiosConfig.put('/api/TaiKhoan/mo-khoa/' + userId)
    }
}

const accountService = new AccountService();
export default accountService;