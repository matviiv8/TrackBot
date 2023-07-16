using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TrackBot.Domain.Models;
using TrackBot.Domain.Repositories;
using TrackBot.Infrastructure.Context;

namespace TrackBot.Infrastructure.Repositories
{
    public class TrackLocationRepository : BaseRepository<TrackLocation>, ITrackLocationRepository
    {
        public TrackLocationRepository(TrackContext trackContext) : base(trackContext) { }
    }
}
