using Telegram.Bot.Types;
using TG.ChatBot.Common.Models.Interfaces;
using TG.ChatBot.Common.StepByStep.Enums;
using TG.ChatBot.Host.Services.StepByStep.Interfaces;

namespace TG.ChatBot.Host.Commands
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
            await _actionService.ExecuteAction(update: update, action: CommandActions.StartAction);
        }
    }
}
