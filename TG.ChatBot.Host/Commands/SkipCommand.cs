using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TG.ChatBot.Common.ChatHub.Models;
using TG.ChatBot.Common.Common.Helpers;
using TG.ChatBot.Common.Models.Interfaces;
using TG.ChatBot.Host.Services.Communication;

namespace TG.ChatBot.Host.Commands
{
    public class SkipCommand : ICommandBase
    {
        private readonly IChatHub _chatHub;
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<SkipCommand> _logger;

        public string Name => "Завершить общение";
        public List<string> Triggers { get; set; }

        public SkipCommand(IServiceProvider serviceProvider, ILogger<SkipCommand> logger, ITelegramBotClient botClient)
        {
            Triggers = new List<string>()
            {
                "/skip",
                "/next"
            };

            _chatHub = ChatHub.GetInstance(serviceProvider);
            _botClient = botClient;
            _logger = logger;
        }

        public Task Execute(Update update)
        {
            var userId = update.GetSenderId();

            if (!userId.HasValue)
            {
                return Task.CompletedTask;
            }

            _chatHub.EndChat(userId.Value);

            return Task.CompletedTask;
        }
    }
}
