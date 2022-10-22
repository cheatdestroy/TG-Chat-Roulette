using TG.ChatBot.Common.Domain.Entities;

namespace TG.ChatBot.Common.ChatHub.Models
{
    public interface IChatHub
    {
        /// <summary>
        /// Добавляет пользователя в пул поиска
        /// </summary>
        /// <param name="user"> Пользователь </param>
        /// <returns> Возвращает пользователя, если был добавлен в пул поиска; иначе null </returns>
        User? AddUserInSearchPool(User user);

        /// <summary>
        /// Удаляет пользователя из пула поиска
        /// </summary>
        /// <param name="userId"> Уникальный идентификатор пользователя </param>
        /// <returns> Возвращает пользователя, если он был удален; иначе null </returns>
        User? RemoveUserFromSearchPool(long userId);

        /// <summary>
        /// Запускает общение между двумя пользователями
        /// </summary>
        /// <param name="firstUser"> Первый пользователь </param>
        /// <param name="secondUser"> Второй пользователь </param>
        /// <returns> Возвращает комнату чата, если она была создана; иначе null </returns>
        ChatRoom? StartChat(User firstUser, User secondUser);

        /// <summary>
        /// Закрывает общение между двумя пользователями
        /// </summary>ц
        /// <param name="initiatorId"> Уникальный идентификатор инициатора закрытия общения </param>
        /// <returns> Возвращает комнату чата, если общение между пользователями закрылось; иначе null </returns>
        ChatRoom? EndChat(long initiatorId);

        /// <summary>
        /// Производит поиск собеседника для указанного пользователя с совпадением критериев двух пользователей
        /// </summary>
        /// <param name="initiator"> Инициатор поиска </param>
        /// <param name="startRoom"> Начинать общение, если собеседник будет найден? </param>
        /// <returns> Возвращает собеседника, если он был найден; иначе null </returns>
        User? FindInterlocutor(User initiator, bool startRoom = false);

        /// <summary>
        /// Проверяет пользователя на его нахождении в пуле поиска
        /// </summary>
        /// <param name="userId"> Пользователь </param>
        /// <returns> Возвращает true, если пользователь находится в пуле поиска; иначе false </returns>
        bool IsUserInSearchPool(long userId);

        /// <summary>
        /// Проверяет пользователя на его нахождении в пуле общения с другим пользователем
        /// </summary>
        /// <param name="userId"> Пользователь </param>
        /// <returns> Возвращает true, если пользователь находится в общении с другим пользователем; иначе false </returns>
        bool IsUserInChatRoom(long userId);
    }
}
