using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToursAndTravells.Models;

namespace ToursAndTravells.Controllers
{
    public class UsersController : Controller
    {
        private readonly TandTDBContext _context;

        public UsersController(TandTDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var users = _context.Users
                .Include(u => u.Country)
                .Include(u => u.State)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                users = users.Where(u =>
                    u.UserName.ToLower().Contains(searchString) ||
                    u.UserEmail.ToLower().Contains(searchString) ||
                    u.UserPhone.Contains(searchString) ||
                    u.State.StateName.ToLower().Contains(searchString) ||
                    u.Country.CountryName.ToLower().Contains(searchString)
                );
            }

            ViewData["SearchString"] = searchString;
            return View(await users.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Country)
                .Include(u => u.State)
                .FirstOrDefaultAsync(m => m.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> AddOrEdit(int? id)
        {
            ViewBag.Countries = new SelectList(_context.Countries, "CountryId", "CountryName");

            if (id == null)
            {
                return View(new User());
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Countries = new SelectList(_context.Countries, "CountryId", "CountryName", user.CountryId);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("UserId,UserName,UserEmail,UserPhone,UserPassword,CountryId,StateId")] User user)
        {
            if (ModelState.IsValid)
            {
                if (id == 0) 
                {
                    _context.Add(user);
                }
                else 
                {
                    try
                    {
                        _context.Update(user);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserExists(user.UserId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryName", user.CountryId);
            ViewData["StateId"] = new SelectList(_context.State, "StateId", "StateName", user.StateId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
        [HttpGet]
        public JsonResult GetStatesByCountry(int countryId)
        {
            var states = _context.State
                .Where(s => s.CountryId == countryId)
                .Select(s => new { s.StateId, s.StateName })
                .ToList();

            return Json(states);
        }

    }
}
