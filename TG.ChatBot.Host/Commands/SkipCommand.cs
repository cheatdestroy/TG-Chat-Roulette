using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TG.ChatBot.Common.ChatHub.Models;
using TG.ChatBot.Common.Common.Helpers;
using TG.ChatBot.Common.Models.Interfaces;

namespace TG.ChatBot.Host.Commands
{
    public class SkipCommand : ICommandBase
    {
        private readonly IChatHub _chatHub;
        private readonly ICommandService _serviceCommand;
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<SkipCommand> _logger;

        public string Name => "Завершить общение";
        public List<string> Triggers { get; set; }

        public SkipCommand(IChatHub chatHub, ICommandService serviceCommand, ILogger<SkipCommand> logger, ITelegramBotClient botClient)
        {
            Triggers = new List<string>()
            {
                "/skip",
                "/next"
            };

            _chatHub = chatHub;
            _botClient = botClient;
            _logger = logger;
            _serviceCommand = serviceCommand;
        }

        public async Task Execute(Update update)
        {
            var userId = update.GetSenderId();

            if (!userId.HasValue)
            {
                return;
            }

            var chatRoom = await _chatHub.EndChat(userId.Value);

            if (chatRoom != null)
            {
                var totalMessages = chatRoom.NumberMessagesFirstUser + chatRoom.NumberMessagesSecondUser;
                var duration = chatRoom.EndDate?.Subtract(chatRoom.StartDate) ?? TimeSpan.MinValue;
                await NotifyEndChat(chatRoom.FirstUserId, chatRoom.FirstUserId == chatRoom.InitiatorEndId, totalMessages, duration);
                await NotifyEndChat(chatRoom.SecondUserId, chatRoom.SecondUserId == chatRoom.InitiatorEndId, totalMessages, duration);
            }

            if (update.Message?.Text == "/next")
            {
                update.Message.Text = "/find";
                await _serviceCommand.ExecuteCommand(update);
            }
        }

        private async Task NotifyEndChat(long userId, bool isInitiator, int totalMessages, TimeSpan duration)
        {
            var hours = duration.Hours > 0 ? $"{duration.Hours} час. " : String.Empty;
            var minutes = duration.Minutes > 0 ? $"{duration.Minutes} мин. " : String.Empty;
            var seconds = $"{duration.Seconds} сек. ";

            var textMessage = new StringBuilder(isInitiator ? "Вы завершили чат." : "Собеседник завершил чат!");
            textMessage.Append($"\n\nВсего сообщений: {totalMessages}\n");
            textMessage.Append($"Продолжительность общения: {hours}{minutes}{seconds}\n\n");
            textMessage.Append("Найти нового собеседника 👉🏻 /find\n\n");
            textMessage.Append("👇🏻 Оцените собеседника 👇🏻");

            var keyboard = new InlineKeyboardMarkup(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("❤️", "0"),
                        InlineKeyboardButton.WithCallbackData("😐", "0"),
                        InlineKeyboardButton.WithCallbackData("💩", "0")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("🚫 Заблокировать 🚫", "0")
                    }
                });

            await _botClient.SendTextMessageAsync(
                chatId: userId,
                text: textMessage.ToString(),
                parseMode: ParseMode.Markdown,
                replyMarkup: keyboard);
        }
    }
}
