using ChatBot.Anonymous.Models.Interfaces;

namespace ChatBot.Anonymous.Services.CollectionExtension
{
    /// <summary>
    /// Расширения для DI
    /// </summary>
    public static class CommandsServiceCollectionExtension
    {
        /// <summary>
        /// Создаёт конфигурацию бота и регистрирует указанные команды
        /// </summary>
        /// <param name="services"> DI </param>
        /// <param name="commands"> Список команд </param>
        /// <returns></returns>
        public static IServiceCollection AddCommands(this IServiceCollection services, List<ICommandBase> commands)
        {
            services.AddTransient(serviceProvider =>
            {
                var configureCommand = new ConfigureCommand();

                configureCommand.CommandsList.AddRange(commands);

                return configureCommand;
            });

            return services;
        }
    }
}
