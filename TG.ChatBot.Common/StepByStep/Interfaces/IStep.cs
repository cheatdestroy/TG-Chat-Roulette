using TG.ChatBot.Common.StepByStep.Enums;

namespace TG.ChatBot.Common.StepByStep.Interfaces
{
    public interface IStep
    {
        Step Id { get; }
        Task Execute(long chatId);
        Task Processing(string data, long userId);
    }
}
