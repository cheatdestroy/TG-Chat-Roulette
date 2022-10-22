using TG.ChatBot.Host.Models.Interfaces;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TG.ChatBot.Host.Commands
{
    public class StopCommand : ICommandBase
    {
        private readonly IChatHub _chatHub;
        private readonly ITelegramBotClient _botClient;

        public string Name => "Остановка поиска собеседника";
        public List<string> Triggers { get; set; }

        public StopCommand(IChatHub chatHub, ITelegramBotClient botClient)
        {
            Triggers = new List<string>()
            {
                "/stop"
            };

            _chatHub = chatHub;
            _botClient = botClient;
        }

        public async Task Execute(Update update)
        {
            var userId = update.Message?.From?.Id;

            if (userId.HasValue && _chatHub.IsUserInSearchPool(userId.Value))
            {
                var user = _chatHub.RemoveUserFromSearchPool(userId.Value);

                var textMessage = new StringBuilder("Поиск собеседника остановлен\n\n");
                textMessage.Append("Для возобновления поиска 👉🏻 /find");

                await _botClient.SendTextMessageAsync(
                    chatId: userId.Value,
                    text: textMessage.ToString(),
                    parseMode: ParseMode.Markdown);
            }
        }
    }
}
