using System.Text;
using Telegram.Bot;
using TG.ChatBot.Common.Common.Pattern;
using TG.ChatBot.Common.Common.Patterns.PatternObserver;

namespace TG.ChatBot.Host.Services.Communication
{
    public class NotifyChatRoomEvents : IHostedService, IObserver
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IServiceProvider _serviceProvider;

        public NotifyChatRoomEvents(ITelegramBotClient botClient, IServiceProvider serviceProvider)
        {
            _botClient = botClient;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ChatHub.GetInstance(_serviceProvider).Watch(this);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            ChatHub.GetInstance(_serviceProvider).StopWatching(this);

            return Task.CompletedTask;
        }

        public async Task Update(ChatRoomMediator chatRoom, long initiatorId, NotifyTypeEnum type)
        {
            async Task SendStartChatMessage(long userId)
            {
                var textMessage = new StringBuilder("Собеседник найден");
                textMessage.Append("\n\nСледующий собеседник 👉🏻 /next\n");
                textMessage.Append("Закончить диалог 👉🏻 /skip");

                await _botClient.SendTextMessageAsync(
                    chatId: userId,
                    text: textMessage.ToString(),
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
            }

            async Task SendEndChatMessage(long userId)
            {
                var textMessage = new StringBuilder(userId == initiatorId
                    ? "Вы завершили общение с собеседником."
                    : "Собеседник завершил общение с вами.");
                textMessage.Append("\n\nНайти нового собеседника 👉🏻 /find\n\n");

                await _botClient.SendTextMessageAsync(
                    chatId: userId,
                    text: textMessage.ToString(),
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
            }


            if (type == NotifyTypeEnum.StartChatRoom)
            {
                await SendStartChatMessage(chatRoom.FirstUser.Info.UserId);
                await SendStartChatMessage(chatRoom.SecondUser.Info.UserId);
            }
            else if (type == NotifyTypeEnum.EndChatRoom)
            {
                await SendEndChatMessage(chatRoom.FirstUser.Info.UserId);
                await SendEndChatMessage(chatRoom.SecondUser.Info.UserId);
            }
        }
    }
}
