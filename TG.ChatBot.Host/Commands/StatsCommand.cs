using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using TG.ChatBot.Common.ChatHub.Models;
using TG.ChatBot.Common.Common.Helpers;
using TG.ChatBot.Common.Domain.Entities;
using TG.ChatBot.Common.Domain.Repository.Interfaces;
using TG.ChatBot.Common.Models.Interfaces;

namespace TG.ChatBot.Host.Commands
{
    public class StatsCommand : ICommandBase
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IChatRoom _chatRoom;

        public string Name => "Статистика пользователя";

        public List<string> Triggers { get; set; }

        public StatsCommand(ITelegramBotClient botClient, IChatRoom chatRoom)
        {
            Triggers = new List<string>
            {
                "/stats"
            };

            _botClient = botClient;
            _chatRoom = chatRoom;
        }

        public async Task Execute(Update update)
        {
            var userId = update.GetSenderId();

            if (userId != null)
            {
                var (countFinds, firstFind, lastFind, totalMessages, avgDuration, totalDuration) = await GetUserStats(userId.Value);
                
                var textMessage = new StringBuilder("📊 Ваша статистика общения 📊\n\n");
                textMessage.Append($"💬 Количество общений - {countFinds}\n");

                if (firstFind != null)
                {
                    textMessage.Append($"🔎 Первое общение - {firstFind.ToString()}\n");
                }

                if (lastFind != null)
                {
                    textMessage.Append($"🔍 Последнее общение: {lastFind.ToString()}\n");
                }

                textMessage.Append($"📝 Всего отправлено сообщений - {totalMessages}\n");
                textMessage.Append($"⏱ Среднее время общения - {avgDuration}\n");
                textMessage.Append($"⏰ Суммарное время общения - {totalDuration}");

                await _botClient.SendTextMessageAsync(chatId: userId, text: textMessage.ToString());
            }
        }

        private async Task<(int, DateTime?, DateTime?, int, string, string)> GetUserStats(long userId)
        {
            string FormateDate(TimeSpan duration)
            {
                var avgHours = duration.Hours > 0 ? $"{duration.Hours} час. " : String.Empty;
                var avgMinutes = duration.Minutes > 0 ? $"{duration.Minutes} мин. " : String.Empty;
                var avgSeconds = $"{duration.Seconds} сек. ";

                return $"{avgHours}{avgMinutes}{avgSeconds}";
            }

            var stats = (await _chatRoom.Get(firstUserId: userId)).OrderBy(x => x.StartDate);

            var countCommunications = stats.Count();

            var firstCommunication = stats.FirstOrDefault()?.StartDate;
            var lastCommunication = stats.LastOrDefault()?.StartDate;

            var totalMessages = stats.Sum(x => x.FirstUserId == userId ? x.NumberMessagesFirstUser : x.NumberMessagesSecondUser);

            var averageDurationSeconds = stats.Average(x => x.EndDate?.Subtract(x.StartDate).TotalSeconds ?? 0);
            var averageDuration = FormateDate(TimeSpan.FromSeconds(averageDurationSeconds));

            var sumDurationSeconds = stats.Sum(x => x.EndDate?.Subtract(x.StartDate).TotalSeconds ?? 0);
            var sumDuration = FormateDate(TimeSpan.FromSeconds(sumDurationSeconds));

            return (countCommunications, firstCommunication, lastCommunication, totalMessages, averageDuration, sumDuration);
        }
    }
}
