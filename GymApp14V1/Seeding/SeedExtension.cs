using GymApp14V1.Data;
using Microsoft.EntityFrameworkCore;

namespace GymApp14V1.Seeding
{
    public static class SeedExtension
    {
        public static async Task AddSeedData(this WebApplication appl, int nrOfItems = 10)
        {
            using (var scope = appl.Services.CreateScope())
            {
                try
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    await db.SeedAll(nrOfItems);
                }
                catch (Exception e)
                {
                    throw;
                }
            }

        }


        public static async Task SeedAll(this ApplicationDbContext _db, int nrOfItems)
        {
            //Remove database
            EnsureDeleted(_db);

            //Add Roles

            //Add Memeber

            //Add GymPasses

        }





        private static void EnsureDeleted(ApplicationDbContext _db)
        {
            _db.Database.EnsureDeleted();
            _db.Database.Migrate();
        }
    }
}
