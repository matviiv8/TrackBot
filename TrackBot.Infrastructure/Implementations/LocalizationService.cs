using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackBot.Domain.Interfaces;
using TrackBot.Domain.Models;
using TrackBot.Domain.Models.Enums;
using TrackBot.Domain.Repositories;

namespace TrackBot.Infrastructure.Implementations
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IUserLanguagePreferenceRepository _userLanguagePreference;
        private readonly IStringLocalizer<LocalizationService> _localizer;

        public LocalizationService(IUserLanguagePreferenceRepository userLanguagePreference, IStringLocalizer<LocalizationService> localizer)
        {
            this._userLanguagePreference = userLanguagePreference;
            this._localizer = localizer;
        }

        public async Task<Language> GetCurrentLanguage(long userId)
        {
            var userLanguagePreference = await _userLanguagePreference.GetByUserId(userId);

            return userLanguagePreference == null ? Language.en : userLanguagePreference.LanguageCode;
        }

        public async Task SetLanguage(long userId, string languageCode)
        {
            var userLanguagePreference = await _userLanguagePreference.GetByUserId(userId);

            if (Enum.TryParse(languageCode, true, out Language selectedLanguage))
            {
                if (userLanguagePreference == null)
                {
                    userLanguagePreference = new UserLanguagePreference { UserId = userId, LanguageCode = selectedLanguage };
                    await _userLanguagePreference.Create(userLanguagePreference);
                    await _userLanguagePreference.Save();
                }
                else
                {
                    userLanguagePreference.LanguageCode = selectedLanguage;
                    await _userLanguagePreference.Update(userLanguagePreference);
                }
            }
        }

        public async Task<Language> ConvertFullNameToLanguageCode(string language)
        {
            return language.ToLower() == _localizer["English"].Value.ToLower() ? Language.en : Language.uk;
        }

        public async Task<string> ConvertLanguageCodeToFullName(Language languageCode)
        {
            return languageCode == Language.en ? _localizer["English"].Value : _localizer["Ukrainian"].Value;
        }
    }
}
