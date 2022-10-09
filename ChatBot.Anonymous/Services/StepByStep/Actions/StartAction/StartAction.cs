using ChatBot.Anonymous.Common.Enums;
using ChatBot.Anonymous.Common.Helpers;
using ChatBot.Anonymous.Services.StepByStep.Interfaces;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatBot.Anonymous.Services.StepByStep.Actions
{
    public class StartAction<T> : IAction where T : class, IActionSteps
    {
        private readonly ILogger<StartAction<T>> _logger;
        private readonly ITelegramBotClient _botClient;
        private readonly RepositoryService _repositoryService;

        public CommandActions Action { get; }
        public IActionSteps Steps { get; }

        public StartAction(
            ILogger<StartAction<T>> logger,
            ITelegramBotClient botClient,
            RepositoryService repositoryService,
            T actionSteps)
        {
            Action = CommandActions.StartAction;
            Steps = actionSteps;

            _logger = logger;
            _botClient = botClient;
            _repositoryService = repositoryService;
        }

        #region Execute/Processing steps
        public async Task ExecuteSteps(Message message, Domain.Entities.User user)
        {
            Argument.NotNull(user?.Action?.CurrentAction, "Current action is null");

            var currentStep = user.Action.CurrentStep.ToEnum<Step>() ?? Steps.GetDefaultStep()?.Id;

            Argument.NotNull(currentStep, "Step id is null");

            try
            {
                var chatId = message.Chat.Id;
                var currentStepData = Steps.GetStepById(stepId: currentStep.Value);

                if (currentStepData != null)
                {
                    await currentStepData.Execute(chatId: chatId);
                    await _repositoryService.Action.SaveAction(userId: user.UserId, actionId: (int)Action, stepId: (int)currentStep);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem with executing step");
                throw;
            }
        }

        public async Task ProcessingSteps(Update update, Domain.Entities.User user)
        {
            var currentStep = user.Action?.CurrentStep.ToEnum<Step>();

            Argument.NotNull(currentStep, "Step id is null");

            var userId = user.UserId;
            var currentStepData = Steps.GetStepById(stepId: currentStep.Value);

            Argument.NotNull(currentStepData, "Step is null");

            try
            {
                Message? message = null;
                var data = default(string);

                if (update.Type == UpdateType.Message)
                {
                    message = update.Message;
                    data = message?.Text;
                }
                else if (update.Type == UpdateType.CallbackQuery)
                {
                    message = update.CallbackQuery?.Message;
                    data = update.CallbackQuery?.Data;
                }

                if (string.IsNullOrEmpty(data) || message == null)
                {
                    return;
                }

                await currentStepData.Processing(data: data, userId: userId);
                await SetNextStep(userId, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem with processing step");
                throw;
            }

        }
        #endregion

        public async Task FinishAction(Message message, long userId)
        {
            Argument.NotNull(message, "Message is null");

            await _repositoryService.Action.SaveAction(userId: userId);
            var user = await _repositoryService.User.GetById(userId: userId);

            Argument.NotNull(user, "User is null");

            var chatId = user.UserId;

            await Argument.NotNull(
                value: user.Gender,
                message: "Не указан пол",
                chatId: chatId,
                botClient: _botClient);

            await Argument.NotNull(
                value: user.Age,
                message: "Не указан возраст",
                chatId: chatId,
                botClient: _botClient);

            await Argument.NotNull(
                value: user.UserSetting?.PreferredChatType,
                message: "Не указан предпочитаемый тип общения",
                chatId: chatId,
                botClient: _botClient);

            await Argument.NotNull(
                value: user.UserSetting?.PreferredGender,
                message: "Не указан предпочитаемый пол собеседника",
                chatId: chatId,
                botClient: _botClient);

            await Argument.NotNull(
                value: user.UserSetting?.PreferredAge,
                message: "Не указан предпочитаемый возраст собеседника",
                chatId: chatId,
                botClient: _botClient);


            var gender = user.Gender.ToEnum<Gender>().GetDescription();
            var chatType = user.UserSetting?.PreferredChatType.ToEnum<CommunicationType>().GetDescription();
            var preferredGender = user.UserSetting?.PreferredGender.ToEnum<Gender>().GetDescription();
            var preferredAge = user.UserSetting?.PreferredAge.ToEnum<AgeCategory>().GetDescription();

            var textMessage = new StringBuilder("Информация обновлена:\n\n");
            textMessage.Append($"Ваш пол: {gender}\n");
            textMessage.Append($"Ваш возраст: {user.Age}\n");
            textMessage.Append($"Предпочитаемый тип чата: {chatType}\n");
            textMessage.Append($"Предпочитаемый пол собеседника: {preferredGender}\n");
            textMessage.Append($"Предпочитаемый возраст собеседника: {preferredAge}\n\n");
            textMessage.Append($"Для начала общения нажмите /search");
            await _botClient.SendTextMessageAsync(
                chatId: userId,
                text: textMessage.ToString(),
                parseMode: ParseMode.Markdown);
        }

        private async Task SetNextStep(long userId, Message message)
        {
            var user = await _repositoryService.User.GetById(userId: userId);

            Argument.NotNull(user?.Action, "Action is null");

            var currentStep = user.Action.CurrentStep.ToEnum<Step>();

            if (currentStep != null)
            {
                user.Action.CurrentStep = (int?)Steps.GetNextStep(stepId: currentStep.Value)?.Id;
            }

            if (user.Action.CurrentStep != null)
            {
                await ExecuteSteps(message, user);
            }
            else
            {
                await _repositoryService.Action.SaveAction(userId: user.UserId);
                await FinishAction(message: message, userId: userId);
            }
        }

    }
}
