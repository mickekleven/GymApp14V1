using AutoMapper;
using GymApp14V1.Core.ViewModels;
using GymApp14V1.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymApp14V1.Controllers
{
    //[Authorize(Roles = ClientArgs.ADMIN_ROLE)]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;

        private const string viewLocation = "../Roles";

        public RoleController(ApplicationDbContext context, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _mapper = mapper;
            _roleManager = roleManager;
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

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreateAsync(RoleViewModel model)
        {
            var isExist = await _roleManager.FindByNameAsync(model.Name);
            if (isExist is not null) { return BadRequest(); }

            var indentityRole = _mapper.Map<IdentityRole>(model);

            var insertResult = await _roleManager.CreateAsync(indentityRole);

            return RedirectToAction(nameof(Index));
        }


        private async Task<IEnumerable<RoleViewModel>> GetAllAsync() =>
            await _mapper.ProjectTo<RoleViewModel>(_context.Roles).ToListAsync();

        private async Task<IEnumerable<MemberViewModel>> GetAllMemberAsync() =>
            await _mapper.ProjectTo<MemberViewModel>(_context.Users).ToListAsync();


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
