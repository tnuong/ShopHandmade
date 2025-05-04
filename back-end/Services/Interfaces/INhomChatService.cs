using back_end.Core.Models;

namespace back_end.Services.Interfaces
{
    public interface INhomChatService
    {
        Task AddToGroup(NhomChat group);
        Task<NhomChat> FindGroupByGroupName(string groupName);
        Task<List<NhomChat>> FindAllByUsername(string username);
        Task UpdateGroup(NhomChat group);
    }
}
