using ChatBot.Anonymous.Models.Interfaces;
using ChatBot.Anonymous.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChatBot.Anonymous.Controllers
{
    [ApiController]
    [Route("api/webhook")]
    public class WebhookController : ControllerBase
    {
        private readonly ICommandService _serviceCommand;

        public WebhookController(ICommandService serviceCommand)
        {
            _serviceCommand = serviceCommand;
        }

        /// <summary>
        /// Получает обновления от телеграм бота
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPost("update")]
        public async Task<ActionResult> Update([FromBody] Update update)
        {
            await _serviceCommand.SearchAndExecuteCommand(update);
            return Ok();
        }

        /// <summary>
        /// Временная фича для пробуждения приложения
        /// </summary>
        /// <returns></returns>
        [HttpGet("hc")]
        public string HealthCheck()
        {
            return "Server available";
        }
    }
}
