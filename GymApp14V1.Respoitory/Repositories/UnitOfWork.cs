using GarageV3.Data.Repositories.Interfaces;
using GymApp14V1.Data.Data;
using GymApp14V1.Repository.Interfaces;

namespace GymApp14V1.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IAppUserGymClassRepository AppUserGymClassRepo { get; private set; }
        public IGymClassRepository GymClassRepo { get; private set; }
        public IApplicationUserRepository ApplicationUserRepo { get; private set; }

        public IRoleRepository RoleRepo { get; private set; }


        public UnitOfWork(ApplicationDbContext _context)
        {
            this._context = _context;

            AppUserGymClassRepo = new AppUserGymClassRepository(_context);

            ApplicationUserRepo = new ApplicationUserRepository(_context);

            GymClassRepo = new GymClassRepository(_context);

            RoleRepo = new RoleRepository(_context);

        }

        public async Task<int> CompleteAsync(bool stopTracker = false)
        {
            try
            {
                var save = await _context.SaveChangesAsync();
                if (stopTracker) { _context.ChangeTracker.Clear(); }
                return save;
            }
            catch (Exception e)
            {
                return -99;
                throw;
            }

        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
