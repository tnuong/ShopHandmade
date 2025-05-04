using back_end.Core.Requests;
using back_end.Core.Responses;

namespace back_end.Services.Interfaces
{
    public interface INhaCungCapService
    {
        public Task<BaseResponse> CreateSupplier(CreateSupplierRequest request);
        public Task<BaseResponse> UpdateSupplier(int id, EditSupplierRequest request);
        public Task<BaseResponse> DeleteSupplier(int id);
        Task<BaseResponse> GetSupplierById(int id);
        Task<BaseResponse> GetAllSuppliers(int pageIndex, int pageSize, string searchString);
    }
}
