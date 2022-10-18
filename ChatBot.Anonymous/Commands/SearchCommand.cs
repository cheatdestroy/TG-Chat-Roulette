using ChatBot.Anonymous.Common.Helpers;
using ChatBot.Anonymous.Models.Interfaces;
using ChatBot.Anonymous.Services;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatBot.Anonymous.Commands
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

        public SearchCommand(IChatHub chatHub, ITelegramBotClient botClient, RepositoryService repository)
        {
            Triggers = new List<string>
            {
                "/find",
                "/search"
            };

            _chatHub = chatHub;
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

                    if (potentialUser != null)
                    {
                        await NotifyUserFound(user.UserId);
                        await NotifyUserFound(potentialUser.UserId);
                    }
                    else
                    {
                        _chatHub.AddUserInSearchPool(user);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async Task NotifyUserFound(long chatId)
        {
            var textMessage = new StringBuilder("Собеседник найден!\n\n");
            textMessage.Append("Следующий собеседник 👉🏻 /next\n");
            textMessage.Append("Закончить диалог 👉🏻 /skip");

            await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: textMessage.ToString(),
                    parseMode: ParseMode.Markdown);
        }
    }
}
