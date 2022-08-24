using GymApp14V1.Core.Models;
using System.Linq.Expressions;

namespace GymApp14V1.Repository.Interfaces
{
    public interface IGymClassRepository : IRepository<GymClass>
    {
        IQueryable<GymClass> GetAll(bool ignoreQueryFilter = false);
        IQueryable<GymClass> Find(Expression<Func<GymClass, bool>> predicate, bool ignoreQueryFilter = false);
        Task<GymClass> GetAsync(string id, bool ignoreQueryFilter = false);
    }
}
