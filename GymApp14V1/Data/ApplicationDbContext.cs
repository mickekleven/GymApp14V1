using GymApp14V1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymApp14V1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GymClass> GymPasses => Set<GymClass>();
        public DbSet<ApplicationUser> GymMembers => Set<ApplicationUser>();

        //public DbSet<ApplicationUserGymClass> GymRelation => Set<ApplicationUserGymClass>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<ApplicationUserGymClass>().HasKey(aug => new { aug.GymClassId, aug.ApplicationUserId });

        }


    }
}