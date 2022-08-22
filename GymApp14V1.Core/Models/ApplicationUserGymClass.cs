namespace GymApp14V1.Core.Models
{
#nullable disable
    public class ApplicationUserGymClass
    {
        public GymClass GymClass { get; set; }
        public int GymClassId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
