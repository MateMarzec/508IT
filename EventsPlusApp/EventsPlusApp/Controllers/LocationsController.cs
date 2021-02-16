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
    public class LocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocationsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Policy = "readpolicy")]
        public async Task<IActionResult> Index_defualt()
        {
            var applicationDbContext = _context.Locations.Include(l => l.Owner);
            return View(await _context.Locations.ToListAsync());
        }
        [Authorize(Policy = "readpolicy")]
        // GET: Locations
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            //Sorting
            ViewData["NameSort"] = String.IsNullOrEmpty(sortOrder) ? "Name_Desc" : "";
            ViewData["MaximumNumberofParticipantsSort"] = sortOrder == "MaximumNumberofParticipants_Asc" ? "MaximumNumberofParticipants_Desc" : "MaximumNumberofParticipants_Asc";
            ViewData["PostCodeSort"] = sortOrder == "PostCode_Asc" ? "PostCode_Desc" : "PostCode_Asc";
            ViewData["AddressSort"] = sortOrder == "Address_Asc" ? "Address_Desc" : "Address_Asc";
            ViewData["CitySort"] = sortOrder == "City_Asc" ? "City_Desc" : "City_Asc";
            ViewData["CountrySort"] = sortOrder == "Country_Asc" ? "Country_Desc" : "Country_Asc";
            ViewData["OwnerIDSort"] = sortOrder == "OwnerID_Asc" ? "OwnerID_Desc" : "OwnerID_Asc";
            var Locations = from f in _context.Locations.Include(f => f.Owner)
                               select f;
            switch (sortOrder)
            {
                case "Name_Desc":
                    Locations = Locations.OrderByDescending(f => f.Name);
                    break;
                case "MaximumNumberofParticipants_Asc":
                    Locations = Locations.OrderBy(f => f.MaximumNumberofParticipants);
                    break;
                case "MaximumNumberofParticipants_Desc":
                    Locations = Locations.OrderByDescending(f => f.MaximumNumberofParticipants);
                    break;
                case "PostCode_Asc":
                    Locations = Locations.OrderBy(f => f.PostCode);
                    break;
                case "PostCode_Desc":
                    Locations = Locations.OrderByDescending(f => f.PostCode);
                    break;
                case "Address_Asc":
                    Locations = Locations.OrderBy(f => f.Address);
                    break;
                case "Address_Desc":
                    Locations = Locations.OrderByDescending(f => f.Address);
                    break;
                case "City_Asc":
                    Locations = Locations.OrderBy(f => f.City);
                    break;
                case "City_Desc":
                    Locations = Locations.OrderByDescending(f => f.City);
                    break;
                case "Country_Asc":
                    Locations = Locations.OrderBy(f => f.Country);
                    break;
                case "Country_Desc":
                    Locations = Locations.OrderByDescending(f => f.Country);
                    break;
                case "OwnerID_Asc":
                    Locations = Locations.OrderBy(f => f.Owner.ID);
                    break;
                case "OwnerID_Desc":
                    Locations = Locations.OrderByDescending(f => f.Owner.ID);
                    break;
                default:
                    Locations = Locations.OrderBy(f => f.Name);
                    break;
            }
            //Searching
            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                Locations = Locations.Where(f => f.Name.Contains(searchString)
                         || f.PostCode.Contains(searchString)
                         || f.Address.Contains(searchString)
                         || f.City.Contains(searchString)
                         || f.Country.Contains(searchString)
                         || f.Owner.FirstName.Contains(searchString));
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
            return View(await PaginatedList<Location>.CreateAsync(Locations.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        [Authorize(Policy = "readpolicy")]
        // GET: Locations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .Include(l => l.Owner)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }
        [Authorize(Policy = "writepolicy")]
        // GET: Locations/Create
        public IActionResult Create()
        {
            ViewData["OwnerID"] = new SelectList(_context.Owners, "ID", "FirstName");
            return View();
        }
        [Authorize(Policy = "writepolicy")]
        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,MaximumNumberofParticipants,Name,PostCode,Address,City,Country,OwnerID")] Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerID"] = new SelectList(_context.Owners, "ID", "FirstName", location.OwnerID);
            return View(location);
        }
        [Authorize(Policy = "writepolicy")]
        // GET: Locations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            ViewData["OwnerID"] = new SelectList(_context.Owners, "ID", "FirstName", location.OwnerID);
            return View(location);
        }
        [Authorize(Policy = "writepolicy")]
        // POST: Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,MaximumNumberofParticipants,Name,PostCode,Address,City,Country,OwnerID")] Location location)
        {
            if (id != location.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.ID))
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
            ViewData["OwnerID"] = new SelectList(_context.Owners, "ID", "FirstName", location.OwnerID);
            return View(location);
        }
        [Authorize(Policy = "writepolicy")]
        // GET: Locations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .Include(l => l.Owner)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }
        [Authorize(Policy = "writepolicy")]
        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.ID == id);
        }
    }
}
