using TG.ChatBot.Common.ChatHub.Enums;
using TG.ChatBot.Common.ChatHub.Models;
using TG.ChatBot.Common.Common.Enums;
using TG.ChatBot.Common.Common.Helpers;
using TG.ChatBot.Common.Domain.Entities;

namespace TG.ChatBot.Host.Services.Communication
{
    public class ChatHub : IChatHub
    {
        private static ChatHub? instance;
        private readonly ILogger<ChatHub> _logger;
        private readonly IMessaging _messaging;

        private readonly List<User> _usersSearchPool;
        private readonly List<ChatRoom> _chatRoomPool;

        protected ChatHub(ILogger<ChatHub> logger, IMessaging messaging)
        {
            _usersSearchPool = new List<User>();
            _chatRoomPool = new List<ChatRoom>();

            _logger = logger;
            _messaging = messaging;
        }

        public static ChatHub GetInstance(IServiceProvider serviceProvider)
        {
            if (instance == null)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<ChatHub>>();
                var messaging = serviceProvider.GetRequiredService<IMessaging>();
                instance = new ChatHub(logger, messaging);
            }

            return instance;
        }

        public User? AddUserInSearchPool(User user)
        {
            UserHelper.CheckUserValidFields(user);

            var userId = user.UserId;

            if (IsUserInSearchPool(userId))
            {
                _logger.LogWarning("The user is already in the pool");
                return null;
            }

            _usersSearchPool.Add(user);
            _logger.LogInformation("User was successfully added to the search pool");

            return user;
        }

        public User? RemoveUserFromSearchPool(long userId)
        {
            var user = _usersSearchPool.FirstOrDefault(x => x.UserId == userId);

            if (user != null)
            {
                _usersSearchPool.Remove(user);
                _logger.LogInformation("User was successfully removed from the search pool");
            }

            return user;
        }

        public ChatRoom? EndChat(long initiatorId)
        {
            var chatRoom = _chatRoomPool.FirstOrDefault(x => x.FirstUserId == initiatorId || x.SecondUserId == initiatorId);

            if (chatRoom != null)
            {
                _chatRoomPool.Remove(chatRoom);
                chatRoom.InitiatorEndId = initiatorId;
                _logger.LogInformation("One of the chats was completed");
            }

            return chatRoom;
        }

        public ChatRoom? StartChat(User firstUser, User secondUser)
        {
            RemoveUserFromSearchPool(firstUser.UserId);
            RemoveUserFromSearchPool(secondUser.UserId);

            var newChatRoom = new ChatRoom()
            {
                FirstUserId = firstUser.UserId,
                SecondUserId = secondUser.UserId,
                StatusRoom = (int)StatusRoom.Open,
                StartDate = DateTime.Now
            };

            _chatRoomPool.Add(newChatRoom);
            _logger.LogInformation("New chat room launched");

            return newChatRoom;
        }

        public async Task RedirectMessage(string message, long senderId)
        {
            var chatRoom = _chatRoomPool.FirstOrDefault(x => x.FirstUserId == senderId || x.SecondUserId == senderId);

            if (chatRoom != null)
            {
                var recipientId = chatRoom.FirstUserId == senderId ? chatRoom.SecondUserId : chatRoom.FirstUserId;
                await _messaging.SendMessage(message: message, recipient: recipientId);
            }
        }

        public bool IsUserInSearchPool(long userId)
        {
            var user = _usersSearchPool.FirstOrDefault(x => x.UserId == userId);

            return user != null;
        }

        public bool IsUserInChatRoom(long userId)
        {
            var chatRoom = _chatRoomPool.FirstOrDefault(x => x.FirstUserId == userId || x.SecondUserId == userId);

            return chatRoom != null;
        }

        public User? FindInterlocutor(User initiator, bool startRoom = false)
        {
            UserHelper.CheckUserValidFields(initiator);

            var potentialUser = _usersSearchPool.FirstOrDefault(x => CompareUsersCriteria(initiator, x));

            if (potentialUser != null && !IsUserInChatRoom(initiator.UserId) && !IsUserInChatRoom(potentialUser.UserId))
            {
                var potentialUserId = potentialUser.UserId;
                var initiatorUserId = initiator.UserId;

                if (startRoom)
                {
                    StartChat(initiator, potentialUser);
                }
            }

            return potentialUser;
        }

        private bool CompareUsersCriteria(User firstUser, User secondUser)
        {
            // Если это один и тот-же пользователь
            if (firstUser.UserId == secondUser.UserId)
            {
                return false;
            }

            bool IsEligible(User user, User comparableUser)
            {
                var preferredAgeRange = user.UserSetting?.PreferredAge
                    ?.ToEnum<AgeCategory>()
                    ?.GetAgeRange();

                if (preferredAgeRange == null)
                {
                    return false;
                }

                _logger.LogTrace($"Min Age: {preferredAgeRange?.Min}");
                _logger.LogTrace($"Max Age: {preferredAgeRange?.Max}");

                var gender = user.UserSetting?.PreferredGender;

                // Если предпочитаемый пол не соответствует
                if (gender != comparableUser.Gender && gender != (int)Gender.Any)
                {
                    return false;
                }

                // Если предпочитаемый возраст не соответствует
                // Если предпочитаемый минимальный возраст больше, чем возраст собеседника
                // Если предпочитаемый максимальный возраст меньше, чем возраст собеседника
                if (preferredAgeRange?.Min > comparableUser.Age
                    || preferredAgeRange?.Max < comparableUser.Age)
                {
                    return false;
                }

                return true;
            }

            var result = IsEligible(firstUser, secondUser) && IsEligible(secondUser, firstUser);

            return result;
        }
    }
}
