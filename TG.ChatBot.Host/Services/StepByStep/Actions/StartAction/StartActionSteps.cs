
using TG.ChatBot.Host.Services.StepsByStep.Steps;

namespace TG.ChatBot.Host.Services.StepByStep.Actions.StartAction
{
    /// <summary>
    /// Последовательность шагов для действия Start (/start)
    /// </summary>
    public class StartActionSteps : ActionSteps
    {
        public StartActionSteps(
            GenderStep genderStep,
            AgeStep ageStep,
            PreferredChatTypeStep preferredChatTypeStep,
            PreferredGenderStep preferredGenderStep,
            PreferredAgeStep preferredAgeStep)
        {
            AddStep(genderStep);
            AddStep(ageStep);
            AddStep(preferredChatTypeStep);
            AddStep(preferredGenderStep);
            AddStep(preferredAgeStep);
        }
    }
}
