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
        private static IEnumerable<KeyValuePair<string, string>> roles;

        public static async Task AddGymData(this WebApplication appl, IConfigurationSection confSection)
        {

            SetSeedDataParams(confSection);

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


        public static async Task AddGymData(this WebApplication appl, ConfigurationManager confManager)
        {
            SetSeedDataParams(confManager.GetSection("SeedDataParams"));

            SetSeedRoles(confManager.GetSection("Roles"));

            var test = roles.Select(r => r.Value).ToList();

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
            var _roles = GetRoles();


            //Add Memeber

            //Add GymPasses

        }


        private static IEnumerable<string> GetRoles()
        {
            var _roles = roles.Select(x => x.Value).ToList();

            throw new NotImplementedException();
        }


        private static void EnsureDeleted(ApplicationDbContext _db)
        {
            _db.Database.EnsureDeleted();
            _db.Database.Migrate();
        }


        private static void SetSeedDataParams(IConfigurationSection _confSection)
        {
            numberOfSeedItems = _confSection.GetValue<int>("NumberOfSeedItems");
            firstNames = _confSection.GetSection("FirstNames").AsEnumerable();
            lastNames = _confSection.GetSection("LastNames").AsEnumerable();
            gymPasses = _confSection.GetSection("GymPasses").AsEnumerable();
            emailProviders = _confSection.GetSection("EmailProviders").AsEnumerable();

        }

        private static void SetSeedRoles(IConfigurationSection _confSection)
        {
            //roles = _confSection.GetValue<string>("")

            throw new NotImplementedException();
        }
    }
}
