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


        public async override Task<GymClass?> GetAsync(string id)
        {
            try
            {
                return await AppDbContext.GymPasses
                    .FirstOrDefaultAsync(a => a.Id == int.Parse(id));

            }
            catch (Exception e)
            {

                throw;
            }
        }

        public virtual IQueryable<GymClass?> Find(Expression<Func<GymClass, bool>> predicate) =>
                AppDbContext!.GymPasses
                .Where(predicate);

        public override IQueryable<GymClass> GetAll(string sortAlt = "")
        {
            return AppDbContext!.GymPasses;

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
