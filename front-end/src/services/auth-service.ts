import { GoogleAuthorizeType, SignInRequest } from '../areas/customers/components/modals/LoginModal';
import { SignUpRequest } from '../areas/customers/components/modals/RegisterModal';
import axiosConfig from '../configuration/axiosConfig';
import { AuthResponse, BaseResponse, DataResponse } from '../resources';

class AuthService {

    signIn(payload: SignInRequest) : Promise<DataResponse<AuthResponse>> {
        return axiosConfig.post("/api/XacThuc/dang-nhap", payload);
    }

    signUp(payload: SignUpRequest) : Promise<BaseResponse> {
        return axiosConfig.post("/api/XacThuc/dang-ki", payload)
    }

    googleAuthorize(payload: GoogleAuthorizeType) : Promise<DataResponse<AuthResponse>> {
        return axiosConfig.post("/api/XacThuc/google/dang-nhap", payload);
    }

}

const authService = new AuthService();
export default authService;