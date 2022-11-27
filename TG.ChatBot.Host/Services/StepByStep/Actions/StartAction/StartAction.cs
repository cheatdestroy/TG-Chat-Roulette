using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TG.ChatBot.Common.Common.Helpers;
using TG.ChatBot.Common.Domain;
using TG.ChatBot.Common.StepByStep.Enums;
using TG.ChatBot.Common.StepByStep.Interfaces;

namespace TG.ChatBot.Host.Services.StepByStep.Actions
{
    public class StartAction : ActionBase
    {
        private readonly ITelegramBotClient _botClient;

        public StartAction(
            ILogger<StartAction> logger,
            ActionSteps steps,
            ITelegramBotClient botClient,
            RepositoryService repository) : base(CommandActions.StartAction, steps, logger, repository)
        {
            Steps.AddStep(Step.Gender);
            Steps.AddStep(Step.Age);
            Steps.AddStep(Step.ChatType);
            Steps.AddStep(Step.PreferredGender);
            Steps.AddStep(Step.PreferredAge);

            _botClient = botClient;
        }

        public override async Task FinishAction(Common.Domain.Entities.User user)
        {
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

            var textMessage = user.FormatUserInfo();
            textMessage.Insert(0, "Информация обновлена\n\n");
            textMessage.Append($"\n\nТеперь можно начинать поиск собеседника 👉🏻 /find");
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: textMessage.ToString(),
                parseMode: ParseMode.Markdown);
        }
    }
}
