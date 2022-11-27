
using TG.ChatBot.Common.StepByStep.Enums;

namespace TG.ChatBot.Common.StepByStep.Interfaces
{
    public interface IActionSteps
    {
        /// <summary>
        /// Добавляет шаг в коллекцию
        /// </summary>
        /// <remarks> Последовательность добавления влияет на результаты получения следующего/предыдущего/по умолчанию шага </remarks>
        /// <param name="stepId"> Идентификатор шага </param>
        /// <param name="stepIndex"> Позиция элемента в коллекции. </param>
        /// <returns> Возвращает добавленный шаг </returns>
        IStep? AddStep(Step stepId, int? stepIndex = null);

        /// <summary>
        /// Получение шага по идентификатору шага
        /// </summary>
        /// <param name="stepId"> Идентификатор шага </param>
        /// <returns> Возвращает шаг </returns>
        IStep? GetStepById(Step stepId);

        /// <summary>
        /// Получение индекса шага в коллекции
        /// </summary>
        /// <param name="stepId"> Идентификатор шага </param>
        /// <returns></returns>
        int? GetStepPosition(Step stepId);

        /// <summary>
        /// Получает следующий шаг
        /// </summary>
        /// <param name="stepId"> Текущий шаг </param>
        /// <returns> Возвращает следующий шаг </returns>
        IStep? GetNextStep(Step stepId);

        /// <summary>
        /// Получает предыдущий шаг
        /// </summary>
        /// <param name="stepId"> Текущий шаг </param>
        /// <returns> Возвращает предыдущий шаг </returns>
        IStep? GetPreviousStep(Step stepId);

        /// <summary>
        /// Получает стандартный шаг в случае отсутствия шага
        /// </summary>
        /// <returns> Возвращает шаг по умолчанию </returns>
        IStep? GetDefaultStep();
    }
}
