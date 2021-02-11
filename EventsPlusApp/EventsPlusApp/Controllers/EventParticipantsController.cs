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
    public class EventParticipantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventParticipantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EventParticipants
        public async Task<IActionResult> Index()
        {
            return View(await _context.EventParticipants.ToListAsync());
        }

        // GET: EventParticipants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventParticipant = await _context.EventParticipants
                .FirstOrDefaultAsync(m => m.ID == id);
            if (eventParticipant == null)
            {
                return NotFound();
            }

            return View(eventParticipant);
        }

        // GET: EventParticipants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EventParticipants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,PhoneNumber")] EventParticipant eventParticipant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventParticipant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventParticipant);
        }

        // GET: EventParticipants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventParticipant = await _context.EventParticipants.FindAsync(id);
            if (eventParticipant == null)
            {
                return NotFound();
            }
            return View(eventParticipant);
        }

        // POST: EventParticipants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,PhoneNumber")] EventParticipant eventParticipant)
        {
            if (id != eventParticipant.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventParticipant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventParticipantExists(eventParticipant.ID))
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
            return View(eventParticipant);
        }

        // GET: EventParticipants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventParticipant = await _context.EventParticipants
                .FirstOrDefaultAsync(m => m.ID == id);
            if (eventParticipant == null)
            {
                return NotFound();
            }

            return View(eventParticipant);
        }

        // POST: EventParticipants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventParticipant = await _context.EventParticipants.FindAsync(id);
            _context.EventParticipants.Remove(eventParticipant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventParticipantExists(int id)
        {
            return _context.EventParticipants.Any(e => e.ID == id);
        }
    }
}
