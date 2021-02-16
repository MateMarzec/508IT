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
    public class OwnersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OwnersController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Policy = "readpolicy")]
        // GET: Owners
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            //Sorting
            ViewData["FirstNameSort"] = String.IsNullOrEmpty(sortOrder) ? "FirstName_Desc" : "";
            ViewData["LastNameSort"] = sortOrder == "LastName_Asc" ? "LastName_Desc" : "LastName_Asc";
            ViewData["PhoneNumberSort"] = sortOrder == "PhoneNumber_Asc" ? "PhoneNumber_Desc" : "PhoneNumber_Asc";
            var Owners = from f in _context.Owners
                         select f;
            switch (sortOrder)
            {
                case "FirstName_Desc":
                    Owners = Owners.OrderByDescending(f => f.FirstName);
                    break;
                case "LastName_Asc":
                    Owners = Owners.OrderBy(f => f.LastName);
                    break;
                case "LastName_Desc":
                    Owners = Owners.OrderByDescending(f => f.LastName);
                    break;
                case "PhoneNumber_Desc":
                    Owners = Owners.OrderByDescending(f => f.PhoneNumber);
                    break;
                case "PhoneNumber_Asc":
                    Owners = Owners.OrderBy(f => f.PhoneNumber);
                    break;
                default:
                    Owners = Owners.OrderBy(f => f.FirstName);
                    break;
            }
            //Searching
            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                Owners = Owners.Where(f => f.FirstName.Contains(searchString)
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
            return View(await PaginatedList<Owner>.CreateAsync(Owners.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Authorize(Policy = "readpolicy")]
        // GET: Owners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owners
                .FirstOrDefaultAsync(m => m.ID == id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }
        [Authorize(Policy = "writepolicy")]
        // GET: Owners/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Policy = "writepolicy")]
        // POST: Owners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,PhoneNumber")] Owner owner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(owner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(owner);
        }
        [Authorize(Policy = "writepolicy")]
        // GET: Owners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owners.FindAsync(id);
            if (owner == null)
            {
                return NotFound();
            }
            return View(owner);
        }
        [Authorize(Policy = "writepolicy")]
        // POST: Owners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,PhoneNumber")] Owner owner)
        {
            if (id != owner.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(owner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OwnerExists(owner.ID))
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
            return View(owner);
        }
        [Authorize(Policy = "writepolicy")]
        // GET: Owners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owners
                .FirstOrDefaultAsync(m => m.ID == id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }
        [Authorize(Policy = "writepolicy")]
        // POST: Owners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var owner = await _context.Owners.FindAsync(id);
            _context.Owners.Remove(owner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OwnerExists(int id)
        {
            return _context.Owners.Any(e => e.ID == id);
        }
    }
}
