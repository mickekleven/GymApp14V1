using Microsoft.AspNetCore.Identity;

namespace GymApp14V1.Core.Models
{
#nullable disable
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<ApplicationUserGymClass> AttendedClasses { get; set; }
    }
}
