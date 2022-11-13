namespace TG.ChatBot.Common.ChatHub.Enums
{
    public interface IMessaging
    {
        /// <summary>
        /// Отправляет сообщение собеседнику
        /// </summary>
        /// <param name="message"> Сообщение </param>
        /// <param name="recipient"> Получатель </param>
        /// <returns></returns>
        Task SendMessage(string message, long recipient);
    }
}
