using ChatBot.Anonymous.Common.Enums;
using ChatBot.Anonymous.Common.Enums.ActionSteps;
using ChatBot.Anonymous.Common.Helpers;
using ChatBot.Anonymous.Models;
using ChatBot.Anonymous.Models.Interfaces;
using ChatBot.Anonymous.Services;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ChatBot.Anonymous.Commands.Actions
{
    public class StartAction : IActionSteps
    {
        private readonly ITelegramBotClient _botClient;
        private readonly BotConfiguration _configuration;
        private readonly RepositoryService _repositoryService;

        public CommandActions Action { get; }

        public StartAction(ITelegramBotClient botClient, RepositoryService repositoryService, IConfiguration configuration)
        {
            Action = CommandActions.StartAction;

            _botClient = botClient;
            _repositoryService = repositoryService;
            _configuration = configuration.GetMainConfigurationToObject();
        }

        #region Get steps
        public int GetDefaultStep()
        {
            return (int)StartSteps.GenderStep;
        }

        public int? GetNextStep(int? currentStep)
        {
            if (currentStep.IsNotOwnerValue(typeof(StartSteps)))
            {
                return null;
            }

            return (StartSteps?)currentStep switch
            {
                StartSteps.GenderStep => (int)StartSteps.AgeStep,
                StartSteps.AgeStep => (int)StartSteps.ChatTypeStep,
                _ => null,
            };
        }

        public int? GetPreviousStep(int? currentStep)
        {
            if (currentStep.IsNotOwnerValue(typeof(StartSteps)))
            {
                return null;
            }

            return (StartSteps?)currentStep switch
            {
                StartSteps.AgeStep => (int)StartSteps.GenderStep,
                StartSteps.ChatTypeStep => (int)StartSteps.AgeStep,
                _ => null,
            };
        }
        #endregion

        #region Execute/Processing steps
        public async Task ExecuteSteps(Message message, Domain.Entities.User user)
        {
            if (user.Action == null)
            {
                throw new ArgumentNullException(nameof(user.Action), "Action is null");
            }

            if (user.Action.CurrentStep.IsNotOwnerValue(typeof(StartSteps)))
            {
                user.Action.CurrentStep ??= GetDefaultStep();
            }

            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            try
            {
                switch ((StartSteps?)user.Action.CurrentStep)
                {
                    case StartSteps.GenderStep:
                        await ExecuteGenderStep(chatId: chatId);
                        break;
                    case StartSteps.AgeStep:
                        await ExecuteAgeStep(message.Chat.Id);
                        break;
                    case StartSteps.ChatTypeStep:
                        await ExecuteChatTypeStep(message.Chat.Id);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(user.Action.CurrentStep), "Current step is not valid");
                }

                await _repositoryService.Action.SaveAction(user.UserId, (int)Action, user.Action.CurrentStep);
            }
            catch
            {
            }
        }

        public async Task ProcessingSteps(Update update, Domain.Entities.User user)
        {
            if (user.Action == null)
            {
                throw new ArgumentNullException(nameof(user.Action), "Action is null");
            }

            if (user.Action.CurrentStep.IsNotOwnerValue(typeof(StartSteps)))
            {
                throw new ArgumentOutOfRangeException(nameof(user.Action.CurrentStep), "Current step is not valid");
            }

            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        await HandlindMessage(update.Message!, user);
                        break;
                    case UpdateType.CallbackQuery:
                        await HandlingCallbackQuery(update.CallbackQuery!, user);
                        break;
                    default:
                        return;
                }
            }
            catch
            {
            }
        }
        #endregion

        public async Task FinishAction(Message message, long userId)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message is null");
            }

            await _repositoryService.Action.SaveAction(userId: userId);
            var user = await _repositoryService.User.GetById(userId: userId);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User is null");
            }

            var gender = (Gender?)user.Gender switch
            {
                Gender.Male => "мужской",
                Gender.Female => "женский",
                _ => "неизвестно"
            };

            var chatType = (CommunicationType?)user.UserSetting?.PreferredChatType switch
            {
                CommunicationType.Standart => "стандартный",
                CommunicationType.OnlyVoice => "голосовые сообщения",
                _ => "неизвестно"
            };

            var textMessage = new StringBuilder("Вы успешно заполнили необходимые поля!\n\n");
            textMessage.Append($"Ваш пол: {gender}\n");
            textMessage.Append($"Ваш возраст: {user.Age}\n");
            textMessage.Append($"Предпочитаемый тип чата: {chatType}\n");
            await _botClient.SendTextMessageAsync(
                chatId: userId, 
                text: textMessage.ToString(), 
                parseMode: ParseMode.Markdown);
        }

        #region Handling steps process
        private async Task HandlindMessage(Message message, Domain.Entities.User user)
        {
            if (user?.Action?.CurrentStep == null)
            {
                throw new ArgumentNullException(nameof(user.Action.CurrentStep), "Action or Step is null");
            }

            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message is null");
            }

            var data = message.Text;

            if (string.IsNullOrEmpty(data))
            {
                return;
            }

            var userId = user.UserId;

            switch ((StartSteps)user.Action.CurrentStep)
            {
                case StartSteps.AgeStep:
                    await ProcessingAgeStep(data: data, userId: userId);
                    break;
                default:
                    await ExecuteSteps(message, user);
                    return;
            }

            await SetNextStep(userId, message);
        }

        private async Task HandlingCallbackQuery(CallbackQuery callbackQuery, Domain.Entities.User user)
        {
            if (user?.Action?.CurrentStep == null)
            {
                throw new ArgumentNullException(nameof(user.Action.CurrentStep), "Action or Step is null");
            }

            if (callbackQuery.Message == null)
            {
                throw new ArgumentNullException(nameof(callbackQuery.Message), "Message is null");
            }

            var data = callbackQuery.Data;

            if (string.IsNullOrEmpty(data))
            {
                return;
            }

            var userId = user.UserId;

            switch ((StartSteps)user.Action.CurrentStep)
            {
                case StartSteps.GenderStep:
                    await ProcessingGenderStep(data: data, userId: userId);
                    break;
                case StartSteps.ChatTypeStep:
                    await ProcessingChatTypeStep(data: data, userId: userId);
                    await FinishAction(message: callbackQuery.Message, userId: userId);
                    return;
                default:
                    await ExecuteSteps(callbackQuery.Message, user);
                    return;
            }

            await SetNextStep(userId, callbackQuery.Message);
        }

        private async Task SetNextStep(long userId, Message message)
        {
            var user = await _repositoryService.User.GetById(userId: userId);

            if (user != null && user.Action != null)
            {
                user.Action.CurrentStep = GetNextStep(user.Action.CurrentStep);
                await ExecuteSteps(message, user);
            }
        }
        #endregion

        #region Execute steps
        private async Task ExecuteGenderStep(long chatId)
        {
            var keyboard = new InlineKeyboardMarkup(
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("мужской ♂", Gender.Male.ToString("d")),
                    InlineKeyboardButton.WithCallbackData("женский ♀‍", Gender.Female.ToString("d"))
                });

            var textMessage = new StringBuilder("Выберите ваш пол");
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: textMessage.ToString(),
                parseMode: ParseMode.Markdown, replyMarkup: keyboard);
        }

        private async Task ExecuteAgeStep(long chatId)
        {
            var textMessage = new StringBuilder("Введите ваш возраст");
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: textMessage.ToString(),
                parseMode: ParseMode.Markdown);
        }

        private async Task ExecuteChatTypeStep(long chatId)
        {
            var keyboard = new InlineKeyboardMarkup(
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("обычный 📨", CommunicationType.Standart.ToString("d")),
                    InlineKeyboardButton.WithCallbackData("голосовые сообщения 🎤", CommunicationType.OnlyVoice.ToString("d"))
                });

            var textMessage = new StringBuilder("Выберите предпочитаемый тип чата\n\n");
            textMessage.Append("1. *обычный* - стандартный привычный чат, с возможностью обмена любыми типами сообщений\n");
            textMessage.Append("2. *голосовые сообщений* - возможность отправлять только голосовые сообщения");
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: textMessage.ToString(),
                parseMode: ParseMode.Markdown, replyMarkup: keyboard);
        }
        #endregion

        #region Processing steps
        private async Task ProcessingGenderStep(string data, long userId)
        {
            var gender = Convert.ToInt32(data);

            if (gender.IsNotOwnerValue(typeof(Gender)))
            {
                throw new ArgumentOutOfRangeException(nameof(gender), "Gender is not valid");
            }

            await _repositoryService.User.SaveUser(userId: userId, gender: gender);
        }

        private async Task ProcessingAgeStep(string data, long userId)
        {
            var age = Convert.ToInt32(data);
            var minimumAge = _configuration.MinimumAge;
            var maximumAge = _configuration.MaximumAge;

            if (age > maximumAge || age < minimumAge)
            {
                await _botClient.SendTextMessageAsync(
                    chatId: userId,
                    text: $"_Возраст не может быть меньше {minimumAge} или больше {maximumAge}_",
                    parseMode: ParseMode.Markdown);

                throw new ArgumentOutOfRangeException(nameof(age), "Age is not within the specified range (min - max)");
            }

            await _repositoryService.User.SaveUser(userId: userId, age: age);
        }

        private async Task ProcessingChatTypeStep(string data, long userId)
        {
            int? chatType = Convert.ToInt32(data);

            if (chatType.IsNotOwnerValue(typeof(CommunicationType)))
            {
                throw new ArgumentOutOfRangeException(nameof(chatType), "ChatType is not valid");
            }

            await _repositoryService.Settings.SaveSetting(userId: userId, preferredChatType: chatType);
        }
        #endregion
    }
}
