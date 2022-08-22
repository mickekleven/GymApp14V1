using AutoMapper;
using GymApp14V1.Core.Models;
using GymApp14V1.Core.ViewModels;
using GymApp14V1.Data.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymApp14V1.Controllers
{
    [Authorize]
    public class GymClassController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;


        public GymClassController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }



        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {


            return _context.GymPasses != null ?
                        View(await GetGymClassesAsync()) :
                        Problem("Entity set 'ApplicationDbContext.GymPasses'  is null.");
        }

        // GET: GymPass/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GymPasses == null)
            {
                return NotFound();
            }

            var gymPass = await _context.GymPasses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymPass == null)
            {
                return NotFound();
            }

            return View(gymPass);
        }

        // GET: GymPass/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GymPass/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GymPassId,Name,StartTime,Duration,Description")] GymClass gymPass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gymPass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gymPass);
        }


        [Authorize(Roles = "Administrator, Member")]
        [HttpGet, ActionName("BookingXX")]
        public async Task<IActionResult> BookingAsync(string gymClassId)
        {
            if (string.IsNullOrWhiteSpace(gymClassId)) { return NotFound(); }
            var getResult = await GetGymClassVMAsync(gymClassId);
            return View(getResult);
        }


        // GET: GymPass/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GymPasses == null)
            {
                return NotFound();
            }

            var gymPass = await _context.GymPasses.FindAsync(id);
            if (gymPass == null)
            {
                return NotFound();
            }
            return View(gymPass);
        }

        // POST: GymPass/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GymPassId,Name,StartTime,Duration,Description")] GymClass gymPass)
        {
            if (id != gymPass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gymPass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymPassExists(gymPass.Id))
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
            return View(gymPass);
        }

        // GET: GymPass/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GymPasses == null)
            {
                return NotFound();
            }

            var gymPass = await _context.GymPasses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymPass == null)
            {
                return NotFound();
            }

            return View(gymPass);
        }

        // POST: GymPass/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GymPasses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GymPasses'  is null.");
            }
            var gymPass = await _context.GymPasses.FindAsync(id);
            if (gymPass != null)
            {
                _context.GymPasses.Remove(gymPass);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet, ActionName("Booking")]
        public async Task<IActionResult> BookingToggleAsync()
        {
            var member = await GetMemberVMAsync(User.Identity.Name);
            if (member is null)
            {
                var info = new BookingViewModel
                {
                    MemberAction = MemberAction.UserMessage,
                    UserMessage = "You are not logged in"
                };

                return View("../Bookings/Booking", info);
            }

            var gymClasses = await GetGymClassesAsync();

            var bookingVM = new BookingViewModel
            {
                Member = member,
                GymClasses = await GetAllGymClassesAsync(),
                MemberAction = MemberAction.Add
            };

            return View("../Bookings/Booking", bookingVM);
        }


        [HttpPost, ActionName("Booking")]
        public async Task<IActionResult> BookingToggleAsync(BookingViewModel model)
        {
            if (model.GymClassId == 0) { return NotFound(); }

            var member = await GetMemberAsync(User.Identity.Name);
            if (member is null) { return NotFound(); }

            var gymClass = await GetGymClassAsync(model.GymClassId.ToString());
            if (gymClass is null) { return NotFound(); }

            var appUserGymClass = new ApplicationUserGymClass
            {
                ApplicationUser = member,
                GymClass = gymClass
            };

            _context.Add(appUserGymClass);
            var addResult = await _context.SaveChangesAsync();


            var bookingVM = new BookingViewModel
            {
                MemberAction = MemberAction.UserMessage,
                UserMessage = addResult > 0
                    ? $"You are now added as participant to {gymClass.Name}"
                    : "Opps! Something went wrong during this action"
            };


            return View("../Bookings/Booking", bookingVM);
        }

        [AllowAnonymous]
        [HttpGet, ActionName("GymClasses")]
        public async Task<IActionResult> GetGymClassesAsync()
        {
            var getResult = await GetAllGymClassesAsync();

            return View("GymClassMain", getResult);
        }



        private bool GymPassExists(int id)
        {
            return (_context.GymPasses?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // *******************************************************************
        // Gymclass queries - private
        // *******************************************************************

        /// <summary>
        /// Returns ViewModel
        /// </summary>
        /// <param name="_gymClassId"></param>
        /// <returns></returns>
        private async Task<GymClassViewModel?> GetGymClassVMAsync(string _gymClassId)
        {
            return await _mapper.
                ProjectTo<GymClassViewModel>(_context.GymPasses)
                .FirstOrDefaultAsync(g => g.Id == int.Parse(_gymClassId));
        }

        /// <summary>
        /// Returns Entity
        /// </summary>
        /// <param name="_gymClassId"></param>
        /// <returns></returns>
        private async Task<GymClass?> GetGymClassAsync(string _gymClassId) =>
            await _context.GymPasses.FirstOrDefaultAsync(g => g.Id == int.Parse(_gymClassId));


        private async Task<IEnumerable<GymClassViewModel>> GetAllGymClassesAsync() =>
            await _mapper.ProjectTo<GymClassViewModel>(_context.GymPasses)
            .OrderBy(a => a.Name)
            .ToListAsync();


        // *******************************************************************
        // Member queries - private
        // *******************************************************************




        private async Task<MemberViewModel> GetMemberVMAsync(string _memberId)
        {
            var member = await GetMemberAsync(_memberId);

            return _mapper.Map<MemberViewModel>(member);
        }


        private async Task<ApplicationUser> GetMemberAsync(string _memberIdOrName)
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



        private async Task<IEnumerable<MemberViewModel>> GetAllMemberAsync()
        {
            return await _mapper.ProjectTo<MemberViewModel>(_context.Users)
                .OrderBy(a => a.FirstName)
                .ToListAsync();
        }
    }
}
