using TG.ChatBot.Common.Domain.Entities;

namespace TG.ChatBot.Common.Common.Pattern
{
    public abstract class Mediator
    {
        public abstract Task Send(string message, long userId);
    }
}
