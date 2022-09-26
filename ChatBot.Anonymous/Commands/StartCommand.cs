using ChatBot.Anonymous.Common.Enums;
using ChatBot.Anonymous.Common.Helpers;
using ChatBot.Anonymous.Domain.Repository.Interfaces;
using ChatBot.Anonymous.Models.Interfaces;
using ChatBot.Anonymous.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatBot.Anonymous.Commands
{
    public class StartCommand : ICommandBase
    {
        private readonly IActionService _actionService;
        public string Name => "Инициализация";
        public List<string> Triggers { get; set; }

        public StartCommand(IActionService actionService)
        {
            Triggers = new List<string>
            {
                "/start"
            };

            _actionService = actionService;
        }

        public async Task Execute(Update update)
        {
            if (update.Message?.Chat.Type != ChatType.Private)
            {
                return;
            }

            await _actionService.ExecuteAction(update: update, action: CommandActions.StartAction);
        }
    }
}
