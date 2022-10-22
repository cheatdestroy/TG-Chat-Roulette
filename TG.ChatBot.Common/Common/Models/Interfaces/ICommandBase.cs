using Telegram.Bot;
using Telegram.Bot.Types;

namespace TG.ChatBot.Common.Models.Interfaces
{
    public interface ICommandBase
    {
        /// <summary>
        /// Название команды
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Список слов/команд для вызова команды
        /// </summary>
        List<string> Triggers { get; set; }

        /// <summary>
        /// Вызывает обработку команды
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task Execute(Update update);
    }
}
