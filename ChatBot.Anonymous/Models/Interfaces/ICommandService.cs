using Telegram.Bot.Types;

namespace ChatBot.Anonymous.Models.Interfaces
{
    public interface ICommandService
    {
        /// <summary>
        /// Поиск команды по отправленному сообщению и её последующий вызов
        /// </summary>
        /// <param name="update"></param>
        Task SearchAndExecuteCommand(Update update);
    }
}
