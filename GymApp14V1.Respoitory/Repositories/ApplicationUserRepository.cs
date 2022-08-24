using GymApp14V1.Core.Models;
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


        /// <summary>
        /// Sets the generic context to its type
        /// </summary>
        public ApplicationDbContext AppDbContext
        {
            get { return Context; }
        }
    }
}
