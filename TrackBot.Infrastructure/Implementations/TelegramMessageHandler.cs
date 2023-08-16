using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using System.Globalization;
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
        private readonly IKeyboardButtonMarkupService _keyboardButtonMarkupService;
        private readonly IStringLocalizer<TelegramMessageHandler> _localizer;
        private readonly ILocalizationService _localizationService;

        public TelegramMessageHandler(ITrackLocationService trackLocationService, IMemoryCache cache, ITelegramMessageSender messageSender, 
            IKeyboardButtonMarkupService keyboardButtonMarkupService, IStringLocalizer<TelegramMessageHandler> localizer, ILocalizationService localizationService)
        {
            this._trackLocationService = trackLocationService;
            this._cache = cache;
            this._messageSender = messageSender;
            this._keyboardButtonMarkupService = keyboardButtonMarkupService;
            this._localizer = localizer;
            this._localizationService = localizationService;
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
                    case string when text == _localizer["Top10Walks"].Value.ToLower():
                        await HandleTop10WalksMessage(chatId, replyMarkup);
                        break;
                    case string when text == _localizer["LastWalk"].Value.ToLower():
                        await HandleLastWalkMessage(chatId, replyMarkup);
                        break;
                    case string when text == _localizer["ChangeLanguage"].Value.ToLower():
                        await HandleLanguageChange(chatId, replyMarkup);
                        break;
                    case string when text == _localizer["Back"].Value.ToLower():
                        await HandleBackMessage(chatId, replyMarkup);
                        break;
                    case string when text == _localizer["English"].Value.ToLower() || text == _localizer["Ukrainian"].Value.ToLower():
                        await HandleUserLanguage(chatId, replyMarkup, text);
                        break;
                    default:
                        await HandleIMEIInputMessage(chatId, replyMarkup, text);
                        break;
                }
            }
            catch (Exception error)
            {
                await _messageSender.SendErrorMessage(chatId, $"{_localizer["ErrorMessage"].Value}: {error.Message}", replyMarkup);
            }
        }

        private async Task HandleUserLanguage(long chatId, IReplyMarkup replyMarkup, string text)
        {
            var languageCode = await _localizationService.ConvertFullNameToLanguageCode(text);
            await _localizationService.SetLanguage(chatId, languageCode.ToString());

            var newCulture = new CultureInfo(languageCode.ToString());
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;

            _cache.Set("CurrentLanguage", languageCode.ToString());

            await HandleStartMessage(chatId, replyMarkup);
        }

        private async Task HandleLanguageChange(long chatId, IReplyMarkup replyMarkup)
        {
            var currentLanguageCode = await _localizationService.GetCurrentLanguage(chatId);
            var currentLanguage = await _localizationService.ConvertLanguageCodeToFullName(currentLanguageCode);
            var keyboard = _keyboardButtonMarkupService.CreateLanguageAndBackKeyboard(currentLanguageCode);

            await _messageSender.SendMessage(chatId, $"{_localizer["CurrentLanguage"].Value} - {_localizer[$"{currentLanguage}"].Value}\n\n{_localizer["SelectLanguage"].Value}:", keyboard);
            _cache.Set("PreviousAction", "change language");
        }

        private async Task HandleStartMessage(long chatId, IReplyMarkup replyMarkup)
        {
            await _messageSender.SendMessage(chatId, $"{_localizer["EnterIMEI"].Value}:", replyMarkup);
        }

        private async Task HandleTop10WalksMessage(long chatId, IReplyMarkup replyMarkup)
        {
            var imei = _cache.Get<string>("IMEI");
            var keyboard = _keyboardButtonMarkupService.CreateLastWalkKeyboard();

            if (string.IsNullOrEmpty(imei))
            {
                await _messageSender.SendMessage(chatId, _localizer["IMEINotProvided"].Value, replyMarkup);
            }
            else
            {
                var walks = await _trackLocationService.DivisionIntoWalksAsync(imei);
                var top10walks = walks.OrderByDescending(walk => walk.Distance).Take(10).ToList();
                var top10message = new StringBuilder(_localizer["Top10Walks"].Value + ":\n\n");

                for (int i = 0; i < top10walks.Count; i++)
                {
                    var walk = top10walks[i];
                    top10message.AppendLine($"{i + 1}. {_localizer["Date"]}: {walk.Start.ToShortDateString()}, {_localizer["Time"]}: {walk.Time} {_localizer["min"]}, {_localizer["Distance"]}: {walk.Distance} {_localizer["km"]}");
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

            var keyboard = _keyboardButtonMarkupService.CreateTop10WalksKeyboard();

            if (lastWalk != null)
            {
                var lastWalkMessage = new StringBuilder()
                        .AppendLine($"{_localizer["LastWalk"].Value}:")
                        .AppendLine($"{_localizer["Date"]}: {lastWalk.Start.ToShortDateString()}")
                        .AppendLine($"{_localizer["Time"]}: {lastWalk.Time} {_localizer["min"]}")
                        .AppendLine($"{_localizer["Distance"]}: {lastWalk.Distance} {_localizer["km"]}")
                        .ToString();

                _cache.Set("PreviousAction", "last walk");
                await _messageSender.SendMessage(chatId, lastWalkMessage, keyboard);
            }
            else
            {
                await _messageSender.SendMessage(chatId, _localizer["NoWalksFound"].Value, keyboard);
            }
        }

        private async Task HandleBackMessage(long chatId, IReplyMarkup replyMarkup)
        {
            var previousMessage = _cache.Get<string>("PreviousAction");
            var imei = _cache.Get<string>("IMEI");

            _cache.Set("PreviousAction", "back");

            if (previousMessage.Equals("top 10 walks") || previousMessage.Equals("last walk") || previousMessage.Equals("change language"))
            {
                await HandleIMEIInputMessage(chatId, replyMarkup, imei);
            }
            else
            {
                await _messageSender.SendMessage(chatId, $"{_localizer["EnterIMEI"].Value}:", replyMarkup);
            }
        }

        private async Task HandleIMEIInputMessage(long chatId, IReplyMarkup replyMarkup, string imei)
        {
            _cache.Set("IMEI", imei);

            if (await _trackLocationService.IsValidIMEI(imei))
            {
                if (string.IsNullOrEmpty(imei))
                {
                    await _messageSender.SendMessage(chatId, _localizer["EnterIMEI"].Value, replyMarkup);
                }
                else if (!await _trackLocationService.IsTrackLocationExistsWithIMEI(imei))
                {
                    await _messageSender.SendMessage(chatId, _localizer["IMEINotFound"].Value, replyMarkup);
                }
                else
                {
                    var walks = await _trackLocationService.DivisionIntoWalksAsync(imei);
                    var keyboard = _keyboardButtonMarkupService.CreateTotalWalksAndBackKeyboard();

                    var imeiInputMessage = new StringBuilder()
                        .AppendLine($"{_localizer["TotalWalksMessage"].Value}: {walks.Count()}")
                        .AppendLine($"{_localizer["TotalKilometersMessage"].Value}: {Math.Round(walks.Select(walk => walk.Distance).Sum(), 2)}")
                        .AppendLine($"{_localizer["TotalTimeMessage"].Value}: {Math.Round(walks.Select(walk => walk.Time).Sum(), 2)}")
                        .ToString();

                    await _messageSender.SendMessage(chatId, imeiInputMessage, keyboard);
                    
                }
            }
            else
            {
                await _messageSender.SendMessage(chatId, _localizer["InvalidIMEI"].Value, replyMarkup);
            }
        }

    }
}
