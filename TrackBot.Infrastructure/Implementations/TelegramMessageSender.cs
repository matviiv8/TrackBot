using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TrackBot.Domain.Interfaces;

namespace TrackBot.Infrastructure.Implementations
{
    public class TelegramMessageSender : ITelegramMessageSender
    {
        private readonly ITelegramBotClient _telegramBotClient;

        public TelegramMessageSender(ITelegramBotClient telegramBotClient)
        {
            this._telegramBotClient = telegramBotClient;
        }

        public async Task SendMessage(long chatId, string message, IReplyMarkup replyMarkup)
        {
            await _telegramBotClient.SendTextMessageAsync(chatId, message, replyMarkup: replyMarkup);
        }

        public async Task SendErrorMessage(long chatId, string errorMessage, IReplyMarkup replyMarkup)
        {
            await _telegramBotClient.SendTextMessageAsync(chatId, errorMessage, replyMarkup: replyMarkup);
        }
    }
}
