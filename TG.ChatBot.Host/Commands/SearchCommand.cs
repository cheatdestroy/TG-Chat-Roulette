﻿using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TG.ChatBot.Common.ChatHub.Models;
using TG.ChatBot.Common.Common.Helpers;
using TG.ChatBot.Common.Domain;
using TG.ChatBot.Common.Models.Interfaces;
using TG.ChatBot.Host.Services.Communication;

namespace TG.ChatBot.Host.Commands
{
    /// <summary>
    /// Команда поиска собеседника
    /// </summary>
    public class SearchCommand : ICommandBase
    {
        private readonly IChatHub _chatHub;
        private readonly ITelegramBotClient _botClient;
        private readonly RepositoryService _repository;

        public string Name => "Поиск собеседника";
        public List<string> Triggers { get; set; }

        public SearchCommand(IServiceProvider serviceProvider, ITelegramBotClient botClient, RepositoryService repository)
        {
            Triggers = new List<string>
            {
                "/find",
                "/search"
            };

            _chatHub = ChatHub.GetInstance(serviceProvider);
            _botClient = botClient;
            _repository = repository;
        }

        public async Task Execute(Update update)
        {
            var userId = update.Message?.From?.Id;

            if (!userId.HasValue || _chatHub.IsUserInChatRoom(userId.Value))
            {
                return;
            }

            try
            {
                var user = await _repository.User.GetById(userId: userId.Value);

                UserHelper.CheckUserValidFields(user);

                var isAlreadyInSearch = _chatHub.IsUserInSearchPool(userId.Value);

                var textMessage = new StringBuilder(isAlreadyInSearch
                    ? "Вы уже находитесь в поиске"
                    : "Идёт поиск собеседника");
                textMessage.Append("\n\nОстановить поиск 👉🏻 /stop");

                await _botClient.SendTextMessageAsync(
                    chatId: userId,
                    text: textMessage.ToString(),
                    parseMode: ParseMode.Markdown);

                if (!isAlreadyInSearch)
                {
                    var potentialUser = _chatHub.FindInterlocutor(user, true);

                    if (potentialUser == null)
                    {
                        _chatHub.AddUserInSearchPool(user);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
