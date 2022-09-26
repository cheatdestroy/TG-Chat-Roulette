using ChatBot.Anonymous.Domain.Repository.Interfaces;

namespace ChatBot.Anonymous.Services
{
    public class RepositoryService
    {
        public IUser User { get; }
        public IAction Action { get; }

        public RepositoryService(IUser user, IAction action)
        {
            User = user;
            Action = action;
        }
    }
}
