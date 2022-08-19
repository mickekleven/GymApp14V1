using GymApp14V1.Data;
using GymApp14V1.Models;
using Microsoft.AspNetCore.Identity;
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
        private static IEnumerable<KeyValuePair<string, string>> gymClasses;

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
            EnsureDeleted(_db);

            //Add Roles
            var _roles = GetRoles();
            _db.AddRange(_roles);

            //Get Member
            var _members = GetMember();
            _db.AddRange(_members);

            //Add GymPasses

            var gymClasses = GetGymClasses(_members);
            _db.AddRange(gymClasses);

            await _db.SaveChangesAsync();
        }


        private static IEnumerable<IdentityRole> GetRoles()
        {
            var _roles = roles.Where(i => i.Value is not null)
                              .Select(l => new IdentityRole { Name = l.Value, ConcurrencyStamp = "0" }).ToList();
            return _roles;
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


        private static IEnumerable<GymClass> GetGymClasses(IEnumerable<ApplicationUser> _users)
        {
            var days = rnd.Next(10);

            var _gymClasses = gymClasses.Where(r => r.Value is not null)
                .Select(l => new GymClass { Description = l.Value, Duration = new TimeSpan(3), Name = l.Value, StartTime = DateTime.Now.AddDays(rnd.Next(10)) });

            return _gymClasses;
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
            gymClasses = _confSection.GetSection("GymClasses").AsEnumerable();
        }

        private static void SetSeedRoles(IConfigurationSection _confSection)
        {
            roles = _confSection.AsEnumerable();
        }
    }
}
