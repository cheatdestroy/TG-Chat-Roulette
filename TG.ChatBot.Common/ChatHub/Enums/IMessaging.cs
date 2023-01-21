using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TG.ChatBot.Common.ChatHub.Enums
{
    public interface IMessaging
    {
        Task SendMessage(Message message, long userId);
    }
}
