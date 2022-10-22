using TG.ChatBot.Host.Common.Enums;
using TG.ChatBot.Host.Common.Helpers;
using TG.ChatBot.Host.Models.Interfaces;
using TG.ChatBot.Host.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using User = TG.ChatBot.Host.Domain.Entities.User;

namespace TG.ChatBot.Host.Commands
{
    /// <summary>
    /// Команда просмотра профиля
    /// </summary>
    public class ProfileCommand : ICommandBase
    {
        private readonly RepositoryService _repository;
        private readonly ITelegramBotClient _botClient;

        public string Name => "Профиль";
        public List<string> Triggers { get; set; }

        public ProfileCommand(RepositoryService repository, ITelegramBotClient botClient)
        {
            Triggers = new List<string>
            {
                "/profile"
            };

            _repository = repository;
            _botClient = botClient;
        }

        public async Task Execute(Update update)
        {
          /*  var userId = update.Message?.From?.Id;

            if (!userId.HasValue || !update.Message.IsPrivateChat())
            {
                return;
            }

            var user = await _repository.User.GetById(userId: userId.Value);*/

        }

      /*  private async Task ProfileInfo(User user)
        {
            if (user?.Action == null)
            {
                return;
            }

            var textMessage = user.GetUserInfo();
            textMessage.Insert(0, "Ваш профиль:\n\n");

            var keyboard = new InlineKeyboardMarkup(
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Изменить данные", Step.ChangingData.ToString("d"))
                });

            await _botClient.SendTextMessageAsync(
                chatId: user.Action.ChatId,
                text: textMessage.ToString(),
                parseMode: ParseMode.Markdown,
                replyMarkup: keyboard);
        }

        private async Task ChangingData(long chatId)
        {
            var keyboard = new InlineKeyboardMarkup(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Пол", Step.Gender.ToString("d")),
                        InlineKeyboardButton.WithCallbackData("Возраст", Step.Age.ToString("d"))
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Тип общения", Step.ChatType.ToString("d"))
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Пол собеседника", Step.PreferredGender.ToString("d")),
                        InlineKeyboardButton.WithCallbackData("Возраст собеседника", Step.PreferredAge.ToString("d"))
                    }
                });

            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: textMessage.ToString(),
                parseMode: ParseMode.Markdown,
                replyMarkup: keyboard);
        }*/
    }
}
