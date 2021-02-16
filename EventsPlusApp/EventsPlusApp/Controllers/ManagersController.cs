using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventsPlusApp.Data;
using EventsPlusApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace EventsPlusApp.Controllers
{
    public class ManagersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagersController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Policy = "readpolicy")]
        public async Task<IActionResult> Index_defualt()
        {
            return View(await _context.Managers.ToListAsync());
        }
        [Authorize(Policy = "readpolicy")]
        // GET: Managers
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            //Sorting
            ViewData["FirstNameSort"] = String.IsNullOrEmpty(sortOrder) ? "FirstName_Desc" : "";
            ViewData["LastNameSort"] = sortOrder == "LastName_Asc" ? "LastName_Desc" : "LastName_Asc";
            ViewData["PhoneNumberSort"] = sortOrder == "PhoneNumber_Asc" ? "PhoneNumber_Desc" : "PhoneNumber_Asc";
            var Managers = from f in _context.Managers
                               select f;
            switch (sortOrder)
            {
                case "FirstName_Desc":
                    Managers = Managers.OrderByDescending(f => f.FirstName);
                    break;
                case "LastName_Asc":
                    Managers = Managers.OrderBy(f => f.LastName);
                    break;
                case "LastName_Desc":
                    Managers = Managers.OrderByDescending(f => f.LastName);
                    break;
                case "PhoneNumber_Asc":
                    Managers = Managers.OrderBy(f => f.PhoneNumber);
                    break;
                case "PhoneNumber_Desc":
                    Managers = Managers.OrderByDescending(f => f.PhoneNumber);
                    break;
                default:
                    Managers = Managers.OrderBy(f => f.FirstName);
                    break;
            } //Searching
            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                Managers = Managers.Where(f => f.FirstName.Contains(searchString)
                         || f.LastName.Contains(searchString)
                         || f.PhoneNumber.Contains(searchString));
            }

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            //Paging and return view
            int pageSize = 10;
            return View(await PaginatedList<Manager>.CreateAsync(Managers.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        [Authorize(Policy = "readpolicy")]
        // GET: Managers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }
        [Authorize(Policy = "writepolicy")]
        // GET: Managers/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Policy = "writepolicy")]
        // POST: Managers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,PhoneNumber")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                _context.Add(manager);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(manager);
        }
        [Authorize(Policy = "writepolicy")]
        // GET: Managers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers.FindAsync(id);
            if (manager == null)
            {
                return NotFound();
            }
            return View(manager);
        }
        [Authorize(Policy = "writepolicy")]
        // POST: Managers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,PhoneNumber")] Manager manager)
        {
            if (id != manager.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManagerExists(manager.ID))
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
            return View(manager);
        }
        [Authorize(Policy = "writepolicy")]
        // GET: Managers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }
        [Authorize(Policy = "writepolicy")]
        // POST: Managers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var manager = await _context.Managers.FindAsync(id);
            _context.Managers.Remove(manager);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ManagerExists(int id)
        {
            return _context.Managers.Any(e => e.ID == id);
        }
    }
}
