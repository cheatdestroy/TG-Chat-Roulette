using ChatBot.Anonymous.Models.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChatBot.Anonymous.Services
{
    /// <summary>
    /// Конфигуратор команд
    /// </summary>
    public class ConfigureCommand
    {
        /// <summary>
        /// Список зарегистрированных команд
        /// </summary>
        public List<ICommandBase> CommandsList { get; }

        public ConfigureCommand()
        {
            CommandsList = new List<ICommandBase>();
        }

        /// <summary>
        /// Поиск команды по отправленному сообщению и её последующий вызов
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SearchAndExecuteCommand(ITelegramBotClient botClient, Message message)
        {
            var command = CommandsList.FirstOrDefault(x => 
            {
                return x.Triggers.Any(trigger => trigger.Equals(message.Text));
            });

            if (command != null)
            {
                await command.Execute(botClient, message);
            }
        }
    }
}
