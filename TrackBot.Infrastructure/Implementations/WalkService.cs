using TrackBot.Domain.Models;
using TrackBot.Domain.Interfaces;

namespace TrackBot.Infrastructure.Implementations
{
    public class WalkService : IWalkService
    {
        public decimal GetDistance(TrackLocation firstTrackLocation, TrackLocation secondTrackLocation)
        {
            const decimal radiusEarth = 6371;

            var differenceLatitude = ConvertToRadians(firstTrackLocation.Latitude - secondTrackLocation.Latitude);
            var differenceLongitude = ConvertToRadians(firstTrackLocation.Longitude - secondTrackLocation.Longitude);

            var greatCircleTerm = Math.Sin(differenceLatitude / 2) * Math.Sin(differenceLatitude / 2) + 
                Math.Cos(ConvertToRadians(firstTrackLocation.Latitude)) * Math.Cos(ConvertToRadians(secondTrackLocation.Latitude)) *
                    Math.Sin(differenceLongitude / 2) * Math.Sin(differenceLongitude / 2);

            var greatCircleDistance = 2 * Math.Atan2(Math.Sqrt(greatCircleTerm), Math.Sqrt(1 - greatCircleTerm));

            return radiusEarth * (decimal)greatCircleDistance;
        }

        public TimeSpan GetTime(TrackLocation firstTrackLocation, TrackLocation secondTrackLocation)
        {
            return secondTrackLocation.DateTrack - firstTrackLocation.DateTrack;
        }

        private double ConvertToRadians(decimal angle)
        {
            return (double)angle * Math.PI / 180;
        }
    }
}
