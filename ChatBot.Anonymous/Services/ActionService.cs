using ChatBot.Anonymous.Common.Enums;
using ChatBot.Anonymous.Common.Helpers;
using ChatBot.Anonymous.Domain.Repository.Interfaces;
using ChatBot.Anonymous.Models.Interfaces;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatBot.Anonymous.Services
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
        /// Обрабатывает шаги указанного или сохраненного действия
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
            var repositoryServices = scope.ServiceProvider.GetRequiredService<RepositoryService>();

            var user = await repositoryServices.User.GetById(userId: userId.Value) 
                ?? await repositoryServices.User.SaveUser(userId: userId.Value); ;

            if (action.HasValue)
            {
                user.Action = await repositoryServices.Action.SaveAction(userId: userId.Value, actionId: (int?)action);
            }

            if (user?.Action?.CurrentAction == null)
            {
                return;
            }

            var actionId = user.Action.CurrentAction.Value;

            if (actionId.IsNotOwnerValue(typeof(CommandActions)))
            {
                return;
            }

            var actions = scope.ServiceProvider.GetServices<IActionSteps>();
            var commandAction = actions.FirstOrDefault(x => x.Action == (CommandActions)actionId);

            if (commandAction != null)
            {
                try
                {
                    if (user.Action.CurrentStep.HasValue)
                    {
                        await commandAction.ProcessingSteps(update, user);
                    }
                    else if (update.Message != null)
                    {
                        await commandAction.ExecuteSteps(update.Message, user);
                    }
                }
                catch
                {
                }
            }
        }
    }
}
