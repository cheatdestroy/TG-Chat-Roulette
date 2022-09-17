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
        private readonly ITelegramBotClient _botClient;

        /// <summary>
        /// Список зарегистрированных команд
        /// </summary>
        public List<ICommandBase> CommandsList { get; }

        public CommandService(IServiceProvider serviceProvider, ITelegramBotClient botClient)
        {
            using var scope = serviceProvider.CreateScope();
            var commands = scope.ServiceProvider.GetServices<ICommandBase>();

            CommandsList = new List<ICommandBase>(commands);
            _botClient = botClient;
        }

        /// <summary>
        /// Поиск команды по отправленному сообщению и её последующий вызов
        /// </summary>
        /// <param name="message"></param>
        public async Task SearchAndExecuteCommand(Message message)
        {
            var command = CommandsList.FirstOrDefault(x =>
            {
                return x.Triggers.Any(trigger => trigger.Equals(message.Text));
            });

            if (command != null)
            {
                await command.Execute(_botClient, message);
            }
        }
    }
}
