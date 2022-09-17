using ChatBot.Anonymous.Common.Helpers;
using ChatBot.Anonymous.Domain.Repository.Interfaces;
using ChatBot.Anonymous.Models.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatBot.Anonymous.Commands
{
    public class StartCommand : ICommandBase
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IUser _user;

        public string Name => "Инициализация";

        public List<string> Triggers { get; set; }

        public StartCommand(ITelegramBotClient botClient, IUser user)
        {
            Triggers = new List<string>
            {
                "/start"
            };

            _botClient = botClient;
            _user = user;
        }

        public async Task Execute(Message message)
        {
            var (userId, chatId, _, _) = CommandHelper.GetRequiredParams(message);

            if (message.Chat.Type != ChatType.Private || !userId.HasValue)
            {
                return;
            }

            var user = await _user.GetById(userId: userId.Value) ?? await _user.SaveUser(userId: userId.Value);

            await _botClient.SendTextMessageAsync(chatId, $"{user!.UserId} | {chatId}");
        }
    }
}
