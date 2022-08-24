using GymApp14V1.Core.Models;
using GymApp14V1.Repository.Interfaces;

namespace GarageV3.Data.Repositories.Interfaces
{
    public interface IAppUserGymClassRepository : IRepository<ApplicationUserGymClass>
    {
        Task<ApplicationUserGymClass> GetAsync(string memberId, int gymClassId);
    }
}
