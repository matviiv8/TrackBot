using Telegram.Bot.Types.ReplyMarkups;

namespace TrackBot.Domain.Interfaces
{
    public interface ITelegramMessageSender
    {
        Task SendMessage(long chatId, string message, IReplyMarkup replyMarkup);
        Task SendErrorMessage(long chatId, string errorMessage, IReplyMarkup replyMarkup);
    }
}
