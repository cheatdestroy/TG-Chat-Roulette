using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TG.ChatBot.Common.Common.Helpers;
using TG.ChatBot.Common.Domain;
using TG.ChatBot.Common.StepByStep.Enums;
using TG.ChatBot.Common.StepByStep.Interfaces;

namespace TG.ChatBot.Host.Services.StepByStep.Actions
{
    public class ProfileAction : ActionBase
    {
        private readonly ITelegramBotClient _botClient;

        public ProfileAction(
            ILogger<ActionBase> logger, 
            ActionSteps steps,
            ITelegramBotClient botClient,
            RepositoryService repository) : base(CommandActions.ProfileAction, steps, logger, repository)
        {
            Steps.AddStep(Step.Profile);

            IsDynamicSteps = true;
            _botClient = botClient;
        }

        public override async Task FinishAction(Common.Domain.Entities.User user)
        {
            Argument.NotNull(user, "User is null");

            var chatId = user.UserId;

            var textMessage = user.FormatUserInfo();
            textMessage.Insert(0, "Информация обновлена\n\n");
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: textMessage.ToString(),
                parseMode: ParseMode.Markdown);
        }
    }
}
