using back_end.Core.DTOs;
using back_end.Core.Responses;
using back_end.Core.Responses.Resources;

namespace back_end.Services.Interfaces
{
    public interface ITinNhanService
    {
        Task<TinNhanResource> CreateNewMessage(MessageDTO messageDTO);
        Task<BaseResponse> GetAllMessages(string recipientId);
    }
}
