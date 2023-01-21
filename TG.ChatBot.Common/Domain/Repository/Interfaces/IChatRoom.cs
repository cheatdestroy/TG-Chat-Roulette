using TG.ChatBot.Common.ChatHub.Enums;
using TG.ChatBot.Common.Domain.Entities;

namespace TG.ChatBot.Common.Domain.Repository.Interfaces
{
    public interface IChatRoom
    {
        Task<IEnumerable<ChatRoom>> Get(
            Guid? roomId = null,
            long? firstUserId = null,
            long? secondUserId = null,
            StatusRoom? status = null,
            int? offset = null,
            int? limit = null);

        Task<ChatRoom?> SaveChatRoom(ChatRoom chatRoom);
    }
}
