using AutoMapper;
using GymApp14V1.Core.Models;
using GymApp14V1.Core.ViewModels;
using GymApp14V1.Repository.Interfaces;
using GymApp14V1.Util.Helpers;
using GymApp14V1.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymApp14V1.Controllers
{
    [Authorize(Roles = ClientArgs.ADMIN_ROLE)]
    public class RoleController : Controller
    {
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IUnitOfWork _unitOfWork;
        private const string viewLocation = "../Roles";

        public RoleController(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {

            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }


        [HttpGet, ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            var getResult = await GetAllAsync();



            ViewBag.PageHeader = GetPageHeader("Member role list", "Admin page CRUD");

            return View($"{viewLocation}/Index", getResult);
        }


        [HttpGet, ActionName("Create")]
        public IActionResult Create()
        {
            var model = new RoleViewModel
            {
                PageHeader = GetPageHeader("Member role list", "Admin page CRUD")
            };

            return View($"{viewLocation}/Create", model);
        }

        [ModelStateValidation]
        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreateAsync(RoleViewModel model)
        {
            var isExist = await _roleManager.FindByNameAsync(model.Name);
            if (isExist is not null) { return BadRequest(); }

            var indentityRole = _mapper.Map<IdentityRole>(model);

            indentityRole.Id = Guid.NewGuid().ToString();

            var insertResult = await _roleManager.CreateAsync(indentityRole);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet, ActionName("Edit")]
        public async Task<IActionResult> EditAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) { return BadRequest(); }

            var _role = await _roleManager.FindByIdAsync(id);
            if (_role is null) { return NotFound(); }

            var role = _mapper.Map<RoleViewModel>(_role);


            role.PageHeader = GetPageHeader("Member role list", "Admin page CRUD");

            return View($"{viewLocation}/Edit", role);
        }

        [ModelStateValidation]
        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditAsync(RoleViewModel model)
        {

            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role is null) { return NotFound(); }

            role.Name = model.Name;

            var updateResult = await _roleManager.UpdateAsync(role);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet, ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) { return BadRequest(); }

            var _role = await _roleManager.FindByIdAsync(id);
            if (_role is null) { return NotFound(); }

            return View($"{viewLocation}/Delete", _mapper.Map<RoleViewModel>(_role));

        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync(RoleViewModel model)
        {
            var _role = await _roleManager.FindByIdAsync(model.Id);
            if (_role is null) { return NotFound(); }


            var deleteResult = await _roleManager.DeleteAsync(_role);

            return RedirectToAction(nameof(Index));

        }

        // ****************************************************************
        // User and Roles
        // ****************************************************************

        [HttpGet, ActionName("MemberRoleIndex")]
        public async Task<IActionResult> MemberRoleIndexAsync()
        {
            var MemberList = _unitOfWork.ApplicationUserRepo.GetAll();

            var model = new MemberRoleViewModel
            {
                Members = await _mapper.ProjectTo<MemberViewModel>(MemberList).ToListAsync(),
                PageHeader = GetPageHeader("Member and role crud", "Admin page CRUD")
            };

            return View("../MembersAndRoles/MemberRoleIndex", model);
        }



        //Update
        [HttpGet, ActionName("MemberRoleEdit")]
        public async Task<IActionResult> MemberRoleEditAsync(string id)
        {
            //Todo: Continue here with implementation

            var member = await _userManager.FindByIdAsync(id);
            if (member is null) { return NotFound(); }

            var roles = await _userManager.GetRolesAsync(member);

            var aspNetRoles = await _unitOfWork.RoleRepo.GetAll().ToListAsync();
            var _aspNetRoles = aspNetRoles.Select(l => l.Name).ToList();

            var NotSelected = _aspNetRoles.Union(roles).Distinct().ToList();


            //Todo remove User Role from collection

            var aspRoles = await _unitOfWork.RoleRepo.GetAll().ToListAsync();

            var model = new MemberRoleViewModel
            {
                Member = _mapper.Map<MemberViewModel>(member),
                MemberRoles = roles,
                NotSelectedRoles = NotSelected,

                PageHeader = GetPageHeader("Add Role", "Admin page CRUD")
            };

            return View("../MembersAndRoles/MemberRoleEdit", model);
        }

        [HttpPost, ActionName("MemberRoleEdit")]
        public async Task<IActionResult> MemberRoleEditAsync(MemberRoleViewModel model)
        {
            throw new NotImplementedException();
        }


        [HttpGet, ActionName("AddRoleToMember")]
        public async Task<IActionResult> AddRoleToMemberAsync(string memberId)
        {
            var member = await _userManager.FindByIdAsync(memberId);
            if (member is null) { return NotFound(); }

            // Roles
            var roles = await _unitOfWork.RoleRepo.GetRoles().ToListAsync();
            var _roles = roles.Select(l => l.Name);

            // UserRoles
            var userRoles = await _userManager.GetRolesAsync(member);
            if (roles is null) { return NotFound(); }

            var union = _roles.Union(userRoles).Distinct();


            return View(union);
        }


        private async Task<IEnumerable<RoleViewModel>> GetAllAsync() =>
            await _mapper.ProjectTo<RoleViewModel>(_unitOfWork.RoleRepo.GetAll()).ToListAsync();

        private async Task<IEnumerable<MemberViewModel>> GetAllMemberAsync() =>
            await _mapper.ProjectTo<MemberViewModel>(_unitOfWork.ApplicationUserRepo.GetAll()).ToListAsync();


        private PageHeaderViewModel GetPageHeader(string headLine, string SubTitle, string content = "")
        {
            content = string.IsNullOrWhiteSpace(content)
                ? "Start live helthier today already"
                : content;

            return new PageHeaderViewModel
            {
                HeadLine = headLine,
                SubTitle = SubTitle,
                Content = content
            };
        }

    }
}
