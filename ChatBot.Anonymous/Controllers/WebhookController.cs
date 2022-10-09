using ChatBot.Anonymous.Models.Interfaces;
using ChatBot.Anonymous.Services.StepByStep.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace ChatBot.Anonymous.Controllers
{
    [ApiController]
    [Route("api/webhook")]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;
        private readonly ICommandService _commandService;
        private readonly IActionService _actionService;

        public WebhookController(ILogger<WebhookController> logger, ICommandService commandService, IActionService actionService)
        {
            _logger = logger;
            _commandService = commandService;
            _actionService = actionService;
        }

        /// <summary>
        /// Получает обновления от телеграм бота
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPost("update")]
        public async Task<ActionResult> Update([FromBody] Update update)
        {
            _logger.LogTrace($"Update successfully received ({update.Message?.From?.Id ?? update.CallbackQuery?.Message?.From?.Id} | {update.Message?.Text ?? update.CallbackQuery?.Data})");
            var isDone = await _commandService.ExecuteCommand(update: update);

            if (!isDone)
            {
                await _actionService.ExecuteAction(update: update);
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
