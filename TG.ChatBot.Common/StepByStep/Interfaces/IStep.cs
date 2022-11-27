using TG.ChatBot.Common.StepByStep.Enums;

namespace TG.ChatBot.Common.StepByStep.Interfaces
{
    public interface IStep
    {
        /// <summary>
        /// Идентификатор шага
        /// </summary>
        Step Id { get; }

        /// <summary>
        /// Вызов шага
        /// </summary>
        /// <param name="chatId"> Идентификатор чата </param>
        /// <returns></returns>
        Task Execute(long chatId);

        /// <summary>
        /// Обработка шага
        /// </summary>
        /// <param name="data"> Полученные данные от пользователя </param>
        /// <param name="userId"> Пользователь </param>
        /// <param name="action"> Динамическое переключение шагов (Идентификатор пользователя, текущий шаг, новый шаг)</param>
        /// <example> Переключение шагов подразумевает смену шага на указанный </example>
        /// <returns></returns>
        Task Processing(string data, long userId, Action<long, IStep, Step> action);
    }
}
