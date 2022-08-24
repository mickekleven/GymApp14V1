using GymApp14V1.Core.Models;

namespace GymApp14V1.Repository.Interfaces
{
    public interface IGymClassRepository : IRepository<GymClass>
    {
        IQueryable<GymClass> GetAll(bool ignoreQueryFilter = false);

        Task<GymClass> GetAsync(string id, bool ignoreQueryFilter = false);
    }
}
