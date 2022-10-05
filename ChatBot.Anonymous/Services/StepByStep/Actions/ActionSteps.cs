using ChatBot.Anonymous.Common.Enums;
using ChatBot.Anonymous.Models;
using ChatBot.Anonymous.Services.StepByStep.Interfaces;

namespace ChatBot.Anonymous.Services.StepByStep.Actions
{
    public class ActionSteps : IActionSteps
    {
        private readonly List<IStep> _steps;

        public ActionSteps()
        {
            _steps = new List<IStep>();
        }

        public ActionSteps(params IStep[] steps)
        {
            _steps = steps.DistinctBy(x => x.Id).ToList();
        }

        public virtual IStep? AddStep(IStep step)
        {
            var isDuplicateStep = _steps.Select(x => x.Id == step.Id)
                .FirstOrDefault();

            if (isDuplicateStep)
            {
                throw new Exception($"It is not possible to add a new step because it has already been added ({step.Id})");
            }

            _steps.Add(step);

            return step;
        }

        public virtual IStep? GetStepById(Step stepId)
        {
            return _steps.FirstOrDefault(x => x.Id == stepId);
        }

        public virtual IStep? GetDefaultStep()
        {
            return _steps.FirstOrDefault();
        }

        public virtual IStep? GetNextStep(Step stepId)
        {
            var index = _steps.FindIndex(x => x.Id == stepId);
            
            if (++index >= _steps.Count)
            {
                return null;
            }

            return _steps[index];
        }

        public virtual IStep? GetPreviousStep(Step stepId)
        {
            var index = _steps.FindIndex(x => x.Id == stepId);

            if (--index < 0)
            {
                return null;
            }

            return _steps[index];
        }
    }
}
