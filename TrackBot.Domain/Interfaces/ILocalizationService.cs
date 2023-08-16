using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackBot.Domain.Models.Enums;

namespace TrackBot.Domain.Interfaces
{
    public interface ILocalizationService
    {
        Task SetLanguage(long userId, string languageCode);
        Task<Language> GetCurrentLanguage(long userId);
        Task<Language> ConvertFullNameToLanguageCode(string language);
        Task<string> ConvertLanguageCodeToFullName(Language languageCode);
    }
}
