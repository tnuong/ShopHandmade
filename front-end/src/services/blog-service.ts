import { BaseResponse, BlogResource, DataResponse } from "../resources";
import axiosConfig from '../configuration/axiosConfig';

class BlogService {
    createBlog(payload: FormData) : Promise<BaseResponse> {
        return axiosConfig.post('/api/BaiViet', payload);
    }

    getAllBlogs(): Promise<DataResponse<BlogResource[]>> {
        return axiosConfig.get('/api/BaiViet')
    }

    getBlogById(id: number | string) : Promise<DataResponse<BlogResource>> {
        return axiosConfig.get('/api/BaiViet/' + id)
    }

    updateBlog(id: number | string, payload: FormData) : Promise<void> {
        return axiosConfig.put('/api/BaiViet/' + id, payload)
    }

    getAllBlogExceptBlogId(id: number | string) : Promise<DataResponse<BlogResource[]>>  {
        return axiosConfig.get('/api/BaiViet/ngoai-tru/' + id)
    }

    getAllBlogRelatedUserId(userId: number | string, blogId: number | string) : Promise<DataResponse<BlogResource[]>>  {
        return axiosConfig.get('/api/BaiViet/lien-quan/' + userId + "/" + blogId)
    }

    hiddenBlog(id: number | string) : Promise<void> {
        return axiosConfig.put('/api/BaiViet/bai-viet-an/' + id)
    }

    showBlog(id: number | string) : Promise<void> {
        return axiosConfig.put('/api/BaiViet/hien-thi/' + id)
    }

    deleteBlog(id: number | string) : Promise<void> {
        return axiosConfig.delete('/api/BaiViet/' + id)
    }
}

const blogService = new BlogService();
export default blogService;