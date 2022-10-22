using System.ComponentModel;

namespace TG.ChatBot.Common.Common.Enums
{
    /// <summary>
    /// Тип общения
    /// </summary>
    public enum CommunicationType
    {
        /// <summary>
        /// Обычное
        /// </summary>
        [Description("обычный")]
        Standart = 1,

        /// <summary>
        /// Только голосовые сообщения
        /// </summary>
        [Description("голосовые сообщения")]
        OnlyVoice = 2
    }
}
