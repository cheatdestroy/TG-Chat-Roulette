using TG.ChatBot.Host.Models;

namespace TG.ChatBot.Host.Common.Helpers
{
    /// <summary>
    /// Вспомогательные функции для конфигураций
    /// </summary>
    public static class ConfigurationHelper
    {
        /// <summary>
        /// Наименование секции с конфигурацией бота
        /// </summary>
        /// <returns> Наименование секции </returns>
        public static string GetNameSectionMainConfiguration() => "BotConfiguration";

        /// <summary>
        /// Наименование секции с конфигурацией базы данных
        /// </summary>
        /// <returns> Наименование секции </returns>
        public static string GetNameSectionDatabaseConfiguration() => "DBConfiguration";

        /// <summary>
        /// Привязывает конфигурацию бота к объекту
        /// </summary>
        /// <param name="configuration"> Конфигурация </param>
        /// <returns></returns>
        public static BotConfiguration GetMainConfigurationToObject(this IConfiguration configuration)
        {
            return configuration.GetRequiredSection(GetNameSectionMainConfiguration()).Get<BotConfiguration>();
        }
    }
}
