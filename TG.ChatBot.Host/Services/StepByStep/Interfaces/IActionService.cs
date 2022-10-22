using Telegram.Bot.Types;
using TG.ChatBot.Common.StepByStep.Enums;

namespace TG.ChatBot.Host.Services.StepByStep.Interfaces
{
    public interface IActionService
    {
        /// <summary>
        /// Обрабатывает шаги указанного или сохраненного действия
        /// </summary>
        /// <param name="update"></param>
        /// <param name="action"> Номер действия </param>
        /// <returns></returns>
        Task ExecuteAction(Update update, CommandActions? action = null);
    }
}
