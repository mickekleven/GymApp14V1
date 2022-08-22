using AutoMapper;
using GymApp14V1.Data;
using GymApp14V1.Models;
using GymApp14V1.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymApp14V1.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public MemberController(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }



        [HttpGet, ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            var getResult = await GetFullMemberCollectionAsync();

            var viewModel = new AdminViewModel
            {
                Members = getResult
            };

            return View("../Members/MemberAdmin", viewModel);

        }

        // GET: Member/Details/5

        [HttpGet]
        [ActionName("MemberDetails")]
        public async Task<IActionResult> MemberDetailsAsync(string id)
        {
            if (id == null) { return NotFound(); }

            var memberViewModel = await GetVMAsync(id);
            if (memberViewModel == null) { return NotFound(); }

            return View(memberViewModel);
        }


        [HttpGet, ActionName("MemberEdit")]
        public async Task<IActionResult> MemberEditAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var memberViewModel = await GetVMAsync(id);
            if (memberViewModel == null) return NotFound();

            return View(memberViewModel);
        }

        [HttpPost, ActionName("MemberEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MemberEditAsync(string id, MemberViewModel memberViewModel)
        {
            if (id != memberViewModel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var member = await GetAsync(memberViewModel.Id);
                if (member is null) return NotFound();

                try
                {
                    _context.Update(memberViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExist(memberViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(memberViewModel);
        }


        [HttpGet, ActionName("Delete")]
        public async Task<IActionResult> MemberDeleteAsync(string id)
        {
            if (id == null) return NotFound();


            var memberViewModel = await GetVMAsync(id);
            if (memberViewModel == null) return NotFound();


            return View(memberViewModel);
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MemberDeleteConfirmedAsync(string id)
        {
            var member = await GetAsync(id);
            if (member != null)
            {
                var deleteRoles = await _userManager
                    .RemoveFromRolesAsync(member, new List<string> { "Administrator", "Member", "Visitor" });
                if (!deleteRoles.Succeeded) { return BadRequest(); }




                var deleteRole = _userManager.DeleteAsync(member);

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExist(string id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }



        private async Task<MemberViewModel> GetVMAsync(string _memberId)
        {
            var member = await GetAsync(_memberId);

            var roles = await GetRolesAsync(member);

            return _mapper.Map<MemberViewModel>(member);
        }

        private async Task<ApplicationUser> GetAsync(string _memberIdOrName)
        {

            if (string.IsNullOrWhiteSpace(_memberIdOrName))
            {
                throw new ArgumentNullException(_memberIdOrName, "Argument is null or empty");
            }

            var isGuid = Guid.TryParse(_memberIdOrName, out Guid _guid);

            ApplicationUser _user;

            if (isGuid)
            {
                _user = await _userManager.FindByIdAsync(_memberIdOrName);

            }
            else
            {
                _user = await _userManager.FindByNameAsync(_memberIdOrName);
            }



            return _user;

        }

        private async Task<IEnumerable<MemberViewModel>> GetAllAsync()
        {
            return await _mapper.ProjectTo<MemberViewModel>(_context.Users)
                .OrderBy(a => a.FirstName)
                .ToListAsync();
        }


        private async Task<IList<string>> GetRolesAsync(ApplicationUser _user) =>
                                                    await _userManager.GetRolesAsync(_user);


        private async Task<IEnumerable<string>> GetRolesByMemberIdAsync(string _memberId)
        {
            var roles = await (from a in _context.UserRoles
                               join b in _context.Roles on a.UserId equals b.Id
                               where a.UserId.ToLower() == b.Id.ToLower()
                               select b.Name).ToListAsync();

            return roles;
        }

        /// <summary>
        /// Gets user collection included with role name
        /// </summary>
        /// <param name="_memberId"></param>
        /// <returns></returns>
        private async Task<IEnumerable<MemberViewModel>> GetFullMemberCollectionAsync(string _memberId = "")
        {
            List<MemberViewModel> members = new();

            if (string.IsNullOrWhiteSpace(_memberId))
            {
                return await (from a in _context.Users
                              join b in _context.UserRoles on a.Id equals b.UserId
                              join c in _context.Roles on b.RoleId equals c.Id
                              select new MemberViewModel
                              {
                                  Id = a.Id,
                                  FirstName = a.FirstName,
                                  LastName = a.LastName,
                                  Email = a.Email,
                                  UserName = a.UserName,
                                  Role = c.Name,
                              }).OrderBy(f => f.FirstName).ToListAsync();
            }

            return await (from a in _context.Users
                          join b in _context.UserRoles on a.Id equals b.UserId
                          join c in _context.Roles on b.RoleId equals c.Id
                          where a.Id.ToLower() == _memberId.ToLower()
                          select new MemberViewModel
                          {
                              Id = a.Id,
                              FirstName = a.FirstName,
                              LastName = a.LastName,
                              Email = a.Email,
                              UserName = a.UserName,
                              Role = c.Name,
                          }).OrderBy(f => f.FirstName).ToListAsync();
        }


        private async Task<bool> DeleteMemberAsync(string _memberId)
        {
            var member = await GetAsync(_memberId);
            if (member is null) return false;

            var deleteRole = await DeleteMemberRolesAsync(member);

            var deleteMember = await _userManager.DeleteAsync(member);
            if (!deleteMember.Succeeded) return false;

            return true;
        }

        private async Task<bool> DeleteMemberRolesAsync(ApplicationUser _member)
        {
            var deleteRoles = await _userManager
                .RemoveFromRolesAsync(_member, new List<string> { "Administrator", "Member", "Visitor" });
            if (!deleteRoles.Succeeded) return false;

            return true;
        }


        private async Task<bool> DeleteFromGymClassAsync(string _memberId)
        {
            throw new NotImplementedException();
        }
    }
}
