using AutoMapper;
using GymApp14V1.Core.Models;
using GymApp14V1.Core.ViewModels;
using GymApp14V1.Repository.Interfaces;
using GymApp14V1.Util.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymApp14V1.Controllers
{
    [Authorize]
    public class GymClassController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;


        public GymClassController(UserManager<ApplicationUser> userManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }



        [AllowAnonymous]
        [HttpGet, ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            ViewBag.PageHeader = new PageHeaderViewModel
            {
                HeadLine = "Welcome to Gainers Gym",
                SubTitle = "Most visited gym in the area",
                Content = "Below you find our selection. Just register an account if you are new here or login and start a helthier life"
            };

            var test = await GetAllGymClassesAsync(User.IsInRole(ClientArgs.ADMIN_ROLE));

            return View("../GymClass/Index", await GetAllGymClassesAsync(User.IsInRole(ClientArgs.ADMIN_ROLE)));

        }

        [HttpGet, ActionName("Details")]
        public async Task<IActionResult> DetailsAsync(int? id)
        {
            if (id == null) { return NotFound(); }

            var getResult = await GetGymClassVMAsync(id.ToString(), User.IsInRole(ClientArgs.ADMIN_ROLE));
            if (getResult == null) { return NotFound(); }

            getResult.PageHeader = GetPageHeader("Details", "Additional information");

            return View("../GymClass/GymClassDetails", getResult);
        }

        [HttpGet, ActionName("Create")]
        public IActionResult Create()
        {
            return View("../GymClass/GymClassCreate", new GymClassViewModel
            {
                PageHeader = GetPageHeader("Add GymClass", "CRUD operation")
            });
        }

        // POST: GymPass/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        //[ModelStateValidation] //- Todo: Add middleware and catch any exception there. 
        public async Task<IActionResult> CreateAsync(GymClassViewModel model)
        {
            Expression<Func<GymClass, bool>> predicate = f => f.Name.ToLower() == model.Name.ToLower();

            var isExist = await FindAsync(predicate, User.IsInRole(ClientArgs.ADMIN_ROLE));
            if (isExist.Any()) { return View("../GymClass/GymClassCreate", model); }

            var entity = _mapper.Map<GymClass>(model);


            if (ModelState.IsValid)
            {
                _unitOfWork.GymClassRepo.Add(entity);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                model.PageHeader = GetPageHeader("Add GymClass", "CRUD operation");
                return View("../GymClass/GymClassCreate", model);
            }
        }


        [Authorize(Roles = ClientArgs.ADMIN_ROLE)]
        [HttpGet, ActionName("BookingXX")]
        public async Task<IActionResult> BookingAsync(string gymClassId)
        {
            if (string.IsNullOrWhiteSpace(gymClassId)) { return NotFound(); }
            var getResult = await GetGymClassVMAsync(gymClassId);
            return View(getResult);
        }



        [Authorize(Roles = ClientArgs.ADMIN_ROLE)]
        [HttpGet, ActionName("Edit")]
        public async Task<IActionResult> EditAsync(int? id)
        {
            if (id == null) { return NotFound(); }

            var getResult = await GetGymClassVMAsync(id.ToString(), User.IsInRole(ClientArgs.ADMIN_ROLE));
            if (getResult == null) { return NotFound(); }


            getResult.PageHeader = GetPageHeader("Update GymClass", "CRUD operation");
            return View("../GymClass/GymClassEdit", getResult);

        }

        [Authorize(Roles = ClientArgs.ADMIN_ROLE)]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, GymClassViewModel model)
        {
            if (id != model.Id) { return NotFound(); }

            var entity = await GetGymClassAsync(id.ToString(), true);
            if (entity is null) { return NotFound(); }

            entity.Description = model.Description;
            entity.Name = model.Name;
            entity.Duration = model.Duration;
            entity.StartTime = model.StartTime;


            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.GymClassRepo.Update(entity);
                    await _unitOfWork.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var gymPassExists = await GymPassExists(entity.Id);

                    if (!gymPassExists)
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

            model.PageHeader = GetPageHeader("Update GymClass", "CRUD operation");
            return View("../GymClass/GymClassEdit", model);
        }

        [Authorize(Roles = ClientArgs.ADMIN_ROLE)]
        [HttpGet, ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync(int? id)
        {
            if (id == null) { return NotFound(); }

            var gymClass = await GetGymClassVMAsync(id.ToString());
            if (gymClass == null) { return NotFound(); }

            gymClass.PageHeader = GetPageHeader("Delete GymClass", "CRUD operation");

            return View("../GymClass/GymClassDelete", gymClass);

        }

        [Authorize(Roles = ClientArgs.ADMIN_ROLE)]
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedAsync(int id)
        {
            var entity = await GetGymClassAsync(id.ToString());
            if (entity is null) { return NotFound(); }

            _unitOfWork.GymClassRepo.Remove(entity);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet, ActionName("Booking")]
        public async Task<IActionResult> BookingToggleAsync(int? id)
        {
            if (id is null) { return BadRequest(); }

            var member = await GetMemberVMAsync(User.Identity.Name);
            if (member is null) return NotFound();

            var gymClass = await GetGymClassVMAsync(id.ToString());

            if (member is null)
            {
                var info = new BookingViewModel
                {
                    GymClass = gymClass,
                    MemberAction = MemberAction.UserMessage,
                    UserMessage = "You are not logged in"
                };

                return View("../Bookings/Booking", info);
            }

            var gymClasses = await GetGymClassesAsync();

            var bookingVM = new BookingViewModel
            {
                Member = member,
                GymClass = gymClass,
                PageHeader = GetPageHeader("Reservation - Gym Pass", "Book two sessions for the price of one")
            };

            return View("../Bookings/Booking", bookingVM);
        }

        [HttpPost, ActionName("Booking")]
        public async Task<IActionResult> BookingToggleAsync(BookingViewModel model)
        {
            var member = await GetMemberAsync(User.Identity.Name);
            if (member is null) { return NotFound(); }

            var gymClass = await GetGymClassAsync(model.GymClass.Id.ToString());


            var memberAttn = await GetMemberGymClassAsync(member.Id, gymClass.Id);

            if (memberAttn is null)
            {
                var appUserGymClass = new ApplicationUserGymClass
                {
                    ApplicationUserId = member.Id,
                    GymClassId = gymClass.Id
                };

                _unitOfWork.AppUserGymClassRepo.Add(appUserGymClass);
            }
            else
            {
                _unitOfWork.AppUserGymClassRepo.Remove(memberAttn);
            }


            var addResult = await _unitOfWork.CompleteAsync();


            var bookingVM = new BookingViewModel
            {
                MemberAction = MemberAction.UserMessage,
                UserMessage = addResult > 0
                    ? $"You are now added as participant to {gymClass.Name}"
                    : "Opps! Something went wrong during this action"
            };


            return RedirectToAction(nameof(Index));

            return View("../Bookings/Booking", bookingVM);
        }

        [AllowAnonymous]
        [HttpGet, ActionName("GymClasses")]
        public async Task<IActionResult> GetGymClassesAsync()
        {
            var getResult = await GetAllGymClassesAsync();


            ViewBag.PageHeader = new PageHeaderViewModel
            {
                HeadLine = "Welcome to Gainers Gym",
                SubTitle = "Most visited gym in the area",
                Content = "Below you find our selection. Just register an account if you are new here or login and start a helthier life"
            };


            return View("GymClassMain", getResult);
        }

        [AllowAnonymous]
        [HttpGet, ActionName("GetMembersAndGymClasses")]
        public async Task<IActionResult> GetMembersAndGymClasses()
        {
            var getResult = await GetMemberGymClassesAsync();

            var dd = 0;

            throw new NotImplementedException();
        }





        private async Task<bool> GymPassExists(int id)
        {
            Expression<Func<GymClass, bool>> predicate = i => i.Id == id;

            var isExist = await _unitOfWork.GymClassRepo.Find(predicate).ToListAsync();
            return isExist.Any();
        }

        // *******************************************************************
        // Gymclass queries - private
        // *******************************************************************

        /// <summary>
        /// Returns ViewModel
        /// </summary>
        /// <param name="_gymClassId"></param>
        /// <returns></returns>
        private async Task<GymClassViewModel?> GetGymClassVMAsync(string _gymClassId, bool ignoreQueryFilters = false)
        {
            var getResult = await _unitOfWork.GymClassRepo.GetAsync(_gymClassId, ignoreQueryFilters);
            return _mapper.Map<GymClassViewModel>(getResult);
        }

        /// <summary>
        /// Returns Entity
        /// </summary>
        /// <param name="_gymClassId"></param>
        /// <returns></returns>
        private async Task<GymClass?> GetGymClassAsync(string _gymClassId, bool ignoreQueryFilters = false) =>
                                            await _unitOfWork.GymClassRepo.GetAsync(_gymClassId, ignoreQueryFilters);

        private async Task<IEnumerable<GymClassViewModel>> GetAllGymClassesAsync(bool ignoreQueryFilters = false)
        {
            return await _mapper
                .ProjectTo<GymClassViewModel>(
                _unitOfWork.GymClassRepo.GetAll(ignoreQueryFilters)).ToListAsync();
        }



        // *******************************************************************
        // Member queries - private
        // *******************************************************************


        private async Task<IEnumerable<GymClassViewModel>> FindAsync(
            Expression<Func<GymClass, bool>> predicate, bool ignoreQueryFilter = false)
        {
            var findResult = _unitOfWork.GymClassRepo.Find(predicate, ignoreQueryFilter);
            return await _mapper.ProjectTo<GymClassViewModel>(findResult).ToListAsync();
        }



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
            var users = _unitOfWork.ApplicationUserRepo.GetAll();
            return _mapper.ProjectTo<MemberViewModel>(users).ToList();
        }
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



        /// <summary>
        /// Gets members and All attended classes
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task<IEnumerable<MemberViewModel>> GetMemberGymClassesAsync(string _memberId = "")
        {
            var attendedClss = _unitOfWork.AppUserGymClassRepo.GetAll();

            if (string.IsNullOrWhiteSpace(_memberId))
            {
                return await _mapper.ProjectTo<MemberViewModel>(attendedClss).ToListAsync();
            }


            return await _mapper.ProjectTo<MemberViewModel>(attendedClss)
                .Where(a => a.Id.ToLower() == _memberId.ToLower())
                .ToListAsync();
        }

        /// <summary>
        /// Get GymClasses and all attending menbers
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task<IEnumerable<GymClassViewModel>> GetGymClassesMembersAsync()
        {
            var attendedClss = _unitOfWork.AppUserGymClassRepo.GetAll();
            return await _mapper.ProjectTo<GymClassViewModel>(attendedClss).ToListAsync();
        }


        private async Task<ApplicationUserGymClass> GetMemberGymClassAsync(string memberId, int gymClassId)
        {
            if (string.IsNullOrWhiteSpace(memberId) || gymClassId <= 0) { return default; }

            return await _unitOfWork.AppUserGymClassRepo.GetAsync(memberId, gymClassId);



        }
    }
}
