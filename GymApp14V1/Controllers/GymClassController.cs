﻿using AutoMapper;
using GymApp14V1.Data;
using GymApp14V1.Models;
using GymApp14V1.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymApp14V1.Controllers
{
    //[Authorize]
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
                        View(await _context.GymPasses.ToListAsync()) :
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


        [HttpPost, ActionName("BookingToggle")]
        public async Task<IActionResult> BookingToggleAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) { return NotFound(); }

            var member = await _userManager.FindByNameAsync(User.Identity.Name);
            if (member is null) { return NotFound(); }

            var gymPass = await _context.GymPasses
                .Include(x => x.AttendingMembers)
                .FirstOrDefaultAsync(i => i.Id == int.Parse(id));




            throw new NotImplementedException();


        }


        [HttpGet, ActionName("GetGymClasses")]
        public async Task<IActionResult> GetGymClassesAsync()
        {
            var getResult = await _mapper
                .ProjectTo<GymClassViewModel>(_context.GymPasses)
                .OrderBy(o => o.Name).ToListAsync();

            return View(getResult);
        }



        private bool GymPassExists(int id)
        {
            return (_context.GymPasses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
