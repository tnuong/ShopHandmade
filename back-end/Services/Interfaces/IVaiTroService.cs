using back_end.Core.Responses;

namespace back_end.Services.Interfaces
{
    public interface IVaiTroService
    {
        Task<BaseResponse> GetAllRoles();
    }
}
