using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TG.ChatBot.Common.Common.Helpers;
using TG.ChatBot.Common.Domain;
using TG.ChatBot.Common.StepByStep.Enums;
using TG.ChatBot.Common.StepByStep.Interfaces;

namespace TG.ChatBot.Host.Services.StepByStep.Actions
{
    public class ActionBase : IAction
    {
        protected readonly ILogger<ActionBase> _logger;
        protected readonly RepositoryService _repository;

        public virtual CommandActions Action { get; }
        public virtual IActionSteps Steps { get; protected set; }

        /// <summary>
        /// Динамическое добавление степов из базы данных (в случае их отсутствии при вызове метода ProcessingSteps)
        /// </summary>
        public bool IsDynamicSteps { get; protected set; }

        public ActionBase(
            CommandActions action,
            IActionSteps steps,
            ILogger<ActionBase> logger,
            RepositoryService repository)
        {
            Action = action;
            Steps = steps;

            _logger = logger;
            _repository = repository;
        }

        #region Execute/Processing steps
        public virtual async Task ExecuteSteps(Common.Domain.Entities.User user)
        {
            Argument.NotNull(user?.Action?.CurrentAction, "Current action is null");

            var currentStep = user.Action.CurrentStep?.ToEnum<Step>() ?? Steps.GetDefaultStep()?.Id;

            Argument.NotNull(currentStep, "Step id is null");

            try
            {
                var chatId = user.UserId;
                var currentStepData = Steps.GetStepById(stepId: currentStep.Value);

                if (currentStepData != null)
                {
                    await currentStepData.Execute(chatId: chatId);
                    await _repository.Action.SaveAction(
                        userId: user.UserId,
                        actionId: (int)Action,
                        stepId: (int)currentStep);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem with executing step");
                throw;
            }
        }

        public virtual async Task ProcessingSteps(Update update, Common.Domain.Entities.User user)
        {
            var currentStep = user.Action?.CurrentStep?.ToEnum<Step>();

            Argument.NotNull(currentStep, "Step id is null");

            var userId = user.UserId;
            var currentStepData = Steps.GetStepById(stepId: currentStep.Value);

            if (IsDynamicSteps && currentStepData == null)
            {
                currentStepData = Steps.AddStep(currentStep.Value);
            }

            Argument.NotNull(currentStepData, "Step is null");

            try
            {
                Message? message = null;
                var data = default(string);

                if (update.Type == UpdateType.Message)
                {
                    message = update.Message;
                    data = message?.Text;
                }
                else if (update.Type == UpdateType.CallbackQuery)
                {
                    message = update.CallbackQuery?.Message;
                    data = update.CallbackQuery?.Data;
                }

                if (string.IsNullOrEmpty(data) || message == null)
                {
                    return;
                }

                await currentStepData.Processing(data: data, userId: userId, ChangeStep);
                await SetNextStep(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Problem with processing step");
                throw;
            }
        }
        #endregion

        public virtual Task FinishAction(Common.Domain.Entities.User user) 
        {
            throw new NotImplementedException("FinishAction is not implemented");
        }

        public virtual void ChangeStep(long userId, IStep currentStepId, Step nextStepId)
        {
            var isFound = Steps.GetStepById(nextStepId);

            if (isFound == null)
            {
                var currentStepPosition = Steps.GetStepPosition(currentStepId.Id);

                if (currentStepPosition.HasValue)
                {
                    var nextStepPosition = currentStepPosition.Value + 1; // следующая позиция
                    Steps.AddStep(nextStepId, nextStepPosition);
                }
            }
        }

        protected virtual async Task SetNextStep(long userId, Step? nextStepId = null)
        {
            var user = await _repository.User.GetById(userId: userId);

            Argument.NotNull(user?.Action, "Action is null");

            IStep? nextStep = default;

            if (nextStepId == null)
            {
                var currentStep = user.Action.CurrentStep?.ToEnum<Step>();

                if (currentStep != null)
                {
                    nextStep = Steps.GetNextStep(stepId: currentStep.Value);
                }
            }
            else
            {
                nextStep = Steps.GetStepById(nextStepId.Value);
            }

            if (nextStep != null)
            {
                user.Action.CurrentStep = (int?)nextStep.Id;
                await ExecuteSteps(user);
            }
            else
            {
                user.Action = await _repository.Action.SaveAction(userId: userId);
                await FinishAction(user);
            }
        }
    }
}
