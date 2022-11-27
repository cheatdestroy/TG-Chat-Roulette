using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TG.ChatBot.Common.Common.Enums;
using TG.ChatBot.Common.Common.Helpers;
using TG.ChatBot.Common.Domain;
using TG.ChatBot.Common.StepByStep.Enums;
using TG.ChatBot.Common.StepByStep.Interfaces;

namespace TG.ChatBot.Host.Services.StepsByStep.Steps
{
    /// <summary>
    /// Шаг заполнения предпочитаемого типа чата
    /// </summary>
    public class PreferredChatTypeStep : IStep
    {
        private readonly ITelegramBotClient _botClient;
        private readonly RepositoryService _repository;

        public Step Id { get; } = Step.ChatType;

        public PreferredChatTypeStep(ITelegramBotClient botClient, RepositoryService repository)
        {
            _botClient = botClient;
            _repository = repository;
        }

        public async Task Execute(long chatId)
        {
            var keyboard = new InlineKeyboardMarkup(
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("обычный 📨", CommunicationType.Standart.ToString("d")),
                    InlineKeyboardButton.WithCallbackData("голосовые сообщения 🎤", CommunicationType.OnlyVoice.ToString("d"))
                });

            var textMessage = new StringBuilder("Выберите предпочитаемый тип чата\n\n");
            textMessage.Append("1. *обычный* - стандартный привычный чат, с возможностью обмена любыми типами сообщений\n");
            textMessage.Append("2. *голосовые сообщений* - возможность отправлять только голосовые сообщения");
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: textMessage.ToString(),
                parseMode: ParseMode.Markdown, replyMarkup: keyboard);
        }

        public async Task Processing(string data, long userId, Action<long, IStep, Step> action)
        {
            var chatType = int.Parse(data).ToEnum<CommunicationType>();

            await Argument.NotNull(
                value: chatType,
                message: $"_Выбран неверный тип общения: {chatType.GetDescription()}_",
                chatId: userId,
                botClient: _botClient);

            await _repository.Settings.SaveSetting(userId: userId, preferredChatType: (int)chatType);
        }
    }
}
