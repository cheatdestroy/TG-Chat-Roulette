using System.ComponentModel;

namespace ChatBot.Anonymous.Common.Enums
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
