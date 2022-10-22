using System.ComponentModel;

namespace TG.ChatBot.Common.Common.Enums
{
    /// <summary>
    /// Пол пользователя
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// Мужчина
        /// </summary>
        [Description("мужской")]
        Male = 1,

        /// <summary>
        /// Женщина
        /// </summary>
        [Description("женский")]
        Female = 2,

        /// <summary>
        /// Любой
        /// </summary>
        [Description("любой")]
        Any = 3
    }
}
