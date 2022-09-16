using ChatBot.Anonymous.Common.Helpers;
using ChatBot.Anonymous.Models.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChatBot.Anonymous.Commands
{
    /// <summary>
    /// Команда настроек
    /// </summary>
    public class SettingsCommand : ICommandBase
    {
        public string Name => "Настройки";

        public List<string> Triggers { get; set; }

        public SettingsCommand()
        {
            Triggers = new List<string>
            {
                "/start"
            };
        }

        public async Task Execute(ITelegramBotClient client, Message message)
        {
            var (chatId, messageId, text) = CommandHelper.GetRequiredParams(message);

            await client.SendTextMessageAsync(chatId, Name, replyToMessageId: messageId);
        }
    }
}
