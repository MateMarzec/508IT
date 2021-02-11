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
    public class EventManagersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventManagersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EventManagers
        public async Task<IActionResult> Index()
        {
            return View(await _context.EventManagers.ToListAsync());
        }

        // GET: EventManagers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventManager = await _context.EventManagers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (eventManager == null)
            {
                return NotFound();
            }

            return View(eventManager);
        }

        // GET: EventManagers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EventManagers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,PhoneNumber")] EventManager eventManager)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventManager);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventManager);
        }

        // GET: EventManagers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventManager = await _context.EventManagers.FindAsync(id);
            if (eventManager == null)
            {
                return NotFound();
            }
            return View(eventManager);
        }

        // POST: EventManagers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,PhoneNumber")] EventManager eventManager)
        {
            if (id != eventManager.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventManager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventManagerExists(eventManager.ID))
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
            return View(eventManager);
        }

        // GET: EventManagers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventManager = await _context.EventManagers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (eventManager == null)
            {
                return NotFound();
            }

            return View(eventManager);
        }

        // POST: EventManagers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventManager = await _context.EventManagers.FindAsync(id);
            _context.EventManagers.Remove(eventManager);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventManagerExists(int id)
        {
            return _context.EventManagers.Any(e => e.ID == id);
        }
    }
}
