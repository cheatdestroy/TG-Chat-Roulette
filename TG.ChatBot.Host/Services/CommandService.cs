using Telegram.Bot.Types;
using TG.ChatBot.Common.Models.Interfaces;

namespace TG.ChatBot.Host.Services
{
    /// <summary>
    /// Сервис команд
    /// </summary>
    public class CommandService : ICommandService
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Поиск команды по отправленному сообщению и её последующий вызов
        /// </summary>
        /// <param name="update"></param>
        public async Task<bool> ExecuteCommand(Update update)
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

            return command != null;
        }
    }
}
