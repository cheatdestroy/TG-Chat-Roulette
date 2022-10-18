using ChatBot.Anonymous.Common.Enums;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = ChatBot.Anonymous.Domain.Entities.User;

namespace ChatBot.Anonymous.Common.Helpers
{
    /// <summary>
    /// Вспомогательные функции для объекта User
    /// </summary>
    public static class UserHelper
    {
        /// <summary>
        /// Формирует информацию о пользователе
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static StringBuilder FormatUserInfo(this User user)
        {
            var gender = user.Gender.ToEnum<Gender>().GetDescription();
            var chatType = user.UserSetting?.PreferredChatType.ToEnum<CommunicationType>().GetDescription();
            var preferredGender = user.UserSetting?.PreferredGender.ToEnum<Gender>().GetDescription();
            var preferredAge = user.UserSetting?.PreferredAge.ToEnum<AgeCategory>().GetDescription();

            var userInfo = new StringBuilder();
            userInfo.Append($"Ваш пол: {gender}\n");
            userInfo.Append($"Ваш возраст: {user.Age.ToString() ?? "неизвестно"}\n");
            userInfo.Append($"Предпочитаемый тип чата: {chatType}\n");
            userInfo.Append($"Предпочитаемый пол собеседника: {preferredGender}\n");
            userInfo.Append($"Предпочитаемый возраст собеседника: {preferredAge}");

            return userInfo;
        }

        /// <summary>
        /// Проверяет отправителя на возможность использовать бота
        /// </summary>
        /// <param name="message"></param>
        /// <returns> Возвращает true, если отправитель валидный; иначе false </returns>
        public static bool IsSenderValid(this Message? message)
        {
            return message != null
                && message.Chat?.Type == ChatType.Private;
        }

        /// <summary>
        /// Проверяет отправителя на возможность использовать бота
        /// </summary>
        /// <param name="update"></param>
        /// <returns> Возвращает true, если отправитель валидный; иначе false </returns>
        public static bool IsSenderValid(this Update? update)
        {
            var message = update?.Message ?? update?.CallbackQuery?.Message;
            return IsSenderValid(message);
        }

        /// <summary>
        /// Проверяет пользователя на корректность заполненных данных
        /// </summary>
        /// <param name="user"> Пользователь </param>
        public static void CheckUserValidFields([NotNull] User? user)
        {
            Argument.NotNull(user, "Пользователя не существует");
            Argument.NotNull(user.Gender, "Не заполнен пол");
            Argument.NotNull(user.Age, "Не заполнен возраст");
            Argument.NotNull(user.UserSetting?.PreferredChatType, "Не указан предпочитаемый тип общения");
            Argument.NotNull(user.UserSetting?.PreferredAge, "Не указан предпочитаемый возраст собеседника");
            Argument.NotNull(user.UserSetting?.PreferredGender, "Не указан предпочитаемый пол собеседника");
        }

        /// <summary>
        /// Получает уникальный идентификатор отправителя
        /// </summary>
        /// <param name="message"></param>
        /// <returns> Если найден возвращает уникальный идентификатор отправителя; иначе false </returns>
        public static long? GetSenderId(this Message? message)
        {
            return message?.From?.Id;
        }

        /// <summary>
        /// Получает уникальный идентификатор отправителя
        /// </summary>
        /// <param name="update"></param>
        /// <returns> Если найден возвращает уникальный идентификатор отправителя; иначе false </returns>
        public static long? GetSenderId(this Update? update)
        {
            return update?.Message?.From?.Id ?? update?.CallbackQuery?.From?.Id;
        }
    }
}
