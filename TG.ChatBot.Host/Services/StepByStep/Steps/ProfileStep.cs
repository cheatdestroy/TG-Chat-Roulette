using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TG.ChatBot.Common.Common.Helpers;
using TG.ChatBot.Common.StepByStep.Enums;
using TG.ChatBot.Common.StepByStep.Interfaces;

namespace TG.ChatBot.Host.Services.StepByStep.Steps
{
    public class ProfileStep : IStep
    {
        private readonly ITelegramBotClient _botClient;

        public Step Id { get; } = Step.Profile;

        public ProfileStep(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task Execute(long chatId)
        {
            var textMessage = new StringBuilder("Выберите данные, которые хотите изменить:");
            var keyboard = new InlineKeyboardMarkup(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Пол", Step.Gender.ToString("d")),
                        InlineKeyboardButton.WithCallbackData("Возраст", Step.Age.ToString("d")),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Тип чата", Step.ChatType.ToString("d")),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Пол собеседника", Step.PreferredGender.ToString("d")),
                        InlineKeyboardButton.WithCallbackData("Возраст собеседника", Step.PreferredAge.ToString("d"))
                    },
                });

            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: textMessage.ToString(),
                replyMarkup: keyboard,
                parseMode: ParseMode.Markdown);
        }

        public Task Processing(string data, long userId, Action<long, IStep, Step> action)
        {
            var step = int.Parse(data).ToEnum<Step>();

            if (step != null)
            {
                action.Invoke(userId, this, step.Value);
            }

            return Task.CompletedTask;
        }
    }
}
