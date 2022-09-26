using ChatBot.Anonymous.Common.Helpers;
using ChatBot.Anonymous.Models.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChatBot.Anonymous.Commands
{
    /// <summary>
    /// Команда для теста
    /// </summary>
    public class TestCommand : ICommandBase
    {
        public string Name => "Тестовая команда";
        public List<string> Triggers { get; set; }

        public TestCommand()
        {
            Triggers = new List<string>
            {
                "/test",
                "/unk"
            };
        }

        public async Task Execute(Update update)
        {
        }
    }
}
