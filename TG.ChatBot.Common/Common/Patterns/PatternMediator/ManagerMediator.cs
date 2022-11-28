using TG.ChatBot.Common.Domain.Entities;

namespace TG.ChatBot.Common.Common.Pattern
{
    public class ManagerMediator : Mediator
    {
        public Interlocutor FirstUser { get; set; } = null!;
        public Interlocutor SecondUser { get; set; } = null!;

        public override async Task Send(string message, long userId)
        {
            if (FirstUser.Info.UserId == userId)
            {
                await SecondUser.ReceiveMessage(message);
            }
            else if (SecondUser.Info.UserId == userId)
            {
                await FirstUser.ReceiveMessage(message);
            }
        }
    }
}
