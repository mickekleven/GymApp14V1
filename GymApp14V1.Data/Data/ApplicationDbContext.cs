using GymApp14V1.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymApp14V1.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GymClass> GymPasses => Set<GymClass>();
        public DbSet<ApplicationUser> GymMembers => Set<ApplicationUser>();

        public DbSet<ApplicationUserGymClass> ApplicationUsersGymClasses => Set<ApplicationUserGymClass>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var dataTimeNow = DateTime.Now;

            builder.Entity<ApplicationUserGymClass>()
                .HasKey(aug => new { aug.GymClassId, aug.ApplicationUserId });

            builder.Entity<GymClass>().HasQueryFilter(d => d.StartTime > DateTime.Now);
            builder.Entity<ApplicationUserGymClass>().HasQueryFilter(g => g.GymClass.StartTime > DateTime.Now);
        }
    }
}