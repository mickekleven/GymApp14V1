using GarageV3.Data.Repositories.Interfaces;
using GymApp14V1.Core.Models;
using GymApp14V1.Core.ViewModels;
using GymApp14V1.Data.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymApp14V1.Repository
{
    public class AppUserGymClassRepository : Repository<ApplicationUserGymClass>, IAppUserGymClassRepository
    {
        public AppUserGymClassRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override void Add(ApplicationUserGymClass entity)
        {
            AppDbContext.Add(entity);
        }

        public override void Update(ApplicationUserGymClass entity)
        {

            AppDbContext.Update(entity);
        }

        public override void Remove(ApplicationUserGymClass entity)
        {
            AppDbContext.Remove(entity);
        }

        public async Task<ApplicationUserGymClass> GetAsync(string memberId, int gymClassId)
        {
            var result = await AppDbContext.ApplicationUsersGymClasses
                .Include(x => x.ApplicationUser)
                .Include(x => x.GymClass)
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.GymClassId == gymClassId && x.ApplicationUserId.ToLower() == memberId.ToLower());

            return result is not null ? result : default!;
        }


        public async override Task<ApplicationUserGymClass?> GetAsync(string id)
        {
            var isId = int.TryParse(id, out int idd);
            try
            {
                if (isId)
                {
                    return await AppDbContext.ApplicationUsersGymClasses
                        .Include(x => x.ApplicationUser)
                        .Include(x => x.GymClass)
                        .AsSplitQuery()
                        .FirstOrDefaultAsync(a => a.GymClassId == int.Parse(id));

                }

                return await AppDbContext!.ApplicationUsersGymClasses
                     .Include(x => x.ApplicationUser)
                     .Include(x => x.GymClass)
                     .AsSplitQuery()
                    .FirstOrDefaultAsync(a => a.ApplicationUserId.ToLower().Contains(id.ToLower()));

            }
            catch (Exception e)
            {

                throw;
            }
        }

        public virtual IQueryable<ApplicationUserGymClass?> Find(Expression<Func<ApplicationUserGymClass, bool>> predicate, bool asNotracking = true) =>
          AppDbContext.ApplicationUsersGymClasses
                .Include(v => v.ApplicationUser)
                .Include(v => v.GymClass)
                .AsSplitQuery()
                .Where(predicate);

        public override IQueryable<ApplicationUserGymClass> GetAll(string sortAlt = "")
        {
            return AppDbContext.ApplicationUsersGymClasses
                .Include(v => v.ApplicationUser)
                .Include(v => v.GymClass)
                .AsSplitQuery();
        }



        public async Task<IEnumerable<GymClassViewModel>> GetAttendingCollectionAsync(string memberEmail, bool isIgnoreQueryFiler = false)
        {
            if (isIgnoreQueryFiler)
            {
                return await AppDbContext.ApplicationUsersGymClasses
                    .Include(a => a.ApplicationUser)
                    .Include(a => a.GymClass)
                    .Where(a =>
                        a.GymClass.StartTime > DateTime.Now).Select(a => new GymClassViewModel
                        {
                            Id = a.GymClassId,
                            IsAttending = a.ApplicationUser.Email.ToLower().Equals(memberEmail.ToLower()),
                            Name = a.GymClass.Name,
                            Description = a.GymClass.Description,
                            Duration = a.GymClass.Duration,
                            StartTime = a.GymClass.StartTime

                        }).ToListAsync();
            }


            return await AppDbContext.ApplicationUsersGymClasses
                .Include(a => a.ApplicationUser)
                .Include(a => a.GymClass)
                .Where(a =>
                    a.GymClass.StartTime > DateTime.Now).Select(a => new GymClassViewModel
                    {
                        Id = a.GymClassId,
                        IsAttending = a.ApplicationUser.Email.ToLower().Equals(memberEmail.ToLower()),
                        Name = a.GymClass.Name,
                        Description = a.GymClass.Description,
                        Duration = a.GymClass.Duration,
                        StartTime = a.GymClass.StartTime

                    }).IgnoreQueryFilters().ToListAsync();
        }

        /// <summary>
        /// Sets the generic context to its type
        /// </summary>
        public ApplicationDbContext AppDbContext
        {
            get { return Context; }
        }

    }
}
