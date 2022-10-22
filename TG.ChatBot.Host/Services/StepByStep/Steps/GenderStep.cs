using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TG.ChatBot.Common.Common.Enums;
using TG.ChatBot.Common.Common.Helpers;
using TG.ChatBot.Common.Domain;
using TG.ChatBot.Common.StepByStep.Enums;
using TG.ChatBot.Host.Services.StepByStep.Interfaces;

namespace TG.ChatBot.Host.Services.StepsByStep.Steps
{
    /// <summary>
    /// Шаг заполнения пола пользователя
    /// </summary>
    public class GenderStep : IStep
    {
        private readonly ITelegramBotClient _botClient;
        private readonly RepositoryService _repository;

        public Step Id { get; } = Step.Gender;

        public GenderStep(ITelegramBotClient botClient, RepositoryService repository)
        {
            _botClient = botClient;
            _repository = repository;
        }

        public async Task Execute(long chatId)
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

        public async Task Processing(string data, long userId)
        {
            var gender = int.Parse(data).ToEnum<Gender>();

            await Argument.NotNull(
                value: gender,
                message: $"_Выбран неверный пол: {gender.GetDescription()}_",
                chatId: userId,
                botClient: _botClient);

            await _repository.User.SaveUser(userId: userId, gender: (int)gender);
        }
    }
}
