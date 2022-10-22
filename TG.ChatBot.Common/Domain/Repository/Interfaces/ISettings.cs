using TG.ChatBot.Common.Domain.Entities;

namespace TG.ChatBot.Common.Domain.Repository.Interfaces
{
    public interface ISettings
    {
        /// <summary>
        /// Сохраняет настройки пользователя
        /// </summary>
        /// <param name="userId"> Уникальный идентификатор пользователя </param>
        /// <param name="preferredGender"> Предпочитаемый пол </param>
        /// <param name="preferredAge"> Предпочитаемый возраст </param>
        /// <param name="preferredChatType"> Предпочитаемый тип общения </param>
        /// <returns></returns>
        Task<UserSetting> SaveSetting(
            long userId,
            int? preferredGender = null,
            int? preferredAge = null,
            int? preferredChatType = null);

        /// <summary>
        /// Получает настройки пользователя
        /// </summary>
        /// <param name="userId"> Уникальный идентификатор пользователя </param>
        /// <returns> Возвращает настройки пользователя </returns>
        Task<UserSetting?> GetByUserId(long userId);

        /// <summary>
        /// Получает список настроек
        /// </summary>
        /// <param name="limit"> Лимит </param>
        /// <param name="offset"> Смещение </param>
        /// <returns> Возвращает сформированный запрос </returns>
        IQueryable<UserSetting> Get(
            int? limit = null,
            int? offset = null);
    }
}
