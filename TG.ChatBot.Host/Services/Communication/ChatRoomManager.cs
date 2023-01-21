using Telegram.Bot;
using TG.ChatBot.Common.ChatHub.Enums;
using TG.ChatBot.Common.Domain.Entities;
using TG.ChatBot.Common.Domain.Repository.Interfaces;

namespace TG.ChatBot.Host.Services.Communication
{
    public class ChatRoomManager : IChatRoomManager
    {
        private readonly IChatRoom _chatRoom;
        private static List<ChatRoom> _chatRooms = new List<ChatRoom>();

        public ChatRoomManager(IServiceProvider serviceProvider)
        {
            _chatRoom = serviceProvider.GetRequiredService<IChatRoom>();
        }

        public async Task<ChatRoom?> CreateChatRoom(User firstUser, User secondUser)
        {
            var chatRoom = new ChatRoom()
            {
                FirstUserId = firstUser.UserId,
                SecondUserId = secondUser.UserId,
                StatusRoom = (int)StatusRoom.Open,
                StartDate = DateTime.Now
            };

            var result = await _chatRoom.SaveChatRoom(chatRoom);

            if (result != null)
            {
                _chatRooms.Add(result);
            }

            return result;
        }

        public ChatRoom? IncrementMessagesCounter(long userId)
        {
            var chatRoom = GetChatRoomByUserId(userId);

            if (chatRoom != null)
            {
                if (chatRoom.SecondUserId == userId)
                {
                    chatRoom.NumberMessagesSecondUser++;
                }
                else if (chatRoom.FirstUserId == userId)
                {
                    chatRoom.NumberMessagesFirstUser++;
                }
            }

            return chatRoom;
        }

        public async Task<ChatRoom?> RemoveChatRoom(long userId)
        {
            var chatRoom = GetChatRoomByUserId(userId);

            if (chatRoom != null)
            {
                _chatRooms.Remove(chatRoom);

                chatRoom.InitiatorEndId = userId;
                chatRoom.StatusRoom = (int)StatusRoom.Closed;
                chatRoom.EndDate = DateTime.Now;

                var updatedChatRoom = await _chatRoom.SaveChatRoom(chatRoom);

                return updatedChatRoom;
            }

            return chatRoom;
        }

        public User? GetInterlocutor(long userId)
        {
            var chatRoom = GetChatRoomByUserId(userId);

            if (chatRoom != null)
            {
                var interlocutor = chatRoom.FirstUserId == userId ? chatRoom.SecondUser : chatRoom.FirstUser;

                return interlocutor;
            }

            return null;
        }

        public ChatRoom? GetChatRoomByUserId(long userId)
        {
            var chatRoom = _chatRooms.FirstOrDefault(x => x.FirstUserId == userId || x.SecondUserId == userId);
            return chatRoom;
        }
    }
}
