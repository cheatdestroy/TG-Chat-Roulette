using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TG.ChatBot.Common.Common.Helpers;
using TG.ChatBot.Common.Domain;
using TG.ChatBot.Common.StepByStep.Enums;
using TG.ChatBot.Common.StepByStep.Interfaces;

namespace TG.ChatBot.Host.Services.StepByStep
{
    /// <summary>
    /// Сервис step-by-step
    /// </summary>
    public class ActionService : IActionService
    {
        private readonly ILogger<ActionService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ActionService(ILogger<ActionService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
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
                _logger.LogWarning("UserId is null");
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
                _logger.LogWarning("User current action after saving is null");
                return;
            }

            var actionId = user.Action.CurrentAction?.ToEnum<CommandActions>();

            if (!actionId.HasValue)
            {
                _logger.LogWarning("User current action is null");
                return;
            }

            var actions = scope.ServiceProvider.GetServices<IAction>();
            var commandAction = actions.FirstOrDefault(x => x.Action == actionId);

            if (commandAction != null)
            {
                try
                {
                    if (user.Action.CurrentStep.HasValue)
                    {
                        await commandAction.ProcessingSteps(update: update, user: user);
                    }
                    else
                    {
                        await commandAction.ExecuteSteps(user: user);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Problem with action service");
                }
            }
        }
    }
}
