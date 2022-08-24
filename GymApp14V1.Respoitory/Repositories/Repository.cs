using GymApp14V1.Data.Data;
using GymApp14V1.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymApp14V1.Repository
{
    /// <summary>
    /// Represents the generic implementation of the repository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : IRepository<TEntity?> where TEntity : class
    {
        protected readonly ApplicationDbContext Context;

        public Repository(ApplicationDbContext context)
        {
            Context = context;
        }

        public virtual IQueryable<TEntity> GetAll(string sortAlt = "")
        {
            return Context.Set<TEntity?>();
        }



        /// <summary>
        /// Fetches the entity based on id from DB
        /// </summary>
        /// <param name="id"></param>
        /// <param name="asNoTracking"></param>
        /// <returns></returns>
        public virtual async Task<TEntity?> GetAsync(string id, bool asNoTracking = false)
        {
            return await Context.Set<TEntity?>().FindAsync(id).ConfigureAwait(false);
        }



        public virtual async Task<IEnumerable<TEntity?>> GetAllReducedAsync(string sortAlt = "")
        {
            return await Context.Set<TEntity?>().ToListAsync().ConfigureAwait(false);
        }


        /// <summary>
        /// Finds entities based on predicate 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity?> Find(Expression<Func<TEntity, bool>> predicate, bool asNotracking = true)
        {
            return Context.Set<TEntity?>().Where(predicate);
        }

        public virtual IQueryable<TEntity?> Find(string searchString)
        {
            return Context.Set<IQueryable<TEntity?>>().Find(searchString);
        }


        /// <summary>
        /// Inserts entity 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual void Add(TEntity entity)
        {
            Context.Set<TEntity?>().AddAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// Removes entity from DB
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual void Remove(TEntity entity)
        {
            Context.Set<TEntity?>().Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            Context.Set<TEntity?>().Update(entity);
        }
    }

}
