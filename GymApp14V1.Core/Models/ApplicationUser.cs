using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GymApp14V1.Core.Models
{
#nullable disable
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        public ICollection<ApplicationUserGymClass> AttendedClasses { get; set; }
    }
}
