using Microsoft.Extensions.Caching.Memory;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using TrackBot.Domain.Interfaces;

namespace TrackBot.Infrastructure.Implementations
{
    public class TelegramMessageHandler : ITelegramMessageHandler
    {
        private readonly ITrackLocationService _trackLocationService;
        private readonly IMemoryCache _cache;
        private readonly ITelegramMessageSender _messageSender;

        public TelegramMessageHandler(ITrackLocationService trackLocationService, IMemoryCache cache, ITelegramMessageSender messageSender)
        {
            this._trackLocationService = trackLocationService;
            this._cache = cache;
            this._messageSender = messageSender;
        }

        public async Task HandleMessage(string text, long chatId, IReplyMarkup replyMarkup)
        {
            try
            {
                switch (text)
                {
                    case "/start":
                        await HandleStartMessage(chatId, replyMarkup);
                        break;
                    case "top 10 walks":
                        await HandleTop10WalksMessage(chatId, replyMarkup);
                        break;
                    case "last walk":
                        await HandleLastWalkMessage(chatId, replyMarkup);
                        break;
                    case "back":
                        await HandleBackMessage(chatId, replyMarkup);
                        break;
                    default:
                        await HandleIMEIInputMessage(chatId, replyMarkup, text);
                        break;
                }
            }
            catch (Exception error)
            {
                await _messageSender.SendErrorMessage(chatId, $"An error occurred: {error.Message}", replyMarkup);
            }
        }

        private async Task HandleStartMessage(long chatId, IReplyMarkup replyMarkup)
        {
            await _messageSender.SendMessage(chatId, "Enter the IMEI number:", replyMarkup);
        }

        private async Task HandleTop10WalksMessage(long chatId, IReplyMarkup replyMarkup)
        {
            var imei = _cache.Get<string>("IMEI");
            var backButton = new KeyboardButton("Back");
            var lastWalkButton = new KeyboardButton("Last walk");

            var keyboard = new ReplyKeyboardMarkup(new[] { new[] { lastWalkButton, backButton } });

            if (string.IsNullOrEmpty(imei))
            {
                await _messageSender.SendMessage(chatId, "IMEI is not provided", replyMarkup);
            }
            else
            {
                var walks = await _trackLocationService.DivisionIntoWalksAsync(imei);
                var top10walks = walks.OrderByDescending(walk => walk.Distance).Take(10).ToList();
                var top10message = new StringBuilder("Top 10 walks:\n\n");

                for (int i = 0; i < top10walks.Count; i++)
                {
                    var walk = top10walks[i];
                    top10message.AppendLine($"{i + 1}. Date: {walk.Start.ToShortDateString()}, Time: {walk.Time} min, Distance: {walk.Distance} km");
                }

                _cache.Set("PreviousAction", "top 10 walks");
                await _messageSender.SendMessage(chatId, top10message.ToString(), keyboard);
            }
        }

        private async Task HandleLastWalkMessage(long chatId, IReplyMarkup replyMarkup)
        {
            var imei = _cache.Get<string>("IMEI");
            var walks = await _trackLocationService.DivisionIntoWalksAsync(imei);
            var lastWalk = walks.OrderByDescending(walk => walk.End).FirstOrDefault();
            var backButton = new KeyboardButton("Back");
            var top10Button = new KeyboardButton("Top 10 walks");

            var keyboard = new ReplyKeyboardMarkup(new[] { new[] { top10Button, backButton } });

            if (lastWalk != null)
            {
                var lastWalkMessage = new StringBuilder()
                        .AppendLine("Last walk:")
                        .AppendLine($"Date: {lastWalk.Start.ToShortDateString()}")
                        .AppendLine($"Time: {lastWalk.Time} min")
                        .AppendLine($"Distance: {lastWalk.Distance} km")
                        .ToString();

                _cache.Set("PreviousAction", "last walk");
                await _messageSender.SendMessage(chatId, lastWalkMessage.ToString(), keyboard);
            }
            else
            {
                await _messageSender.SendMessage(chatId, "No walks found", keyboard);
            }
        }

        private async Task HandleBackMessage(long chatId, IReplyMarkup replyMarkup)
        {
            var previousMessage = _cache.Get<string>("PreviousAction");
            var imei = _cache.Get<string>("IMEI");

            _cache.Set("PreviousAction", "back");

            if (previousMessage.Equals("top 10 walks") || previousMessage.Equals("last walk"))
            {
                await HandleIMEIInputMessage(chatId, replyMarkup, imei);
            }
            else
            {
                await _messageSender.SendMessage(chatId, "Enter IMEI:", replyMarkup);
            }
        }

        private async Task HandleIMEIInputMessage(long chatId, IReplyMarkup replyMarkup, string imei)
        {
            _cache.Set("IMEI", imei);

            if (await _trackLocationService.IsValidIMEI(imei))
            {
                if (string.IsNullOrEmpty(imei))
                {
                    await _messageSender.SendMessage(chatId, "Enter IMEI:", replyMarkup);
                }
                else if (!await _trackLocationService.IsTrackLocationExistsWithIMEI(imei))
                {
                    await _messageSender.SendMessage(chatId, "IMEI not found", replyMarkup);
                }
                else
                {
                    var walks = await _trackLocationService.DivisionIntoWalksAsync(imei);
                    var top10Button = new KeyboardButton("Top 10 walks");
                    var lastWalkButton = new KeyboardButton("Last walk");
                    var backButton = new KeyboardButton("Back");

                    var keyboard = new ReplyKeyboardMarkup(new[] { new[] { top10Button, lastWalkButton, backButton } });

                    await _messageSender.SendMessage(chatId, $"Total walks: {walks.Count()}\nTotal kilometers traveled: {Math.Round(walks.Select(walk => walk.Distance).Sum(), 2)}\nTotal time, minutes: {Math.Round(walks.Select(walk => walk.Time).Sum(), 2)}", keyboard);
                }
            }
            else
            {
                await _messageSender.SendMessage(chatId, "Invalid IMEI, try again", replyMarkup);
            }
        }
    }
}
