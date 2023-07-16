using Telegram.Bot.Types.ReplyMarkups;

namespace TrackBot.Domain.Interfaces
{
    public interface ITelegramMessageHandler
    {
        Task HandleMessage(string text, long chatId, IReplyMarkup replyMarkup);
    }
}
