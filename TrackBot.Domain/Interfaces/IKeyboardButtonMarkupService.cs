using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TrackBot.Domain.Models.Enums;

namespace TrackBot.Domain.Interfaces
{
    public interface IKeyboardButtonMarkupService
    {
        ReplyKeyboardMarkup CreateTop10WalksKeyboard();
        ReplyKeyboardMarkup CreateLastWalkKeyboard();
        ReplyKeyboardMarkup CreateTotalWalksAndBackKeyboard();
        ReplyKeyboardMarkup CreateLanguageAndBackKeyboard(Language currentLanguageCode);
    }
}
