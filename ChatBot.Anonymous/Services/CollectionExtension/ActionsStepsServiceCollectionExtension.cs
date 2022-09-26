using ChatBot.Anonymous.Models.Interfaces;
using ChatBot.Anonymous.Common.Enums;

namespace ChatBot.Anonymous.Services.CollectionExtension
{
    /// <summary>
    /// Расширения для DI
    /// </summary>
    public static class ActionsStepsServiceCollectionExtension
    {
        /// <summary>
        /// Добавляет сервис действий в контейнер DI
        /// </summary>
        /// <typeparam name="T"> Сервис действий </typeparam>
        /// <param name="services"> Контейнер </param>
        /// <returns></returns>
        public static IServiceCollection AddActionService<T>(this IServiceCollection services) 
            where T : class, IActionService
        {
            services.AddSingleton<IActionService, T>();

            return services;
        }

        /// <summary>
        /// Добавляет действия в контейнер DI
        /// </summary>
        /// <typeparam name="T"> Действие </typeparam>
        /// <param name="services"> Контейнер </param>
        /// <returns></returns>
        public static IServiceCollection AddAction<T>(this IServiceCollection services)
            where T : class, IActionSteps
        {
            var action = services.FirstOrDefault(x => x.ImplementationType == typeof(T));

            if (action != null)
            {
                services.Remove(action);
            }

            services.AddTransient<IActionSteps, T>();

            return services;
        }
    }
}
