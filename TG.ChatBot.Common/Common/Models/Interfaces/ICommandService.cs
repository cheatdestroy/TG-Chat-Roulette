using Telegram.Bot.Types;

namespace TG.ChatBot.Common.Models.Interfaces
{
    public interface ICommandService
    {
        /// <summary>
        /// Поиск команды по отправленному сообщению и её последующий вызов
        /// </summary>
        /// <param name="update"></param>
        Task<bool> ExecuteCommand(Update update);
    }
}
