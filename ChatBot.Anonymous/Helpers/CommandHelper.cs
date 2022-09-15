using ChatBot.Anonymous.Models.Interfaces;
using Telegram.Bot.Types;

namespace ChatBot.Anonymous.Helpers
{
    /// <summary>
    /// Вспомогательные функции для команд
    /// </summary>
    public static class CommandHelper
    {
        /// <summary>
        /// Получение обязательных параметров
        /// </summary>
        /// <param name="message"></param>
        /// <returns> Возвращает идентификатор чата, идентификатор сообщения, текс сообщения </returns>
        public static (long, int, string?) GetRequiredParams(Message message)
        {
            return (message.Chat.Id, message.MessageId, message.Text);
        }
    }
}
