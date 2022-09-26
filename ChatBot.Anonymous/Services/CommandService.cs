using ChatBot.Anonymous.Domain.Repository.Interfaces;
using ChatBot.Anonymous.Models.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatBot.Anonymous.Services
{
    /// <summary>
    /// Сервис команд
    /// </summary>
    public class CommandService : ICommandService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IActionService _actionService;

        public CommandService(IServiceProvider serviceProvider, IActionService actionService)
        {
            _serviceProvider = serviceProvider;
            _actionService = actionService;
        }

        /// <summary>
        /// Поиск команды по отправленному сообщению и её последующий вызов
        /// </summary>
        /// <param name="update"></param>
        public async Task SearchAndExecuteCommand(Update update)
        {
            using var scope = _serviceProvider.CreateAsyncScope();
            var commands = scope.ServiceProvider.GetServices<ICommandBase>();

            var command = commands?.FirstOrDefault(x =>
            {
                return x.Triggers.Any(trigger => trigger.Equals(update.Message?.Text));
            });

            if (command != null)
            {
                await command.Execute(update: update);
            }
            else
            {
                await _actionService.ExecuteAction(update: update);
            }
        }
    }
}
