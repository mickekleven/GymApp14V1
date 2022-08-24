using System.Linq.Expressions;

namespace GymApp14V1.Repository.Interfaces
{
    /// <summary>
    /// Represents generic interface for db factories
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets single post from database asyncronously
        /// </summary>
        /// <param name="id">client</param>
        /// <param name="asNoTracking"></param>
        /// <exception cref="ArgumentNullException">Thrown when userId input arguments is null</exception>
        /// <returns></returns>
        Task<TEntity?> GetAsync(string id, bool asNoTracking = false);



        /// <summary>
        /// Gets the full collection from DB
        /// </summary>
        /// <param name="sortAlt"> Sorting based on columns</param>
        /// <returns></returns>
        IQueryable<TEntity?> GetAll(string sortAlt = "");


        IQueryable<TEntity?> Find(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true);

        /// <summary>
        /// Finds records based on user input
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        IQueryable<TEntity?> Find(string searchString);

        void Update(TEntity entity);
        void Add(TEntity entity);
        void Remove(TEntity entity);
    }

}
