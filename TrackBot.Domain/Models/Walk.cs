namespace TrackBot.Domain.Models
{
    public class Walk
    {
        public int Id { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public decimal Distance { get; set; }

        public double Time { get; set; }
    }
}
