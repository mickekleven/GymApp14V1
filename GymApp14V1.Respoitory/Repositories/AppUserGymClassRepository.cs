using GarageV3.Data.Repositories.Interfaces;
using GymApp14V1.Core.Models;
using GymApp14V1.Data.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymApp14V1.Repository
{
    public class AppUserGymClassRepository : Repository<ApplicationUserGymClass>, IAppUserGymClassRepository
    {
        public AppUserGymClassRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override void Add(ApplicationUserGymClass entity)
        {
            AppDbContext.Add(entity);
        }

        public override void Update(ApplicationUserGymClass entity)
        {

            AppDbContext.Update(entity);
        }

        public override void Remove(ApplicationUserGymClass entity)
        {
            AppDbContext.Remove(entity);
        }


        public async override Task<ApplicationUserGymClass?> GetAsync(string id)
        {
            var isId = int.TryParse(id, out int idd);
            try
            {
                if (isId)
                {
                    return await AppDbContext.ApplicationUsersGymClasses
                        .Include(x => x.ApplicationUser)
                        .Include(x => x.GymClass)
                        .AsSplitQuery()
                        .FirstOrDefaultAsync(a => a.GymClassId == int.Parse(id));

                }

                return await AppDbContext!.ApplicationUsersGymClasses
                     .Include(x => x.ApplicationUser)
                     .Include(x => x.GymClass)
                     .AsSplitQuery()
                    .FirstOrDefaultAsync(a => a.ApplicationUserId.ToLower().Contains(id.ToLower()));

            }
            catch (Exception e)
            {

                throw;
            }
        }

        public virtual IQueryable<ApplicationUserGymClass?> Find(Expression<Func<ApplicationUserGymClass, bool>> predicate, bool asNotracking = true) =>
          AppDbContext.ApplicationUsersGymClasses
                .Include(v => v.ApplicationUser)
                .Include(v => v.GymClass)
                .AsSplitQuery()
                .Where(predicate);

        public override IQueryable<ApplicationUserGymClass> GetAll(string sortAlt = "")
        {
            return AppDbContext.ApplicationUsersGymClasses
                .Include(v => v.ApplicationUser)
                .Include(v => v.GymClass)
                .AsSplitQuery();
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
