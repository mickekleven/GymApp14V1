using GymApp14V1.Data;
using GymApp14V1.Models;
using Microsoft.EntityFrameworkCore;

namespace GymApp14V1.Seeding
{
    public static class SeedExtension
    {
        private static Random rnd = new();

        private static int numberOfSeedItems;
        private static string seedPwd;
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
            //EnsureDeleted(_db);

            //Add Roles
            var _roles = roles.Where(r => r.Value is not null).ToList();
            //_db.AddRange(roles);

            //Add GymPasses

            //Get Memeber
            var _members = GetMember();


        }


        private static IEnumerable<ApplicationUser> GetMember()
        {
            List<ApplicationUser> _applicationUsers = new();
            ApplicationUser user = new();

            for (int i = 0; i < numberOfSeedItems; i++)
            {

                user.FirstName = firstNames.ElementAt(rnd.Next(0, firstNames.Count())).Value;
                user.LastName = lastNames.ElementAt(rnd.Next(0, lastNames.Count())).Value;
                user.Email = $"{user.FirstName}.{user.LastName}@{emailProviders.ElementAt(rnd.Next(0, emailProviders.Count())).Value}";
                user.PasswordHash = seedPwd;

                //Lägg till GymPasses


                //FirstName
                //LastName
                //Email
                //password
                //ConfirmPassword


                _applicationUsers.Add(user);
            }

            return _applicationUsers;
        }




        private static void EnsureDeleted(ApplicationDbContext _db)
        {
            _db.Database.EnsureDeleted();
            _db.Database.Migrate();
        }


        private static void SetSeedDataParams(IConfigurationSection _confSection)
        {
            numberOfSeedItems = _confSection.GetValue<int>("NumberOfSeedItems");
            seedPwd = _confSection.GetValue<string>("SeedPwd");
            firstNames = _confSection.GetSection("FirstNames").AsEnumerable();
            lastNames = _confSection.GetSection("LastNames").AsEnumerable();
            gymPasses = _confSection.GetSection("GymPasses").AsEnumerable();
            emailProviders = _confSection.GetSection("EmailProviders").AsEnumerable();


        }

        private static void SetSeedRoles(IConfigurationSection _confSection)
        {
            roles = _confSection.AsEnumerable();
        }
    }
}
