using TG.ChatBot.Host.Domain.Repository.Interfaces;

namespace TG.ChatBot.Host.Services
{
    public class RepositoryService
    {
        public IUser User { get; }
        public ISettings Settings { get; }
        public IAction Action { get; }

        public RepositoryService(IUser user, ISettings settings, IAction action)
        {
            User = user;
            Settings = settings;
            Action = action;
        }
    }
}
