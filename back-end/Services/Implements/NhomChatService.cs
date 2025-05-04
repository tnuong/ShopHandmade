using back_end.Core.Models;
using back_end.Services.Interfaces;

namespace back_end.Services.Implements
{
    public class NhomChatService : INhomChatService
    {
        public Task AddToGroup(NhomChat group)
        {
            throw new NotImplementedException();
        }

        public Task<List<NhomChat>> FindAllByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public Task<NhomChat> FindGroupByGroupName(string groupName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateGroup(NhomChat group)
        {
            throw new NotImplementedException();
        }
    }
}
