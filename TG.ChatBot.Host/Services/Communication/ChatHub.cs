using Telegram.Bot;
using TG.ChatBot.Common.ChatHub.Enums;
using TG.ChatBot.Common.ChatHub.Models;
using TG.ChatBot.Common.Common.Enums;
using TG.ChatBot.Common.Common.Helpers;
using TG.ChatBot.Common.Common.Pattern;
using TG.ChatBot.Common.Domain.Entities;

namespace TG.ChatBot.Host.Services.Communication
{
    public class ChatHub : IChatHub
    {
        private static ChatHub? instance;
        private readonly ILogger<ChatHub> _logger;
        private readonly ITelegramBotClient _botClient;

        private readonly List<User> _usersSearchPool;
        private readonly List<ManagerMediator> _chatRooms;

        protected ChatHub(ILogger<ChatHub> logger, ITelegramBotClient botClient)
        {
            _usersSearchPool = new List<User>();
            _chatRooms = new List<ManagerMediator>();

            _logger = logger;
            _botClient = botClient;
        }

        public static ChatHub GetInstance(IServiceProvider serviceProvider)
        {
            if (instance == null)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<ChatHub>>();
                var botClient = serviceProvider.GetRequiredService<ITelegramBotClient>();
                instance = new ChatHub(logger, botClient);
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

        public ManagerMediator? EndChat(long initiatorId)
        {
            var chatRoom = _chatRooms.FirstOrDefault(x => x.FirstUser.Info.UserId == initiatorId || x.SecondUser.Info.UserId == initiatorId);

            if (chatRoom != null)
            {
                _chatRooms.Remove(chatRoom);
                //chatRoom.InitiatorEndId = initiatorId;
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

            var mediator = new ManagerMediator();
            var firstInterlocutor = new Interlocutor(mediator, firstUser, _botClient);
            var secondInterlocutor = new Interlocutor(mediator, secondUser, _botClient);
            mediator.FirstUser = firstInterlocutor;
            mediator.SecondUser = secondInterlocutor;

            _chatRooms.Add(mediator);
            _logger.LogInformation("New chat room launched");

            return newChatRoom;
        }

        public async Task RedirectMessage(string message, long senderId)
        {
            var chatRoom = _chatRooms.FirstOrDefault(x => x.FirstUser.Info.UserId == senderId || x.SecondUser.Info.UserId == senderId);

            if (chatRoom != null)
            {
                await chatRoom.Send(message, senderId);
            }
        }

        public bool IsUserInSearchPool(long userId)
        {
            var user = _usersSearchPool.FirstOrDefault(x => x.UserId == userId);

            return user != null;
        }

        public bool IsUserInChatRoom(long userId)
        {
            var chatRoom = _chatRooms.FirstOrDefault(x => x.FirstUser.Info.UserId == userId || x.SecondUser.Info.UserId == userId);

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
