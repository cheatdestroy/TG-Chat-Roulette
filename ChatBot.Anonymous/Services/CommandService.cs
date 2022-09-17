using ChatBot.Anonymous.Models.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChatBot.Anonymous.Services
{
    /// <summary>
    /// Сервис команд
    /// </summary>
    public class CommandService : ICommandService
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandService(IServiceProvider serviceProvider, ITelegramBotClient botClient)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Поиск команды по отправленному сообщению и её последующий вызов
        /// </summary>
        /// <param name="message"></param>
        public async Task SearchAndExecuteCommand(Message message)
        {
            using var scope = _serviceProvider.CreateAsyncScope();
            var commands = scope.ServiceProvider.GetServices<ICommandBase>();

            var command = commands?.FirstOrDefault(x =>
            {
                return x.Triggers.Any(trigger => trigger.Equals(message.Text));
            });

            if (command != null)
            {
                await command.Execute(message);
            }
        }
    }
}
