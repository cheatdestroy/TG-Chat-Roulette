using ChatBot.Anonymous.Common.Helpers;
using ChatBot.Anonymous.Models.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChatBot.Anonymous.Commands
{
    /// <summary>
    /// Команда для теста
    /// </summary>
    public class TestCommand : ICommandBase
    {
        private readonly ITelegramBotClient _botClient;

        public string Name => "Тестовая команда";

        public List<string> Triggers { get; set; }

        public TestCommand(ITelegramBotClient botClient)
        {
            Triggers = new List<string>
            {
                "/test",
                "/unk"
            };

            _botClient = botClient;
        }

        public async Task Execute(Message message)
        {
            var (_, chatId, messageId, _) = CommandHelper.GetRequiredParams(message);

            await _botClient.SendTextMessageAsync(chatId, Name, replyToMessageId: messageId);
        }
    }
}
