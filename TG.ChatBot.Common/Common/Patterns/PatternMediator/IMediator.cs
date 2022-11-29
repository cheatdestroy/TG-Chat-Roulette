using TG.ChatBot.Common.Domain.Entities;

namespace TG.ChatBot.Common.Common.Pattern
{
    public interface IMediator
    {
        Task Send(string message, long userId);
    }
}
