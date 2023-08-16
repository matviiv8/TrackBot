using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackBot.Domain.Models.Enums;

namespace TrackBot.Domain.Models
{
    public class UserLanguagePreference
    {
        public int Id { get; set; }

        public long UserId { get; set; }

        public Language LanguageCode { get; set; } = Language.en;
    }
}
