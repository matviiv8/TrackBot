using TrackBot.Domain.Models;

namespace TrackBot.Domain.Interfaces
{
    public interface IWalkService
    {
        decimal GetDistance(TrackLocation firstTrackLocation, TrackLocation secondTrackLocation);
        TimeSpan GetTime(TrackLocation firstTrackLocation, TrackLocation secondTrackLocation);
    }
}