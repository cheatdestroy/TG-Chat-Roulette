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
        private readonly ITelegramBotClient _botClient;
        private readonly ConfigureCommand _configureCommand;

        public WebhookController(ITelegramBotClient botClient, ConfigureCommand configureCommand)
        {
            _botClient = botClient;
            _configureCommand = configureCommand;
        }

        [HttpPost("update")]
        public async Task<ActionResult> Update([FromBody] Update update)
        {
            var message = update.Message;

            if (message != null)
            {
                await _configureCommand.SearchAndExecuteCommand(_botClient, message);
            }

            return Ok();
        }
    }
}
