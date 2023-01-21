using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TG.ChatBot.Common.ChatHub.Enums;
using TG.ChatBot.Common.ChatHub.Models;
using TG.ChatBot.Common.Common.Enums;
using TG.ChatBot.Common.Common.Helpers;
using TG.ChatBot.Common.Domain.Entities;
using User = TG.ChatBot.Common.Domain.Entities.User;

namespace TG.ChatBot.Host.Services.Communication
{
    public class ChatHub : IChatHub
    {
        private readonly ILogger<ChatHub> _logger;

        private static List<User> _usersSearchPool = new List<User>();
        private readonly IChatRoomManager _chatRoomManager;
        private readonly IMessaging _messaging;

        public ChatHub(ILogger<ChatHub> logger, IMessaging messaging, IChatRoomManager chatRoomManager)
        {
            _messaging = messaging;
            _chatRoomManager = chatRoomManager;

            _logger = logger;
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

        public async Task<ChatRoom?> EndChat(long initiatorId)
        {
            var removedChatRoom = await _chatRoomManager.RemoveChatRoom(initiatorId);
            _logger.LogInformation("One of the chats was completed");

            return removedChatRoom;
        }

        public async Task<ChatRoom?> StartChat(User firstUser, User secondUser)
        {
            RemoveUserFromSearchPool(firstUser.UserId);
            RemoveUserFromSearchPool(secondUser.UserId);

            var chatRoom = await _chatRoomManager.CreateChatRoom(firstUser, secondUser);
            _logger.LogInformation("New chat room launched");

            return chatRoom;
        }

        public async Task RedirectMessage(Message message, long senderId)
        {
            var recipient = _chatRoomManager.GetInterlocutor(senderId);

            if (recipient != null)
            {
                await _messaging.SendMessage(message, recipient.UserId);
                _chatRoomManager.IncrementMessagesCounter(senderId);
            }
        }

        public bool IsUserInSearchPool(long userId)
        {
            var user = _usersSearchPool.FirstOrDefault(x => x.UserId == userId);

            return user != null;
        }

        public bool IsUserInChatRoom(long userId)
        {
            var chatRoom = _chatRoomManager.GetChatRoomByUserId(userId);

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
