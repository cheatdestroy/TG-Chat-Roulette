using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using User = TG.ChatBot.Common.Domain.Entities.User;

namespace TG.ChatBot.Common.Common.Pattern
{
    public class Interlocutor
    {
        private readonly ITelegramBotClient _botClient;

        protected IMediator mediator = null!;

        public User Info { get; set; }

        public Interlocutor(User info, ITelegramBotClient botClient)
        {
            Info = info;
            _botClient = botClient;
        }

        public void SetMediator(IMediator mediator) => this.mediator = mediator;

        public virtual void Send(string message)
        {
            mediator.Send(message, Info.UserId);
        }

        public virtual async Task ReceiveMessage(string message)
        {
            await _botClient.SendTextMessageAsync(
                chatId: Info.UserId,
                text: $"{Info.UserId}: {message}",
                parseMode: ParseMode.Markdown);
        }
    }
}
