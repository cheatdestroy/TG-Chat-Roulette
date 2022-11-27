using TG.ChatBot.Common.StepByStep.Enums;
using TG.ChatBot.Common.StepByStep.Interfaces;
using TG.ChatBot.Host.Services.StepsByStep.Steps;

namespace TG.ChatBot.Host.Services.StepByStep.Actions
{
    public class ActionSteps : IActionSteps
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly List<IStep> _steps;

        public ActionSteps(IServiceProvider serviceProvider)
        {
            _steps = new List<IStep>();
            _serviceProvider = serviceProvider;
        }

        public virtual IStep? AddStep(Step stepId, int? stepIndex = null)
        {
            var isDuplicateStep = _steps.Select(x => x.Id == stepId)
                .FirstOrDefault();

            if (!isDuplicateStep)
            {
                var newStep = _serviceProvider.GetServices<IStep>()
                    .FirstOrDefault(x => x.Id == stepId);

                if (newStep != null)
                {
                    _steps.Add(newStep);

                    return newStep;
                }
            }

            return null;
        }

        public virtual IStep? GetStepById(Step stepId)
        {
            return _steps.FirstOrDefault(x => x.Id == stepId);
        }

        public virtual int? GetStepPosition(Step stepId)
        {
            var index = _steps.FindIndex(x => x.Id == stepId);

            return index != -1 ? index : null;
        }

        public virtual IStep? GetDefaultStep()
        {
            return _steps.FirstOrDefault();
        }

        public virtual IStep? GetNextStep(Step stepId)
        {
            var index = _steps.FindIndex(x => x.Id == stepId);

            if (index < 0 || ++index >= _steps.Count)
            {
                return null;
            }

            return _steps[index];
        }

        public virtual IStep? GetPreviousStep(Step stepId)
        {
            var index = _steps.FindIndex(x => x.Id == stepId);

            if (index <= 0)
            {
                return null;
            }

            return _steps[--index];
        }
    }
}
