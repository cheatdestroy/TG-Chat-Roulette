using TG.ChatBot.Host.Common.Helpers;
using TG.ChatBot.Host.Models;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace TG.ChatBot.Host.Services
{
    /// <summary>
    /// Конфигурация вебхука
    /// </summary>
    public class ConfigureWebHook : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly BotConfiguration _botConfiguration;

        public ConfigureWebHook(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _botConfiguration = configuration.GetMainConfigurationToObject();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
            var webhookAddress = $"{_botConfiguration.Url}";

            await botClient.SetWebhookAsync(
                url: webhookAddress,
                allowedUpdates: Array.Empty<UpdateType>(),
                dropPendingUpdates: true,
                cancellationToken: cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            /*using var scope = _serviceProvider.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);*/

            return Task.CompletedTask;
        }
    }
}
