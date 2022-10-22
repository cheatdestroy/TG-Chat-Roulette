using TG.ChatBot.Host.Services;
using TG.ChatBot.Host.Common.Helpers;
using Telegram.Bot;
using TG.ChatBot.Host.Models;
using Telegram.Bot.Types.Enums;
using System.Text;
using TG.ChatBot.Host.Common.Enums;
using TG.ChatBot.Host.Services.StepByStep.Interfaces;

namespace TG.ChatBot.Host.Services.StepsByStep.Steps
{
    /// <summary>
    /// Шаг заполнения возраста пользователя
    /// </summary>
    public class AgeStep : IStep
    {
        private readonly ITelegramBotClient _botClient;
        private readonly BotConfiguration _configuration;
        private readonly RepositoryService _repository;

        public Step Id { get; } = Step.Age;

        public AgeStep(ITelegramBotClient botClient, IConfiguration configuration, RepositoryService repository)
        {
            _botClient = botClient;
            _configuration = configuration.GetMainConfigurationToObject();
            _repository = repository;
        }

        public async Task Execute(long chatId)
        {
            var textMessage = new StringBuilder("Введите ваш возраст");
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: textMessage.ToString(),
                parseMode: ParseMode.Markdown);
        }

        public async Task Processing(string data, long userId)
        {
            var age = int.Parse(data);
            var minimumAge = _configuration.MinimumAge;
            var maximumAge = _configuration.MaximumAge;

            await Argument.OutOfRange(
                value: age,
                maxValue: maximumAge,
                minValue: minimumAge,
                message: $"_Возраст не может быть меньше {minimumAge} или больше {maximumAge}_",
                chatId: userId,
                botClient: _botClient);

            await _repository.User.SaveUser(userId: userId, age: age);
        }
    }
}
