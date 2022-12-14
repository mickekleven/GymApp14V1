using GymApp14V1.Core.Models;
using GymApp14V1.Data.Data;
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

                    var pwdHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<ApplicationUser>>();

                    //await db.SeedAsync();
                }
                catch (Exception e)
                {
                    throw;
                }
            }

        }


        public static async Task AddGymData(this WebApplication appl, ConfigurationManager confManager)
        {

            seedPwd = confManager.GetValue<string>("UserAccountPwd");
            ArgumentNullException.ThrowIfNull(nameof(seedPwd), "UserSecret must have UserAccountPwd");

            SetSeedDataParams(confManager.GetSection("SeedDataParams"));

            SetSeedRoles(confManager.GetSection("Roles"));



            using (var scope = appl.Services.CreateScope())
            {
                try
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var pwdHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<ApplicationUser>>();

                    await db.SeedAsync(pwdHasher);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }


        public static async Task SeedAsync(this ApplicationDbContext _db, IPasswordHasher<ApplicationUser> _pwdHasher)
        {

            if (_db.GymMembers.Any()) { return; }


            //Remove database
            EnsureDeleted(_db);

            //Add Roles
            var _roles = GetRoles();
            _db.Roles.AddRange(_roles);

            //Get Member
            var _members = GetMember(_pwdHasher);
            _db.Users.AddRange(_members);

            //Add GymPasses

            var gymClasses = GetGymClasses(_members);
            _db.AddRange(gymClasses);

            //References User with indentityRole
            var identUserRoles = ReferenceUsersAndRoles(_members, _roles);
            _db.AddRange(identUserRoles);

            var userAndGymClasses = ReferenceUserAndGymClass(_members, gymClasses);
            _db.AddRange(userAndGymClasses);

            await _db.SaveChangesAsync();
        }

        private static IEnumerable<ApplicationUserGymClass> ReferenceUserAndGymClass(IEnumerable<ApplicationUser> members, IEnumerable<GymClass> gymClasses)
        {
            ApplicationUserGymClass appUsrGymClass;
            List<ApplicationUserGymClass> appUsrGymClasses = new();

            foreach (var memb in members)
            {
                appUsrGymClass = new ApplicationUserGymClass
                {
                    ApplicationUser = memb,
                    GymClass = gymClasses.ElementAt(rnd.Next(gymClasses.Count()))
                };

                appUsrGymClasses.Add(appUsrGymClass);
            }

            return appUsrGymClasses.ToList();
        }

        private static IEnumerable<IdentityUserRole<string>> ReferenceUsersAndRoles(IEnumerable<ApplicationUser> users, IEnumerable<IdentityRole> _roles)
        {

            IdentityUserRole<string> userRole;
            List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>();


            foreach (var usr in users)
            {
                userRole = new IdentityUserRole<string>
                {
                    RoleId = _roles.ElementAt(rnd.Next(_roles.Count())).Id.ToString(),
                    UserId = usr.Id.ToString()
                };

                userRoles.Add(userRole);
            }

            return userRoles.ToList();
        }

        private static IEnumerable<IdentityRole> GetRoles()
        {
            var _roles = roles.Where(i => i.Value is not null)
                              .Select(l => new IdentityRole
                              {
                                  Name = l.Value,
                                  NormalizedName = l.Value.ToUpper(),
                                  ConcurrencyStamp = "0"
                              })
                              .ToList();
            return _roles;
        }

        private static IEnumerable<ApplicationUser> GetMember(IPasswordHasher<ApplicationUser> pwdHasher)
        {
            List<ApplicationUser> _applicationUsers = new();


            for (int i = 0; i < numberOfSeedItems; i++)
            {
                ApplicationUser user = new();

                user.FirstName = firstNames.ElementAt(rnd.Next(0, firstNames.Count())).Value;
                user.LastName = lastNames.ElementAt(rnd.Next(0, lastNames.Count())).Value;
                user.Email = ConvertTo($"{user.FirstName.ToLower()}.{user.LastName.ToLower()}@{emailProviders.ElementAt(rnd.Next(0, emailProviders.Count())).Value}");
                user.UserName = user.Email;
                user.NormalizedEmail = ConvertTo(user.Email.ToUpper());
                user.NormalizedUserName = user.UserName.ToUpper();
                user.PasswordHash = pwdHasher.HashPassword(user, seedPwd);

                _applicationUsers.Add(user);


            }

            return _applicationUsers.DistinctBy(n => n.UserName);
        }

        private static string ConvertTo(string inpArg)
        {
            return inpArg
               .Replace("Å", "A").Replace("å", "a")
               .Replace("Ä", "A").Replace("ä", "a")
               .Replace("Ö", "O").Replace("ö", "o");
        }


        private static IEnumerable<GymClass> GetGymClasses(IEnumerable<ApplicationUser> _users)
        {

            var _gymClasses = gymClasses.Where(r => r.Value is not null)
                .Select(l => new GymClass
                {
                    Description = l.Value,
                    Duration = TimeSpan.FromHours(rnd.Next(1, 5)),
                    Name = l.Value,
                    StartTime = DateTime.Now.AddDays(rnd.Next(-5, 10))
                });

            return _gymClasses.ToList();
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
            emailProviders = _confSection.GetSection("EmailProviders").AsEnumerable();
            gymClasses = _confSection.GetSection("GymClasses").AsEnumerable();

            firstNames = firstNames.Where(r => r.Value is not null).ToList();
            lastNames = lastNames.Where(r => r.Value is not null).ToList();
            emailProviders = emailProviders.Where(r => r.Value is not null).ToList();
            gymClasses = gymClasses.Where(r => r.Value is not null).ToList();
        }

        private static void SetSeedRoles(IConfigurationSection _confSection)
        {
            roles = _confSection.AsEnumerable();
        }
    }
}
