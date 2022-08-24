using GymApp14V1.Core.Models;

namespace GymApp14V1.Core.ViewModels
{
    public class MemberViewModel
    {
#nullable disable

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }


        public int GymClassId { get; set; }
        public ICollection<ApplicationUserGymClass> AttendedClasses { get; set; } = new List<ApplicationUserGymClass>();

        public PageHeaderViewModel PageHeader { get; set; }
    }
}
