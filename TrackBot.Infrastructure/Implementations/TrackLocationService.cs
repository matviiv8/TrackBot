using TrackBot.Domain.Interfaces;
using TrackBot.Domain.Models;
using TrackBot.Domain.Repositories;

namespace TrackBot.Infrastructure.Implementations
{
    public class TrackLocationService : ITrackLocationService
    {
        private readonly ITrackLocationRepository _trackLocationRepository;
        private readonly IWalkService _walkService;

        public TrackLocationService(ITrackLocationRepository trackLocationRepository, IWalkService walkService)
        {
            this._trackLocationRepository = trackLocationRepository;
            this._walkService = walkService;
        }

        public async Task<List<Walk>> DivisionIntoWalksAsync(string imei)
        {
            var trackLocations = await _trackLocationRepository.FindAll(trackLocation => trackLocation.Imei.Contains(imei));
            var sortedTrackLocations = trackLocations.OrderBy(trackLocation => trackLocation.DateTrack).ToList();
            var walks = new List<Walk>();
            var walkId = 1;

            for(int i = 0; i < sortedTrackLocations.Count - 1; i++)
            {
                if (sortedTrackLocations[i+1].DateTrack - sortedTrackLocations[i].DateTrack > TimeSpan.FromMinutes(30))
                {
                    var walk = new Walk
                    {
                        Id = walkId++,
                        Start = sortedTrackLocations[i].DateTrack,
                        End = sortedTrackLocations[i+1].DateTrack,
                        Distance = Math.Round(_walkService.GetDistance(sortedTrackLocations[i], sortedTrackLocations[i+1]),1),
                        Time = Math.Round(_walkService.GetTime(sortedTrackLocations[i], sortedTrackLocations[i + 1]).TotalMinutes,1)
                    };

                    walks.Add(walk);
                }
            }

            return walks;
        }

        public async Task<bool> IsValidIMEI(string imei)
        {
            return (imei.Length == 15) && imei.All(symbol => char.IsDigit(symbol));
        }

        public async Task<bool> IsTrackLocationExistsWithIMEI(string imei)
        {
            return await _trackLocationRepository.isExists(trackLocation => trackLocation.Imei.Equals(imei));
        }
    }
}
