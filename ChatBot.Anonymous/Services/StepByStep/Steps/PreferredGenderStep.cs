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
    /// Шаг заполнения предпочитаемого пола собеседника
    /// </summary>
    public class PreferredGenderStep : IStep
    {
        private readonly ITelegramBotClient _botClient;
        private readonly RepositoryService _repository;

        public Step Id { get; } = Step.PreferredGender;

        public PreferredGenderStep(ITelegramBotClient botClient, RepositoryService repository)
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
                        InlineKeyboardButton.WithCallbackData("мужской ♂", Gender.Male.ToString("d")),
                        InlineKeyboardButton.WithCallbackData("женский ♀‍", Gender.Female.ToString("d")),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("не важно ⚥‍", Gender.Any.ToString("d"))
                    }
                });

            var textMessage = new StringBuilder("Выберите пол собеседника");
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: textMessage.ToString(),
                parseMode: ParseMode.Markdown, replyMarkup: keyboard);
        }

        public async Task Processing(string data, long userId)
        {
            var preferredGender = int.Parse(data).ToEnum<Gender>();

            await Argument.NotNull(
                value: preferredGender,
                message: $"_Выбран неверный пол собеседника: {preferredGender.GetDescription()}_",
                chatId: userId,
                botClient: _botClient);

            await _repository.Settings.SaveSetting(userId: userId, preferredGender: (int)preferredGender);
        }
    }
}
