using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventsPlusApp.Data;
using EventsPlusApp.Models;

namespace EventsPlusApp.Controllers
{
    public class EventLocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventLocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EventLocations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EventLocations.Include(e => e.Owner);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: EventLocations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventLocation = await _context.EventLocations
                .Include(e => e.Owner)
                .FirstOrDefaultAsync(m => m.LocationID == id);
            if (eventLocation == null)
            {
                return NotFound();
            }

            return View(eventLocation);
        }

        // GET: EventLocations/Create
        public IActionResult Create()
        {
            ViewData["OwnerID"] = new SelectList(_context.Owners, "ID", "FirstName");
            return View();
        }

        // POST: EventLocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocationID,MaximumNumberofParticipants,LocationName,PostCode,Address,City,Country,OwnerID")] EventLocation eventLocation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventLocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerID"] = new SelectList(_context.Owners, "ID", "FirstName", eventLocation.OwnerID);
            return View(eventLocation);
        }

        // GET: EventLocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventLocation = await _context.EventLocations.FindAsync(id);
            if (eventLocation == null)
            {
                return NotFound();
            }
            ViewData["OwnerID"] = new SelectList(_context.Owners, "ID", "FirstName", eventLocation.OwnerID);
            return View(eventLocation);
        }

        // POST: EventLocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LocationID,MaximumNumberofParticipants,LocationName,PostCode,Address,City,Country,OwnerID")] EventLocation eventLocation)
        {
            if (id != eventLocation.LocationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventLocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventLocationExists(eventLocation.LocationID))
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
            ViewData["OwnerID"] = new SelectList(_context.Owners, "ID", "FirstName", eventLocation.OwnerID);
            return View(eventLocation);
        }

        // GET: EventLocations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventLocation = await _context.EventLocations
                .Include(e => e.Owner)
                .FirstOrDefaultAsync(m => m.LocationID == id);
            if (eventLocation == null)
            {
                return NotFound();
            }

            return View(eventLocation);
        }

        // POST: EventLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventLocation = await _context.EventLocations.FindAsync(id);
            _context.EventLocations.Remove(eventLocation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventLocationExists(int id)
        {
            return _context.EventLocations.Any(e => e.LocationID == id);
        }
    }
}
