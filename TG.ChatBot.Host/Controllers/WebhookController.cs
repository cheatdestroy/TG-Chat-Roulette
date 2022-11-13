using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using TG.ChatBot.Common.ChatHub.Models;
using TG.ChatBot.Common.Common.Helpers;
using TG.ChatBot.Common.Models.Interfaces;
using TG.ChatBot.Common.StepByStep.Interfaces;

namespace TG.ChatBot.Host.Controllers
{
    [ApiController]
    [Route("api/webhook")]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;
        private readonly ICommandService _commandService;
        private readonly IActionService _actionService;
        private readonly IChatHub _chatHub;

        public WebhookController(ILogger<WebhookController> logger, ICommandService commandService, IActionService actionService, IChatHub chatHub)
        {
            _logger = logger;
            _commandService = commandService;
            _actionService = actionService;
            _chatHub = chatHub;
        }

        /// <summary>
        /// Получает обновления от телеграм бота
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPost("update")]
        public async Task<ActionResult> Update([FromBody] Update update)
        {
            _logger.LogTrace($"Update successfully received ({update.GetSenderId()} | {update.Message?.Text ?? update.CallbackQuery?.Data})");

            if (!update.IsSenderValid())
            {
                return Ok();
            }

            var isCommandFound = await _commandService.ExecuteCommand(update: update);

            if (isCommandFound)
            {
                return Ok();
            }

            var userId = update.GetSenderId();

            if (userId != null)
            {
                var message = update.Message?.Text;

                if (_chatHub.IsUserInChatRoom(userId.Value) && !string.IsNullOrEmpty(message))
                {
                    await _chatHub.RedirectMessage(message: message, senderId: userId.Value);
                }
                else
                {
                    await _actionService.ExecuteAction(update: update);
                }
            }

            return Ok();
        }

        /// <summary>
        /// Временная фича для пробуждения приложения
        /// </summary>
        /// <returns></returns>
        [HttpGet("hc")]
        public string HealthCheck()
        {
            _logger.LogTrace("Server available");
            return "Server available";
        }
    }
}
