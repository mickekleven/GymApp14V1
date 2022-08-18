namespace GymApp14V1.Models
{
#nullable disable
    public class GymPass
    {
        public int GymPassId { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime EndTime => StartTime + Duration;
        public string Description { get; set; }
        public ICollection<ApplicationUserGymClass> ActiveMembers { get; set; }
    }
}
