using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TrackBot.Domain.Models;
using TrackBot.Infrastructure.Context;
using TrackBot.Infrastructure.Repositories;

namespace TrackBot.Domain.Repositories.Implementations
{
    public class UserLanguagePreferenceRepository : BaseRepository<UserLanguagePreference>, IUserLanguagePreferenceRepository
    {
        private TrackContext _trackContext;

        public UserLanguagePreferenceRepository(TrackContext trackContext) : base(trackContext)
        {
            this._trackContext = trackContext;
        }

        public async Task<UserLanguagePreference> GetByUserId(long userId)
        {
            return await _trackContext.UserLanguagePreferences.FirstOrDefaultAsync(language => language.UserId == userId);
        }

        public async Task Create(UserLanguagePreference userLanguagePreference)
        {
            await _trackContext.UserLanguagePreferences.AddAsync(userLanguagePreference);
        }

        public async Task Update(UserLanguagePreference userLanguagePreference)
        {
            _trackContext.UserLanguagePreferences.Update(userLanguagePreference);
            await Save();
        }

        public async Task Save()
        {
            await _trackContext.SaveChangesAsync();
        }
    }
}
