using Telegram.Bot.Types;
using TG.ChatBot.Common.StepByStep.Enums;

namespace TG.ChatBot.Common.StepByStep.Interfaces
{
    public interface IAction
    {
        /// <summary>
        /// Идентификатор действия
        /// </summary>
        CommandActions Action { get; }

        /// <summary>
        /// Шаги действия
        /// </summary>
        IActionSteps Steps { get; }

        /// <summary>
        /// Вызывает пошаговую инициализацию
        /// </summary>
        /// <param name="user"> Пользователь </param>
        /// <returns></returns>
        Task ExecuteSteps(Domain.Entities.User user);

        /// <summary>
        /// Обрабатывает пошаговое получение данных
        /// </summary>
        /// <param name="update"></param>
        /// <param name="user"> Пользователь </param>
        /// <returns></returns>
        Task ProcessingSteps(Update update, Domain.Entities.User user);

        /// <summary>
        /// Завершает выполнение текущего действия
        /// </summary>
        /// <param name="user"> Пользователь </param>
        /// <returns></returns>
        Task FinishAction(Domain.Entities.User user);

        /// <summary>
        /// Изменяет текущий шаг действия на указанный
        /// </summary>
        /// <param name="userId"> Уникальный идентификатор пользователя </param>
        /// <param name="currentStepId"> Идентификатор текущего шага </param>
        /// <param name="nextStepId"> Идентификатор нового шага </param>
        /// <returns></returns>
        void ChangeStep(long userId, IStep currentStepId, Step nextStepId);
    }
}
