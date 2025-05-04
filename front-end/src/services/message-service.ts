import axiosConfig from '../configuration/axiosConfig';
import { DataResponse, MessageResource } from "../resources";

class MessageService {
    getAllMessages(recipientId: string) : Promise<DataResponse<MessageResource[]>> {
        return axiosConfig.get("/api/TinNhan/" + recipientId);
    }
}

const messageService = new MessageService();
export default messageService;