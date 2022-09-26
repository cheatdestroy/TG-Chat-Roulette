using ChatBot.Anonymous.Commands;
using ChatBot.Anonymous.Models.Interfaces;

namespace ChatBot.Anonymous.Services.CollectionExtension
{
    /// <summary>
    /// Расширения для DI
    /// </summary>
    public static class CommandsServiceCollectionExtension
    {
        /// <summary>
        /// Добавляет сервис команд в контейнер DI
        /// </summary>
        /// <typeparam name="T"> Сервис команд </typeparam>
        /// <param name="services"> Контейнер </param>
        /// <returns></returns>
        public static IServiceCollection AddCommandService<T>(this IServiceCollection services) 
            where T : class, ICommandService
        {
            services.AddSingleton<ICommandService, T>();

            return services;
        }

        /// <summary>
        /// Добавляет команды в контейнер DI
        /// </summary>
        /// <typeparam name="T"> Команда </typeparam>
        /// <param name="services"> Контейнер </param>
        /// <returns></returns>
        public static IServiceCollection AddCommand<T>(this IServiceCollection services) 
            where T : class, ICommandBase
        {
            var command = services.FirstOrDefault(x => x.ImplementationType == typeof(T));

            if (command != null)
            {
                services.Remove(command);
            }

            services.AddTransient<ICommandBase, T>();

            return services;
        }
    }
}
