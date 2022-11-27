using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TG.ChatBot.Common.Common.Enums;
using TG.ChatBot.Common.Common.Helpers;
using TG.ChatBot.Common.Domain;
using TG.ChatBot.Common.StepByStep.Enums;
using TG.ChatBot.Common.StepByStep.Interfaces;

namespace TG.ChatBot.Host.Services.StepsByStep.Steps
{
    /// <summary>
    /// Шаг заполнения предпочитаемого возраста собеседника
    /// </summary>
    public class PreferredAgeStep : IStep
    {
        private readonly ITelegramBotClient _botClient;
        private readonly RepositoryService _repository;

        public Step Id { get; } = Step.PreferredAge;

        public PreferredAgeStep(ITelegramBotClient botClient, RepositoryService repository)
        {
            _botClient = botClient;
            _repository = repository;
        }

        public async Task Execute(long chatId)
        {
            var keyboard = new InlineKeyboardMarkup(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(AgeCategory.LessThanEighteen.GetAgeRangeDescription(), AgeCategory.LessThanEighteen.ToString("d")),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(AgeCategory.LessThanTwentyOne.GetAgeRangeDescription(), AgeCategory.LessThanTwentyOne.ToString("d")),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(AgeCategory.LessThanTwentyFive.GetAgeRangeDescription(), AgeCategory.LessThanTwentyFive.ToString("d")),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(AgeCategory.LessThanThirty.GetAgeRangeDescription(), AgeCategory.LessThanThirty.ToString("d")),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(AgeCategory.MoreThanThirty.GetAgeRangeDescription(), AgeCategory.MoreThanThirty.ToString("d"))
                    },
                });

            var textMessage = new StringBuilder("Выберите возраст собеседника");
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: textMessage.ToString(),
                parseMode: ParseMode.Markdown, replyMarkup: keyboard);
        }

        public async Task Processing(string data, long userId, Action<long, IStep, Step> action)
        {
            var preferredAge = int.Parse(data).ToEnum<AgeCategory>();

            await Argument.NotNull(
                value: preferredAge,
                message: $"_Выбран неверный возраст собеседника: {preferredAge?.GetAgeRangeDescription()}_",
                chatId: userId,
                botClient: _botClient);

            await _repository.Settings.SaveSetting(userId: userId, preferredAge: (int)preferredAge);
        }
    }
}
