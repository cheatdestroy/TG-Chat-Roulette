using ChatBot.Anonymous.Common.Enums;
using Telegram.Bot.Types;

namespace ChatBot.Anonymous.Models.Interfaces
{
    public interface IActionSteps
    {
        /// <summary>
        /// Номер действия
        /// </summary>
        CommandActions Action { get; }

        /// <summary>
        /// Получает следующий шаг
        /// </summary>
        /// <param name="currentStep"> Текущий шаг </param>
        /// <returns> Возвращает следующий шаг </returns>
        int? GetNextStep(int? currentStep);

        /// <summary>
        /// Получает предыдущий шаг
        /// </summary>
        /// <param name="currentStep"> Текущий шаг </param>
        /// <returns> Возвращает предыдущий шаг </returns>
        int? GetPreviousStep(int? currentStep);

        /// <summary>
        /// Получает стандартный шаг в случае отсутствия шага
        /// </summary>
        /// <returns> Возвращает стандартный шаг </returns>
        int GetDefaultStep();

        /// <summary>
        /// Вызывает пошаговую инициализацию
        /// </summary>
        /// <param name="message"></param>
        /// <param name="user"> Пользователь </param>
        /// <returns></returns>
        Task ExecuteSteps(Message message, Domain.Entities.User user);

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
        /// <param name="message"></param>
        /// <param name="userId"> Уникальный идентификатор пользователя </param>
        /// <returns></returns>
        Task FinishAction(Message message, long userId);
    }
}
