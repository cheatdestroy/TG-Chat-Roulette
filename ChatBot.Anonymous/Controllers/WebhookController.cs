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

        [HttpPost("update")]
        public async Task<ActionResult> Update([FromBody] Update update)
        {
            var message = update.Message;

            if (message != null)
            {
                await _serviceCommand.SearchAndExecuteCommand(message);
            }

            return Ok();
        }
    }
}
