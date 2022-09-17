using Telegram.Bot.Types;

namespace ChatBot.Anonymous.Models.Interfaces
{
    public interface ICommandService
    {
        /// <summary>
        /// Поиск команды по отправленному сообщению и её последующий вызов
        /// </summary>
        /// <param name="message"></param>
        Task SearchAndExecuteCommand(Message message);
    }
}
