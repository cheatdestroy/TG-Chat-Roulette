using TG.ChatBot.Common.Domain.Entities;

namespace TG.ChatBot.Common.Common.Pattern
{
    public class ChatRoomMediator : IMediator
    {
        public Interlocutor FirstUser { get; private set; }
        public Interlocutor SecondUser { get; private set; }

        public ChatRoomMediator(Interlocutor firstUser, Interlocutor secondUser)
        {
            FirstUser = firstUser;
            FirstUser.SetMediator(this);
            SecondUser = secondUser;
            SecondUser.SetMediator(this);
        }

        public async Task Send(string message, long userId)
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
