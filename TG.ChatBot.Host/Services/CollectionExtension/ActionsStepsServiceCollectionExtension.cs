using TG.ChatBot.Common.StepByStep.Interfaces;

namespace TG.ChatBot.Host.Services.CollectionExtension
{
    /// <summary>
    /// Расширения для DI
    /// </summary>
    public static class ActionsStepsServiceCollectionExtension
    {
        /// <summary>
        /// Добавляет сервис действий (step-by-step) в контейнер DI
        /// </summary>
        /// <typeparam name="TActionService"> Сервис действий </typeparam>
        /// <param name="services"> Контейнер </param>
        /// <returns></returns>
        public static IServiceCollection AddActionService<TActionService>(this IServiceCollection services)
            where TActionService : class, IActionService
        {
            services.AddSingleton<IActionService, TActionService>();

            return services;
        }

        /// <summary>
        /// Добавляет действие в контейнер DI
        /// </summary>
        /// <typeparam name="TAction"> Действие </typeparam>
        /// <param name="services"> Контейнер </param>
        /// <returns></returns>
        public static IServiceCollection AddAction<TAction>(this IServiceCollection services)
            where TAction : class, IAction
        {
            var action = services.FirstOrDefault(x => x.ImplementationType == typeof(TAction));

            if (action != null)
            {
                services.Remove(action);
            }

            services.AddTransient<IAction, TAction>();

            return services;
        }

        /// <summary>
        /// Добавляет шаг в контейнер DI
        /// </summary>
        /// <typeparam name="TStep"> Шаг </typeparam>
        /// <param name="services"> Контейнер </param>
        /// <returns></returns>
        public static IServiceCollection AddStep<TStep>(this IServiceCollection services)
            where TStep : class, IStep
        {
            var step = services.FirstOrDefault(x => x.ImplementationType == typeof(TStep));

            if (step != null)
            {
                services.Remove(step);
            }

            services.AddTransient<TStep>();

            return services;
        }

        /// <summary>
        /// Добавляет последовательность шагов в контейнер DI
        /// </summary>
        /// <typeparam name="TActionSteps"> Последовательность шагов </typeparam>
        /// <param name="services"> Контейнер </param>
        /// <returns></returns>
        public static IServiceCollection AddActionSteps<TActionSteps>(this IServiceCollection services)
            where TActionSteps : class, IActionSteps
        {
            services.AddTransient<TActionSteps>();

            return services;
        }
    }
}
