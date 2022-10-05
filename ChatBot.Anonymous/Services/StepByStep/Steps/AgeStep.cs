using ChatBot.Anonymous.Services;
using ChatBot.Anonymous.Common.Helpers;
using Telegram.Bot;
using ChatBot.Anonymous.Models;
using Telegram.Bot.Types.Enums;
using System.Text;
using ChatBot.Anonymous.Common.Enums;
using ChatBot.Anonymous.Services.StepByStep.Interfaces;

namespace ChatBot.Anonymous.Services.StepsByStep.Steps
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
