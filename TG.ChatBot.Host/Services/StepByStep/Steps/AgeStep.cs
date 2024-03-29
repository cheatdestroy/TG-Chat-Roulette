﻿using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TG.ChatBot.Common.Common.Helpers;
using TG.ChatBot.Common.Domain;
using TG.ChatBot.Common.Domain.Entities;
using TG.ChatBot.Common.Models;
using TG.ChatBot.Common.StepByStep.Enums;
using TG.ChatBot.Common.StepByStep.Interfaces;

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

        public async Task Execute(User user)
        {
            var textMessage = new StringBuilder("🎂 Введите ваш возраст");
            await _botClient.SendTextMessageAsync(
                chatId: user.UserId,
                text: textMessage.ToString(),
                parseMode: ParseMode.Markdown);
        }

        public async Task Processing(string data, User user, Action<User, IStep, Step> action)
        {
            var age = int.Parse(data);
            var minimumAge = _configuration.MinimumAge;
            var maximumAge = _configuration.MaximumAge;

            await Argument.OutOfRange(
                value: age,
                maxValue: maximumAge,
                minValue: minimumAge,
                message: $"_Возраст не может быть меньше {minimumAge} или больше {maximumAge}_",
                chatId: user.UserId,
                botClient: _botClient);

            await _repository.User.SaveUser(userId: user.UserId, age: age);
        }
    }
}
