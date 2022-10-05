using ChatBot.Anonymous.Common.Enums;
using ChatBot.Anonymous.Common.Helpers;
using ChatBot.Anonymous.Services;
using ChatBot.Anonymous.Services.StepByStep.Interfaces;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ChatBot.Anonymous.Services.StepsByStep.Steps
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
                        InlineKeyboardButton.WithCallbackData(AgeCategory.LessThanEighteen.GetDescription(), AgeCategory.LessThanEighteen.ToString("d")),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(AgeCategory.LessThanTwentyOne.GetDescription(), AgeCategory.LessThanTwentyOne.ToString("d")),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(AgeCategory.LessThanTwentyFive.GetDescription(), AgeCategory.LessThanTwentyFive.ToString("d")),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(AgeCategory.LessThanThirty.GetDescription(), AgeCategory.LessThanThirty.ToString("d")),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(AgeCategory.MoreThanThirty.GetDescription(), AgeCategory.MoreThanThirty.ToString("d"))
                    },
                });

            var textMessage = new StringBuilder("Выберите возраст собеседника");
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: textMessage.ToString(),
                parseMode: ParseMode.Markdown, replyMarkup: keyboard);
        }

        public async Task Processing(string data, long userId)
        {
            var preferredAge = int.Parse(data).ToEnum<AgeCategory>();

            await Argument.NotNull(
                value: preferredAge,
                message: $"_Выбран неверный возраст собеседника: {preferredAge.GetDescription()}_",
                chatId: userId,
                botClient: _botClient);

            await _repository.Settings.SaveSetting(userId: userId, preferredAge: (int)preferredAge);
        }
    }
}
