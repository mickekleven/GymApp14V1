namespace GymApp14V1.Models
{
#nullable disable
    public class ApplicationUserGymClass
    {
        public GymClass GymClass { get; set; }
        public int GymClassId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int ApplicationUserId { get; set; }
    }
}
