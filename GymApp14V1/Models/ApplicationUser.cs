using Microsoft.AspNetCore.Identity;

namespace GymApp14V1.Models
{
#nullable disable
    public class ApplicationUser : IdentityUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<ApplicationUserGymClass> GymPasses { get; set; }
    }
}
