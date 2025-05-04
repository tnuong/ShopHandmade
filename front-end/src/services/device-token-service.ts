import axiosConfig from '../configuration/axiosConfig';

class DeviceTokenService {
    saveToken(deviceToken: string) : Promise<void> {
        return axiosConfig.post("/api/ThietBiThongBao", { deviceToken });
    }
}

const deviceTokenService = new DeviceTokenService();
export default deviceTokenService;