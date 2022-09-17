using ChatBot.Anonymous.Models.Interfaces;
using Telegram.Bot.Types;

namespace ChatBot.Anonymous.Common.Helpers
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
        /// <returns> Возвращает идентификатор пользователя, идентификатор чата, идентификатор сообщения, текст сообщения </returns>
        public static (long?, long, int, string?) GetRequiredParams(Message message)
        {
            return (message.From?.Id, message.Chat.Id, message.MessageId, message.Text);
        }
    }
}
