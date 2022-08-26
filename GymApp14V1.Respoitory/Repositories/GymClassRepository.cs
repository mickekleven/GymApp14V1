using GymApp14V1.Core.Models;
using GymApp14V1.Data.Data;
using GymApp14V1.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymApp14V1.Repository
{
    public class GymClassRepository : Repository<GymClass>, IGymClassRepository
    {
        public GymClassRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override void Add(GymClass entity)
        {

            AppDbContext.Add(entity);
        }

        public override void Update(GymClass entity)
        {
            AppDbContext.Update(entity);
        }

        public override void Remove(GymClass entity)
        {
            AppDbContext.Remove(entity);
        }


        public async Task<GymClass?> GetAsync(string id, bool ignoreQueryFilter = false)
        {
            try
            {
                if (ignoreQueryFilter)
                {
                    return await AppDbContext.GymPasses.IgnoreQueryFilters()
                        .FirstOrDefaultAsync(g => g.Id == int.Parse(id));
                }

                return await AppDbContext.GymPasses
                        .FirstOrDefaultAsync(g => g.Id == int.Parse(id));
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public IQueryable<GymClass> Find(Expression<Func<GymClass, bool>> predicate, bool ignoreQueryFilter = false)
        {

            if (ignoreQueryFilter)
            {
                return AppDbContext!.GymPasses
                    .Where(predicate)
                    .IgnoreQueryFilters();
            }

            return AppDbContext!.GymPasses
                .Where(predicate)
                .IgnoreQueryFilters();
        }



        public IQueryable<GymClass> GetAll(bool ignoreQueryFilter = false)
        {

            if (ignoreQueryFilter)
            {
                return AppDbContext!.GymPasses
                    .Include(a => a.AttendingMembers)
                    .IgnoreQueryFilters();
            }

            return AppDbContext!.GymPasses
                .Include(a => a.AttendingMembers);
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
