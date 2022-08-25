using Microsoft.AspNetCore.Identity;

namespace GymApp14V1.Repository.Interfaces
{
    public interface IRoleRepository : IRepository<IdentityRole>
    {
        IQueryable<IdentityRole> GetRoles(string roleId = "");

        IQueryable<string> GetRolesByMemberIdAsync(string _memberId);
    }
}