using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using User = TG.ChatBot.Common.Domain.Entities.User;

namespace TG.ChatBot.Common.Common.Pattern
{
    public class Interlocutor
    {
        private readonly ITelegramBotClient _botClient;

        protected Mediator mediator;

        public User Info { get; set; }

        public Interlocutor(Mediator mediator, User info, ITelegramBotClient botClient)
        {
            this.mediator = mediator;
            Info = info;
            _botClient = botClient;
        }

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
