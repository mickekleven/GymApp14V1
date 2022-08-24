using GarageV3.Data.Repositories.Interfaces;

namespace GymApp14V1.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAppUserGymClassRepository AppUserGymClassRepo { get; }
        IGymClassRepository GymClassRepo { get; }
        IApplicationUserRepository ApplicationUserRepo { get; }

        Task<int> CompleteAsync(bool stopTracker = false);

    }
}
