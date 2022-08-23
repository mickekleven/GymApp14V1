using AutoMapper;
using GymApp14V1.Core.ViewModels;
using GymApp14V1.Data.Data;
using GymApp14V1.Util.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymApp14V1.Controllers
{
    [Authorize(Roles = ClientArgs.ADMIN_ROLE)]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;

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

            ViewBag.PageHeader = GetPageHeader("Member role list", "Admin page");

            return View(getResult);
        }


        [HttpGet, ActionName("Create")]
        public async Task<IActionResult> CreateAsync()
        {



            return View();
        }

        [HttpGet, ActionName("Create")]
        public async Task<IActionResult> CreateAsync(RoleViewModel model)
        {
            return View();
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
