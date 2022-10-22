using TG.ChatBot.Host.Domain.Entities;

namespace TG.ChatBot.Host.Domain.Repository.Interfaces
{
    public interface IAction
    {
        /// <summary>
        /// Сохраняет прогресс действия пользователя
        /// </summary>
        /// <param name="userId"> Уникальный идентификатор пользователя </param>
        /// <param name="actionId"> Номер действия </param>
        /// <param name="actionStep"> Номер шага </param>
        /// <returns> Возвращает прогресс действия </returns>
        Task<ActionData> SaveAction(
            long userId, 
            int? actionId = null, 
            int? stepId = null);

        /// <summary>
        /// Получает прогресс действий пользователя по уникальному идентификатору пользователя
        /// </summary>
        /// <param name="userId"> Уникальный идентификатор пользователя </param>
        /// <returns> Возвращает прогресс действия </returns>
        Task<ActionData?> GetByUserId(long userId);

        /// <summary>
        /// Получает список действий пользователей с текущим прогрессом
        /// </summary>
        /// <param name="limit"> Лимит </param>
        /// <param name="offset"> Смещение </param>
        /// <returns> Возвращает сформированный запрос </returns>
        IQueryable<ActionData> Get(
            int? limit = null,
            int? offset = null);
    }
}
