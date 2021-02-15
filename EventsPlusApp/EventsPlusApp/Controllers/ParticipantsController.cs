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
    public class ParticipantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParticipantsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Policy = "readpolicy")]
        // GET: Participants
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["FirstNameSort"] = String.IsNullOrEmpty(sortOrder) ? "FirstName_Desc" : "";
            ViewData["LastNameSort"] = sortOrder == "LastName_Asc" ? "LastName_Desc" : "LastName_Asc";
            ViewData["PhoneNumberSort"] = sortOrder == "PhoneNumber_Asc" ? "PhoneNumber_Desc" : "PhoneNumber_Asc";
            ViewData["EventIDSort"] = sortOrder == "EventID_Asc" ? "EventID_Desc" : "EventID_Asc";
            var Participants = from f in _context.Participants.Include(f => f.Event)
                                  select f;
            switch (sortOrder)
            {
                case "FirstName_Desc":
                    Participants = Participants.OrderByDescending(f => f.FirstName);
                    break;
                case "LastName_Asc":
                    Participants = Participants.OrderBy(f => f.LastName);
                    break;
                case "LastName_Desc":
                    Participants = Participants.OrderByDescending(f => f.LastName);
                    break;
                case "PhoneNumber_Asc":
                    Participants = Participants.OrderBy(f => f.PhoneNumber);
                    break;
                case "PhoneNumber_Desc":
                    Participants = Participants.OrderByDescending(f => f.PhoneNumber);
                    break;
                case "EventID_Desc":
                    Participants = Participants.OrderByDescending(f => f.Event.ID);
                    break;
                case "EventID_Asc":
                    Participants = Participants.OrderBy(f => f.Event.ID);
                    break;
                default:
                    Participants = Participants.OrderBy(f => f.FirstName);
                    break;
            }
            return View(Participants);
        }

        [Authorize(Policy = "readpolicy")]
        // GET: Participants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participant = await _context.Participants
                .Include(p => p.Event)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (participant == null)
            {
                return NotFound();
            }

            return View(participant);
        }
        [Authorize(Policy = "writepolicy")]
        // GET: Participants/Create
        public IActionResult Create()
        {
            ViewData["EventID"] = new SelectList(_context.Events, "ID", "Title");
            return View();
        }
        [Authorize(Policy = "writepolicy")]
        // POST: Participants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,PhoneNumber,EventID")] Participant participant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(participant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventID"] = new SelectList(_context.Events, "ID", "Title", participant.EventID);
            return View(participant);
        }
        [Authorize(Policy = "writepolicy")]
        // GET: Participants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participant = await _context.Participants.FindAsync(id);
            if (participant == null)
            {
                return NotFound();
            }
            ViewData["EventID"] = new SelectList(_context.Events, "ID", "Title", participant.EventID);
            return View(participant);
        }
        [Authorize(Policy = "writepolicy")]
        // POST: Participants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,PhoneNumber,EventID")] Participant participant)
        {
            if (id != participant.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(participant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipantExists(participant.ID))
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
            ViewData["EventID"] = new SelectList(_context.Events, "ID", "Title", participant.EventID);
            return View(participant);
        }
        [Authorize(Policy = "writepolicy")]
        // GET: Participants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participant = await _context.Participants
                .Include(p => p.Event)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (participant == null)
            {
                return NotFound();
            }

            return View(participant);
        }
        [Authorize(Policy = "writepolicy")]
        // POST: Participants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var participant = await _context.Participants.FindAsync(id);
            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParticipantExists(int id)
        {
            return _context.Participants.Any(e => e.ID == id);
        }
    }
}
