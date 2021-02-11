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
    public class ParticipantCredentialsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParticipantCredentialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ParticipantCredentials
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ParticipantsCredentials.Include(p => p.EventParticipant);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ParticipantCredentials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participantCredentials = await _context.ParticipantsCredentials
                .Include(p => p.EventParticipant)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (participantCredentials == null)
            {
                return NotFound();
            }

            return View(participantCredentials);
        }

        // GET: ParticipantCredentials/Create
        public IActionResult Create()
        {
            ViewData["ParticipantID"] = new SelectList(_context.EventParticipants, "ID", "FirstName");
            return View();
        }

        // POST: ParticipantCredentials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ParticipantID,Username,Password,Email,ID,FirstName,LastName,PhoneNumber")] ParticipantCredentials participantCredentials)
        {
            if (ModelState.IsValid)
            {
                _context.Add(participantCredentials);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParticipantID"] = new SelectList(_context.EventParticipants, "ID", "FirstName", participantCredentials.ParticipantID);
            return View(participantCredentials);
        }

        // GET: ParticipantCredentials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participantCredentials = await _context.ParticipantsCredentials.FindAsync(id);
            if (participantCredentials == null)
            {
                return NotFound();
            }
            ViewData["ParticipantID"] = new SelectList(_context.EventParticipants, "ID", "FirstName", participantCredentials.ParticipantID);
            return View(participantCredentials);
        }

        // POST: ParticipantCredentials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ParticipantID,Username,Password,Email,ID,FirstName,LastName,PhoneNumber")] ParticipantCredentials participantCredentials)
        {
            if (id != participantCredentials.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(participantCredentials);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipantCredentialsExists(participantCredentials.ID))
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
            ViewData["ParticipantID"] = new SelectList(_context.EventParticipants, "ID", "FirstName", participantCredentials.ParticipantID);
            return View(participantCredentials);
        }

        // GET: ParticipantCredentials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participantCredentials = await _context.ParticipantsCredentials
                .Include(p => p.EventParticipant)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (participantCredentials == null)
            {
                return NotFound();
            }

            return View(participantCredentials);
        }

        // POST: ParticipantCredentials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var participantCredentials = await _context.ParticipantsCredentials.FindAsync(id);
            _context.ParticipantsCredentials.Remove(participantCredentials);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParticipantCredentialsExists(int id)
        {
            return _context.ParticipantsCredentials.Any(e => e.ID == id);
        }
    }
}
