using back_end.Core.Requests;
using back_end.Core.Responses;

namespace back_end.Services.Interfaces
{
    public interface INhaSanXuatService
    {
        public Task<BaseResponse> CreateManufacturer(CreateManufacturerRequest request);
        public Task<BaseResponse> UpdateManufacturer(int id, EditManufacturerRequest request);
        public Task<BaseResponse> DeleteManufacturer(int id);
        Task<BaseResponse> GetManufacturerById(int id);
        Task<BaseResponse> GetAllManufacturers(int pageIndex, int pageSize, string searchString);
    }
}
