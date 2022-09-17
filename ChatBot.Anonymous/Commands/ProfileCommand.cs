using ChatBot.Anonymous.Common.Helpers;
using ChatBot.Anonymous.Models.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChatBot.Anonymous.Commands
{
    /// <summary>
    /// Команда настроек
    /// </summary>
    public class ProfileCommand : ICommandBase
    {
        private readonly ITelegramBotClient _botClient;

        public string Name => "Профиль";

        public List<string> Triggers { get; set; }

        public ProfileCommand(ITelegramBotClient botClient)
        {
            Triggers = new List<string>
            {
                "/profile"
            };

            _botClient = botClient;
        }

        public async Task Execute(Message message)
        {
            var (_, chatId, messageId, text) = CommandHelper.GetRequiredParams(message);

            await _botClient.SendTextMessageAsync(chatId, Name, replyToMessageId: messageId);
        }
    }
}
