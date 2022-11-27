using Telegram.Bot.Types;
using TG.ChatBot.Common.Models.Interfaces;
using TG.ChatBot.Common.StepByStep.Enums;
using TG.ChatBot.Common.StepByStep.Interfaces;

namespace TG.ChatBot.Host.Commands
{
    public class ProfileCommand : ICommandBase
    {
        private readonly IActionService _actionService;

        public string Name => "Профиль";
        public List<string> Triggers { get; set; }

        public ProfileCommand(IActionService actionService)
        {
            Triggers = new List<string>
            {
                "/profile"
            };

            _actionService = actionService;
        }

        public async Task Execute(Update update)
        {
            await _actionService.ExecuteAction(update: update, action: CommandActions.ProfileAction);
        }
    }
}
