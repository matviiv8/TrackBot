using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TrackBot.Domain.Interfaces;

namespace TrackBot.Controllers
{
    [ApiController]
    [Route("bot")]
    public class TrackBotController : Controller
    {
        private readonly ITelegramMessageHandler _telegramMessageHandler;
        private readonly ILogger<TrackBotController> _logger;

        public TrackBotController(ITelegramMessageHandler telegramMessageHandler, ILogger<TrackBotController> logger)
        {
            this._telegramMessageHandler = telegramMessageHandler;
            this._logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> HandleUpdate([FromBody] Update update)
        {
            if (update.Message != null && update.Message.Type == MessageType.Text)
            {
                var message = update.Message;
                var chatId = message.Chat.Id;
                var hideKeyboard = new ReplyKeyboardRemove();

                if (!string.IsNullOrEmpty(message.Text))
                {
                    await _telegramMessageHandler.HandleMessage(message.Text.ToLower(), chatId, hideKeyboard);
                }
            }

            return Ok();
        }
    }
}
