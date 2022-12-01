using Microsoft.Extensions.Logging;
using NLog;
using TG.ChatBot.Common.Common.Enums;
using TG.ChatBot.Common.Domain.Entities;
using TG.ChatBot.Host.Services.Communication;

namespace TG.ChatBot.Tests
{
    [TestClass]
    public class ChatHubTest
    {
        [TestMethod]
        public void FindInterlocutorIsNotSuccessTest()
        {
            var chatHub = new ChatHub(GetLogger<ChatHub>(), null);

            var firstUser = new User()
            {
                UserId = 123,
                Age = 15,
                Gender = (int)Gender.Male,
                UserSetting = new UserSetting()
                {
                    UserId = 123,
                    PreferredAge = (int)AgeCategory.LessThanEighteen,
                    PreferredChatType = (int)CommunicationType.Standart,
                    PreferredGender = (int)Gender.Female,
                },
                CreatedAt = DateTime.Now,
            };

            var secondUser = new User()
            {
                UserId = 457,
                Age = 28,
                Gender = (int)Gender.Female,
                UserSetting = new UserSetting()
                {
                    UserId = 457,
                    PreferredAge = (int)AgeCategory.LessThanEighteen,
                    PreferredChatType = (int)CommunicationType.Standart,
                    PreferredGender = (int)Gender.Male,
                },
                CreatedAt = DateTime.Now,
            };

            chatHub.AddUserInSearchPool(firstUser);
            var resultForFirstUser = chatHub.FindInterlocutor(firstUser);

            chatHub.AddUserInSearchPool(secondUser);
            var resultForSecondUser = chatHub.FindInterlocutor(secondUser);

            Assert.IsNull(resultForFirstUser);
            Assert.IsNull(resultForSecondUser);
        }

        [TestMethod]
        public void FindInterlocutorSuccessTest()
        {
            var chatHub = new ChatHub(GetLogger<ChatHub>(), null);

            var firstUser = new User()
            {
                UserId = 123,
                Age = 15,
                Gender = (int)Gender.Male,
                UserSetting = new UserSetting()
                {
                    UserId = 123,
                    PreferredAge = (int)AgeCategory.LessThanEighteen,
                    PreferredChatType = (int)CommunicationType.Standart,
                    PreferredGender = (int)Gender.Female,
                },
                CreatedAt = DateTime.Now,
            };

            var secondUser = new User()
            {
                UserId = 457,
                Age = 16,
                Gender = (int)Gender.Female,
                UserSetting = new UserSetting()
                {
                    UserId = 457,
                    PreferredAge = (int)AgeCategory.LessThanEighteen,
                    PreferredChatType = (int)CommunicationType.Standart,
                    PreferredGender = (int)Gender.Male,
                },
                CreatedAt = DateTime.Now,
            };

            chatHub.AddUserInSearchPool(firstUser);
            var resultForFirstUser = chatHub.FindInterlocutor(firstUser);

            chatHub.AddUserInSearchPool(secondUser);
            var resultForSecondUser = chatHub.FindInterlocutor(secondUser);

            Assert.IsNull(resultForFirstUser);
            Assert.IsNotNull(resultForSecondUser);
        }

        [TestMethod]
        public void RemoveUserFromSearchPoolTest()
        {
            var chatHub = new ChatHub(GetLogger<ChatHub>(), null);

            var user = new User()
            {
                UserId = 123,
                Age = 15,
                Gender = (int)Gender.Male,
                UserSetting = new UserSetting()
                {
                    UserId = 123,
                    PreferredAge = (int)AgeCategory.LessThanEighteen,
                    PreferredChatType = (int)CommunicationType.Standart,
                    PreferredGender = (int)Gender.Male,
                },
                CreatedAt = DateTime.Now,
            };

            chatHub.AddUserInSearchPool(user);
            var result = chatHub.RemoveUserFromSearchPool(user.UserId);
            
            Assert.AreEqual(user.UserId, result?.UserId);
        }

        [TestMethod]
        public void AddInSearchPoolTest()
        {
            var chatHub = new ChatHub(GetLogger<ChatHub>(), null);

            var user = new User()
            {
                UserId = 123,
                Age = 15,
                Gender = (int)Gender.Male,
                UserSetting = new UserSetting()
                {
                    UserId = 123,
                    PreferredAge = (int)AgeCategory.LessThanEighteen,
                    PreferredChatType = (int)CommunicationType.Standart,
                    PreferredGender = (int)Gender.Male,
                },
                CreatedAt = DateTime.Now,
            };

            chatHub.AddUserInSearchPool(user);
            var result = chatHub.IsUserInSearchPool(user.UserId);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AddInSearchPoolWithUserEqualNullTest()
        {
            var chatHub = new ChatHub(GetLogger<ChatHub>(), null);

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                chatHub.AddUserInSearchPool(null);
            });
        }
        
        [TestMethod]
        [DataRow(null, (int)Gender.Male, (int)AgeCategory.LessThanTwentyFive, (int)Gender.Male, (int)CommunicationType.Standart)]
        [DataRow(15, null, (int)AgeCategory.LessThanTwentyFive, (int)Gender.Male, (int)CommunicationType.OnlyVoice)]
        [DataRow(16, (int)Gender.Female, null, (int)Gender.Male, (int)CommunicationType.OnlyVoice)]
        [DataRow(17, (int)Gender.Male, (int)AgeCategory.LessThanTwentyFive, null, (int)CommunicationType.Standart)]
        [DataRow(18, (int)Gender.Female, (int)AgeCategory.LessThanTwentyFive, (int)Gender.Female, null)]
        public void AddInSearchPoolWithRequiredFieldsIsEmptyOrNullTest(
            int? age, 
            int? gender,
            int? preferredAge,
            int? preferredGender,
            int? preferredChatType)
        {
            var chatHub = new ChatHub(GetLogger<ChatHub>(), null);

            var user = new User()
            {
                UserId = 123,
                Age = age,
                Gender = gender,
                UserSetting = new UserSetting()
                {
                    UserId = 123,
                    PreferredAge = preferredAge,
                    PreferredGender = preferredGender,
                    PreferredChatType = preferredChatType,
                },
                CreatedAt = DateTime.Now,
            };

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                chatHub.AddUserInSearchPool(user);
            });
        }

        private ILogger<T> GetLogger<T>()
        {
            using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
                .SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace)
                .AddConsole());

            return loggerFactory.CreateLogger<T>();
        }
    }
}