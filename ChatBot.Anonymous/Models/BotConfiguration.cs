namespace ChatBot.Anonymous.Models
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
    }
}
