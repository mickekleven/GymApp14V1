using GymApp14V1.Core.Models;
using GymApp14V1.Core.ViewModels;
using GymApp14V1.Data.Data;
using GymApp14V1.Repository.Interfaces;
using System.Linq.Expressions;

namespace GymApp14V1.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override IQueryable<ApplicationUser> Find(Expression<Func<ApplicationUser, bool>> predicate) =>
          AppDbContext.Users
                .Where(predicate);

        public override IQueryable<ApplicationUser> GetAll(string sortAlt = "")
        {
            return AppDbContext.Users;
        }

        public IQueryable<MemberViewModel> GetFullCollection(string memberId = "")
        {
            if (string.IsNullOrWhiteSpace(memberId))
            {
                return (from a in AppDbContext.Users
                        join b in AppDbContext.UserRoles on a.Id equals b.UserId
                        join c in AppDbContext.Roles on b.RoleId equals c.Id
                        where c.Id == memberId
                        select new MemberViewModel
                        {
                            Id = a.Id,
                            FirstName = a.FirstName,
                            LastName = a.LastName,
                            Email = a.Email,
                            UserName = a.UserName,
                            Role = c.Name,
                        }).OrderBy(f => f.FirstName);
            }


            return (from a in AppDbContext.Users
                    join b in AppDbContext.UserRoles on a.Id equals b.UserId
                    join c in AppDbContext.Roles on b.RoleId equals c.Id
                    select new MemberViewModel
                    {
                        Id = a.Id,
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                        Email = a.Email,
                        UserName = a.UserName,
                        Role = c.Name,
                    }).OrderBy(f => f.FirstName);
        }


        /// <summary>
        /// Sets the generic context to its type
        /// </summary>
        public ApplicationDbContext AppDbContext
        {
            get { return Context; }
        }
    }
}
