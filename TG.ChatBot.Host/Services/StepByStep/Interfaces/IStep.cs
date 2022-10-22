using TG.ChatBot.Host.Common.Enums;

namespace TG.ChatBot.Host.Services.StepByStep.Interfaces
{
    public interface IStep
    {
        Step Id { get; }
        Task Execute(long chatId);
        Task Processing(string data, long userId);
    }
}
