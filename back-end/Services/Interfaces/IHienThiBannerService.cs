using back_end.Core.Requests;
using back_end.Core.Responses;

namespace back_end.Services.Interfaces
{
    public interface IHienThiBannerService
    {
        public Task<BaseResponse> GetAllSlideShows();
        public Task<BaseResponse> CreateSlideShow(CreateSlideShowRequest request);
        public Task EditSlideShow(int id, EditSlideShowRequest request);
        public Task RemoveSlideShow(int id);
    }
}
