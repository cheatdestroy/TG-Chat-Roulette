using Telegram.Bot;
using TG.ChatBot.Common.ChatHub.Enums;

namespace TG.ChatBot.Host.Services.Communication
{
    public class MessagingBase : IMessaging
    {
        private readonly ITelegramBotClient _botClient;

        public MessagingBase(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task SendMessage(string message, long recipient)
        {
            await _botClient.SendTextMessageAsync(
                chatId: recipient, 
                text: message);
        }
    }
}
