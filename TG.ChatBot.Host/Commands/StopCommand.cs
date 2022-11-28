using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TG.ChatBot.Common.ChatHub.Models;
using TG.ChatBot.Common.Models.Interfaces;
using TG.ChatBot.Host.Services.Communication;

namespace TG.ChatBot.Host.Commands
{
    public class StopCommand : ICommandBase
    {
        private readonly IChatHub _chatHub;
        private readonly ITelegramBotClient _botClient;

        public string Name => "Остановка поиска собеседника";
        public List<string> Triggers { get; set; }

        public StopCommand(IServiceProvider serviceProvider, ITelegramBotClient botClient)
        {
            Triggers = new List<string>()
            {
                "/stop"
            };

            _chatHub = ChatHub.GetInstance(serviceProvider);
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
