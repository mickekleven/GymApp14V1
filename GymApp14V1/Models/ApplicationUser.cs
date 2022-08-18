using Microsoft.AspNetCore.Identity;

namespace GymApp14V1.Models
{
#nullable disable
    public class ApplicationUser : IdentityUser
    {
        public int MemberId { get; set; }

        public ICollection<ApplicationUserGymClass> GymPasses { get; set; }
    }
}
