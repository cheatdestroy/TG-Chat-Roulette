using ChatBot.Anonymous.Domain.Entities;

namespace ChatBot.Anonymous.Domain.Repository.Interfaces
{
    public interface IUser
    {
        /// <summary>
        /// Сохраняет пользователя
        /// </summary>
        /// <param name="userId"> Уникальный идентификатор </param>
        /// <returns></returns>
        Task<User> SaveUser(long userId);

        /// <summary>
        /// Получает пользователя по уникальному идентификатору
        /// </summary>
        /// <param name="userId"> Уникальный идентификатор </param>
        /// <returns> Возвращает информацию о найденном пользователе </returns>
        Task<User?> GetById(long userId);

        /// <summary>
        /// Получает список пользователей
        /// </summary>
        /// <param name="limit"> Лимит </param>
        /// <param name="offset"> Смещение </param>
        /// <returns> Возвращает сформированный запрос </returns>
        IQueryable<User> Get(
            int? limit = null,
            int? offset = null);
    }
}
