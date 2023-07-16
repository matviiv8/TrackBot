using TrackBot.Domain.Models;

namespace TrackBot.Domain.Interfaces
{
    public interface ITrackLocationService
    {
        Task<List<Walk>> DivisionIntoWalksAsync(string imei);
        Task<bool> IsValidIMEI(string imei);
        Task<bool> IsTrackLocationExistsWithIMEI(string imei);
    }
}
