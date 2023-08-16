using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TrackBot.Domain.Interfaces;
using TrackBot.Domain.Models.Enums;

namespace TrackBot.Infrastructure.Implementations
{
    public class KeyboardButtonMarkupService : IKeyboardButtonMarkupService
    {
        private readonly IStringLocalizer<KeyboardButtonMarkupService> _localizer;

        public KeyboardButtonMarkupService(IStringLocalizer<KeyboardButtonMarkupService> localizer)
        {
            this._localizer = localizer;
        }

        private KeyboardButton CreateButton(string localizerKey)
        {
            return new KeyboardButton(_localizer[localizerKey].Value);
        }

        public ReplyKeyboardMarkup CreateTop10WalksKeyboard()
        {
            var top10Button = CreateButton("Top10Walks");
            var backButton = CreateButton("Back");
            var changeLanguage = CreateButton("ChangeLanguage");

            return new ReplyKeyboardMarkup(new[] { new[] { top10Button, changeLanguage, backButton } });
        }

        public ReplyKeyboardMarkup CreateLastWalkKeyboard()
        {
            var lastWalkButton = CreateButton("LastWalk");
            var backButton = CreateButton("Back");
            var changeLanguage = CreateButton("ChangeLanguage");

            return new ReplyKeyboardMarkup(new[] { new[] { lastWalkButton, changeLanguage, backButton } });
        }

        public ReplyKeyboardMarkup CreateTotalWalksAndBackKeyboard()
        {
            var top10Button = CreateButton("Top10Walks");
            var lastWalkButton = CreateButton("LastWalk");
            var backButton = CreateButton("Back");
            var changeLanguage = CreateButton("ChangeLanguage");

            return new ReplyKeyboardMarkup(new[]
            {
                new[] { top10Button, lastWalkButton, changeLanguage, backButton },
            });
        }

        public ReplyKeyboardMarkup CreateLanguageAndBackKeyboard(Language currentLanguageCode)
        {
            var languageButton = currentLanguageCode == Language.en ? CreateButton("Ukrainian") : CreateButton("English");
            var backButton = CreateButton("Back");

            return new ReplyKeyboardMarkup(new[]
{
                new[] { languageButton, backButton},
            });
        }
    }
}
