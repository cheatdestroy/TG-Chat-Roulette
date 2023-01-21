using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using TG.ChatBot.Common.ChatHub.Enums;

namespace TG.ChatBot.Host.Services.Communication
{
    public class Messaging : IMessaging
    {
        private readonly ITelegramBotClient _botClient;

        public Messaging(ITelegramBotClient botClient)
        {
            _botClient = botClient; 
        }

        public async Task SendMessage(Message message, long userId)
        {
            switch (message.Type)
            {
                case MessageType.Text:
                    await SendTextMessage(message.Text, userId);
                    break;
                case MessageType.Audio:
                    await SendFileMessage(message.Audio, userId);
                    break;
                case MessageType.Video:
                    await SendFileMessage(message.Video, userId);
                    break;
                case MessageType.Sticker:
                    await SendFileMessage(message.Sticker, userId);
                    break;
                case MessageType.Voice:
                    await SendFileMessage(message.Voice, userId);
                    break;
                case MessageType.Photo:
                    await SendFileMessage(message.Photo, userId);
                    break;
                case MessageType.VideoNote:
                    await SendFileMessage(message.VideoNote, userId);
                    break;
            };
        }

        private async Task SendTextMessage(string? message, long userId)
        {
            if (!string.IsNullOrEmpty(message))
            {
                await _botClient.SendTextMessageAsync(userId, message);
            }
        }

        private async Task SendFileMessage<T>(T? message, long userId)
        {
            if (message != null)
            {
                if (message is Audio)
                {
                    await _botClient.SendAudioAsync(userId, new InputOnlineFile((message as Audio).FileId));
                }
                else if (message is Video)
                {
                    await _botClient.SendVideoAsync(userId, new InputOnlineFile((message as Video).FileId));
                }
                else if (message is Sticker)
                {
                    await _botClient.SendStickerAsync(userId, new InputOnlineFile((message as Sticker).FileId));
                }
                else if (message is Voice)
                {
                    await _botClient.SendVoiceAsync(userId, new InputOnlineFile((message as Voice).FileId));
                }
                else if (message is PhotoSize[])
                {
                    await _botClient.SendPhotoAsync(userId, new InputOnlineFile((message as PhotoSize[])[0].FileId));
                }
                else if (message is VideoNote)
                {
                    await _botClient.SendVideoNoteAsync(userId, new InputOnlineFile((message as VideoNote).FileId));
                }
            }
        }
    }
}
