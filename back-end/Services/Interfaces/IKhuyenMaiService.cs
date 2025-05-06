using back_end.Core.Requests;
using back_end.Core.Responses;

namespace back_end.Services.Interfaces
{
    public interface IKhuyenMaiService
    {
        Task<BaseResponse> CreatePromotion(PromotionRequest request);
        Task<BaseResponse> DeletePromotion(int promotionId);
        Task<BaseResponse> UpdatePromotion(int promotionId, PromotionRequest request);
        Task<BaseResponse> GetAllPromotions(int pageIndex, int pageSize, string searchString);
        Task<BaseResponse> GetAllPromotions();
        Task<BaseResponse> GetPromotionById(int id);
        Task<BaseResponse> AssignPromotion(AssignPromotionRequest request);
    }
}
