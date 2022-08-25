using GymApp14V1.Core.Models;
using GymApp14V1.Core.ViewModels;

namespace GymApp14V1.Repository.Interfaces
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        IQueryable<MemberViewModel> GetFullCollection(string memberId = "");
    }
}