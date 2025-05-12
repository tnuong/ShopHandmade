using back_end.Core.Responses;

namespace back_end.Services.Interfaces
{
    public interface ISanPhamYeuThichService
    {
        Task<BaseResponse> AddSanPhamYeuThich(int maSanPham);
        Task<BaseResponse> RemoveSanPhamYeuThich(int maSanPham);
        Task<BaseResponse> GetAllSanPham(int pageIndex, int pageSize);
    }
}
