using Microsoft.AspNetCore.Identity;

namespace GymApp14V1.Repository.Interfaces
{
    public interface IRoleRepository : IRepository<IdentityRole>
    {
        IQueryable<IdentityRole> GetAll();

        IQueryable<string> GetRolesByMemberIdAsync(string _memberId);
    }
}