namespace TG.ChatBot.Host.Models
{
    /// <summary>
    /// Конфигурация бота
    /// </summary>
    public class BotConfiguration
    {
        /// <summary>
        /// Название бота
        /// </summary>
        public string BotName { get; set; } = default!;

        /// <summary>
        /// Адрес расположения метода контроллера для вебкуха
        /// </summary>
        public string Url { get; set; } = default!;

        /// <summary>
        /// Токен бота
        /// </summary>
        public string Token { get; set; } = default!;

        /// <summary>
        /// Лимит получаемых данных по умолчанию
        /// </summary>
        public int DefaultLimit { get; set; }

        /// <summary>
        /// Смещение получаемых данных по умолчанию
        /// </summary>
        public int DefaultOffset { get; set; }

        /// <summary>
        /// Максимально возможный возраст для общения
        /// </summary>
        public int MaximumAge { get; set; }

        /// <summary>
        /// Минимально возможный возраст для общения
        /// </summary>
        public int MinimumAge { get; set; }
    }
}
