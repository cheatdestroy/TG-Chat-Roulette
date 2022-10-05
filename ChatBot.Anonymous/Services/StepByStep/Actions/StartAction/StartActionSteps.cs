
using ChatBot.Anonymous.Services.StepsByStep.Steps;

namespace ChatBot.Anonymous.Services.StepByStep.Actions.StartAction
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
