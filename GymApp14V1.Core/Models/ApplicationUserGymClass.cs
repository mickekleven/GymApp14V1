namespace GymApp14V1.Core.Models
{

    public class ApplicationUserGymClass
    {
        public GymClass GymClass { get; set; } = new GymClass();
        public int GymClassId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = new ApplicationUser();
        public string ApplicationUserId { get; set; } = string.Empty;
    }
}
