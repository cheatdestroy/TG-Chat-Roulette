using Telegram.Bot.Types.InputFiles;
using TG.ChatBot.Common.Domain.Entities;

namespace TG.ChatBot.Common.ChatHub.Enums
{
    public interface IChatRoomManager
    {
        Task<ChatRoom?> CreateChatRoom(User firstUser, User secondUser);
        Task<ChatRoom?> RemoveChatRoom(long userId);
        ChatRoom? IncrementMessagesCounter(long userId);
        ChatRoom? GetChatRoomByUserId(long userId);
        User? GetInterlocutor(long userId);
    }
}
