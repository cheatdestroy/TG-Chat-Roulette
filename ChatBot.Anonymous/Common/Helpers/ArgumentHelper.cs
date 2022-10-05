using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ChatBot.Anonymous.Common.Helpers
{
    /// <summary>
    /// Вспомогательные команды для валидации аргументов
    /// </summary>
    public static class Argument
    {
        /// <summary>
        /// Проверяет значение на null
        /// </summary>
        /// <remarks> При указании параметров message, chatId и botClient отправляется сообщение пользователю в Telegram </remarks>
        /// <param name="value"> Проверяемое значение </param>
        /// <param name="message"> Сообщение при исключении </param>
        /// <param name="chatId"> Уникальный идентификатор чата </param>
        /// <param name="botClient"> Клиент бота для отправки сообщения </param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"> Если значение null, пробрасывается исключение </exception>
        public static async Task NotNull<T>(
            [NotNull] T? value, 
            string message, 
            long chatId,
            ITelegramBotClient botClient,
            [CallerArgumentExpression(parameterName: "value")] string? paramName = null)
        {
            if (value == null)
            {
                if (!string.IsNullOrEmpty(message) && botClient != null)
                {
                    await botClient.SendTextMessageAsync(
                        chatId: chatId, 
                        text: message, 
                        parseMode: ParseMode.Markdown);
                }

                throw new ArgumentNullException(paramName, message);
            }
        }

        /// <summary>
        /// Проверяет значение на null
        /// </summary>
        /// <param name="value"> Проверяемое значение </param>
        /// <param name="message"> Сообщение при исключении </param>
        /// <exception cref="ArgumentNullException"> Если значение null, пробрасывается исключение </exception>
        public static void NotNull<T>(
            [NotNull] T? value,
            string message,
            [CallerArgumentExpression(parameterName: "value")] string? paramName = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName, message);
            }
        }

        public static async Task OutOfRange(
            int value,
            int maxValue,
            int minValue,
            string message,
            long chatId,
            ITelegramBotClient botClient,
            [CallerArgumentExpression(parameterName: "value")] string? paramName = null)
        {
            if (maxValue < value || value < minValue)
            {
                if (!string.IsNullOrEmpty(message) && botClient != null)
                {
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: message,
                        parseMode: ParseMode.Markdown);
                }

                throw new ArgumentNullException(paramName, message);
            }
        }
        
        public static async Task OutOfRange(
            int value,
            int maxValue,
            int minValue,
            string message,
            [CallerArgumentExpression(parameterName: "value")] string? paramName = null)
        {
            if (maxValue < value || value < minValue)
            {
                throw new ArgumentNullException(paramName, message);
            }
        }
    }
}
