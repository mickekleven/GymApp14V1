using GymApp14V1.Data;
using Microsoft.EntityFrameworkCore;

namespace GymApp14V1.Seeding
{
    public static class SeedExtension
    {

        private static int numberOfSeedItems;
        private static IEnumerable<KeyValuePair<string, string>> firstNames;
        private static IEnumerable<KeyValuePair<string, string>> lastNames;
        private static IEnumerable<KeyValuePair<string, string>> gymPasses;
        private static IEnumerable<KeyValuePair<string, string>> emailProviders;

        public static async Task AddGymData(this WebApplication appl, IConfigurationSection confSection)
        {
            numberOfSeedItems = confSection.GetValue<int>("NumberOfSeedItems");
            firstNames = confSection.GetSection("FirstNames").AsEnumerable();
            lastNames = confSection.GetSection("LastNames").AsEnumerable();
            gymPasses = confSection.GetSection("GymPasses").AsEnumerable();
            emailProviders = confSection.GetSection("EmailProviders").AsEnumerable();


            using (var scope = appl.Services.CreateScope())
            {
                try
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    await db.SeedAsync();
                }
                catch (Exception e)
                {
                    throw;
                }
            }

        }

        public static async Task SeedAsync(this ApplicationDbContext _db)
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
