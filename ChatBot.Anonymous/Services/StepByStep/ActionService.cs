using ChatBot.Anonymous.Common.Enums;
using ChatBot.Anonymous.Common.Helpers;
using ChatBot.Anonymous.Domain.Repository.Interfaces;
using ChatBot.Anonymous.Services.StepByStep.Interfaces;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatBot.Anonymous.Services.StepByStep
{
    /// <summary>
    /// Сервис step-by-step
    /// </summary>
    public class ActionService : IActionService
    {
        private readonly IServiceProvider _serviceProvider;

        public ActionService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Обрабатывает шаги указанного/сохраненного действия
        /// </summary>
        /// <param name="update"></param>
        /// <param name="action"> Номер действия </param>
        /// <returns></returns>
        public async Task ExecuteAction(Update update, CommandActions? action)
        {
            var userId = update.Type switch
            {
                UpdateType.Message => update.Message?.From?.Id,
                UpdateType.CallbackQuery => update.CallbackQuery?.From.Id,
                _ => null
            };

            if (userId == null)
            {
                return;
            }

            using var scope = _serviceProvider.CreateAsyncScope();
            var repositoryService = scope.ServiceProvider.GetRequiredService<RepositoryService>();

            var user = await repositoryService.User.GetById(userId: userId.Value)
                ?? await repositoryService.User.SaveUser(userId: userId.Value);

            if (action.HasValue)
            {
                user.Action = await repositoryService.Action.SaveAction(userId: userId.Value, actionId: (int?)action);
            }

            if (user?.Action?.CurrentAction == null)
            {
                return;
            }

            var actionId = user.Action.CurrentAction.ToEnum<CommandActions>();

            if (!actionId.HasValue)
            {
                return;
            }

            var actions = scope.ServiceProvider.GetServices<Interfaces.IAction>();
            var commandAction = actions.FirstOrDefault(x => x.Action == actionId);

            if (commandAction != null)
            {
                try
                {
                    if (user.Action.CurrentStep.HasValue)
                    {
                        await commandAction.ProcessingSteps(update: update, user: user);
                    }
                    else if (update.Message != null)
                    {
                        await commandAction.ExecuteSteps(message: update.Message, user: user);
                    }
                }
                catch
                {
                }
            }
        }
    }
}
