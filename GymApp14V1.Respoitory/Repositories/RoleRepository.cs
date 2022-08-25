﻿using GymApp14V1.Data.Data;
using GymApp14V1.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace GymApp14V1.Repository
{
    public class RoleRepository : Repository<IdentityRole>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override IQueryable<IdentityRole> Find(Expression<Func<IdentityRole, bool>> predicate) =>
          AppDbContext.Roles
                .Where(predicate);

        public override IQueryable<IdentityRole> GetAll(string sortAlt = "") => AppDbContext.Roles;


        public IQueryable<IdentityRole> GetRoles(string roleId = "")
        {
            if (!string.IsNullOrWhiteSpace(roleId))
            {
                return AppDbContext.Roles.Where(i => i.Id.ToLower() == roleId.ToLower());
            }

            return AppDbContext.Roles;
        }

        public IQueryable<string> GetRolesByMemberIdAsync(string _memberId)
        {
            return (from a in AppDbContext.UserRoles
                    join b in AppDbContext.Roles on a.UserId equals b.Id
                    where a.UserId.ToLower() == b.Id.ToLower()
                    select b.Name);
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
