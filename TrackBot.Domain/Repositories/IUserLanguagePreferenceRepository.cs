using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackBot.Domain.Models;

namespace TrackBot.Domain.Repositories
{
    public interface IUserLanguagePreferenceRepository : IBaseRepository<UserLanguagePreference>
    {
        Task<UserLanguagePreference> GetByUserId(long userId);
        Task Create(UserLanguagePreference userLanguagePreference);
        Task Update(UserLanguagePreference userLanguagePreference);
        Task Save();
    }
}
