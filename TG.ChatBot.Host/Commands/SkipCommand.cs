using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TG.ChatBot.Common.ChatHub.Models;
using TG.ChatBot.Common.Common.Helpers;
using TG.ChatBot.Common.Models.Interfaces;
using TG.ChatBot.Host.Services.Communication;

namespace TG.ChatBot.Host.Commands
{
    public class SkipCommand : ICommandBase
    {
        private readonly IChatHub _chatHub;
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<SkipCommand> _logger;

        public string Name => "Завершить общение";
        public List<string> Triggers { get; set; }

        public SkipCommand(IServiceProvider serviceProvider, ILogger<SkipCommand> logger, ITelegramBotClient botClient)
        {
            Triggers = new List<string>()
            {
                "/skip",
                "/next"
            };

            _chatHub = ChatHub.GetInstance(serviceProvider);
            _botClient = botClient;
            _logger = logger;
        }

        public async Task Execute(Update update)
        {
            var userId = update.GetSenderId();

            if (!userId.HasValue)
            {
                return;
            }

            var chatRoom = _chatHub.EndChat(userId.Value);

            if (chatRoom != null)
            {
                await NotifyEndChat(chatRoom.FirstUserId, chatRoom.FirstUserId, chatRoom.InitiatorEndId);
                await NotifyEndChat(chatRoom.SecondUserId, chatRoom.SecondUserId, chatRoom.InitiatorEndId);
            }
        }

        private async Task NotifyEndChat(long chatId, long userId, long? initiatorId)
        {
            var isInitiator = initiatorId == userId;
            var textMessage = new StringBuilder(isInitiator ? "Вы завершили чат." : "Собеседник завершил чат!");
            textMessage.Append("\n\nНайти нового собеседника 👉🏻 /find\n\n");
            textMessage.Append("👇🏻 Оцените собеседника 👇🏻");

            var keyboard = new InlineKeyboardMarkup(
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("👍🏻", "0"),
                        InlineKeyboardButton.WithCallbackData("👎🏻", "0")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("⚠️ Пожаловаться ⚠️", "0")
                    }
                });

            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: textMessage.ToString(),
                parseMode: ParseMode.Markdown,
                replyMarkup: keyboard);
        }
    }
}
