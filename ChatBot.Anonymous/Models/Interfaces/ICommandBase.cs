using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChatBot.Anonymous.Models.Interfaces
{
    public interface ICommandBase
    {
        /// <summary>
        /// Название команды
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Список слов/команд для вызова команды
        /// </summary>
        public abstract List<string> Triggers { get; set; }

        /// <summary>
        /// Вызывает обработку команды
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public abstract Task Execute(ITelegramBotClient client, Message message);
    }
}
