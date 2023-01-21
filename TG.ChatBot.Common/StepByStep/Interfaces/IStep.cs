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
        /// <param name="user"> Пользователь </param>
        /// <returns></returns>
        Task Execute(Domain.Entities.User user);

        /// <summary>
        /// Обработка шага
        /// </summary>
        /// <param name="data"> Полученные данные от пользователя </param>
        /// <param name="user"> Пользователь </param>
        /// <param name="action"> Динамическое переключение шагов (Пользователь, текущий шаг, новый шаг)</param>
        /// <example> Переключение шагов подразумевает смену шага на указанный </example>
        /// <returns></returns>
        Task Processing(string data, Domain.Entities.User user, Action<Domain.Entities.User, IStep, Step> action);
    }
}
