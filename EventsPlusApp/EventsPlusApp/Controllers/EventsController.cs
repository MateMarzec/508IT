﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventsPlusApp.Data;
using EventsPlusApp.Models;
using EventsPlusApp.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace EventsPlusApp.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Policy = "readpolicy")]
        // GET: Events
        public async Task<IActionResult> Index_defualt()
        {
            var applicationDbContext = _context.Events.Include(l => l.Location).Include(l => l.Manager);
            return View(await applicationDbContext.ToListAsync());
        }
        // Display events with participants
        [Authorize(Policy = "readpolicy")]
        public ViewResult Index(int? id)
        {
            var viewModel = new Booking();
            viewModel.Events = _context.Events
              .Include(l => l.Location)
              .Include(l => l.Manager)
              .Include(c => c.Participants)
              .OrderBy(c => c.Title);

            if (id != null)
            {
                ViewBag.eventID = id.Value;
                viewModel.Participants = viewModel.Events.Where(
                  f => f.ID == id.Value).Single().Participants;
            }

            return View(viewModel);
        }

        [Authorize(Policy = "readpolicy")]
        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var @event = await _context.Events
                .Include(l => l.Location)
                .Include(l => l.Manager)
                .Include(l => l.Participants)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (@event == null)
            {
                return NotFound();
            }
            ViewBag.FootballPlayers = _context.Events
                .Include(c => c.Participants).Where(c => c.ID == id)
                .Single().Participants;

            return View(@event);
        }
        [Authorize(Policy = "writepolicy")]
        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["LocationID"] = new SelectList(_context.Locations, "ID", "Address");
            ViewData["ManagerID"] = new SelectList(_context.Managers, "ID", "FirstName");
            return View();
        }
        [Authorize(Policy = "writepolicy")]
        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Type,DateAndTime,LocationID,ManagerID,ParticipantID")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationID"] = new SelectList(_context.Locations, "ID", "Address", @event.LocationID);
            ViewData["ManagerID"] = new SelectList(_context.Managers, "ID", "FirstName", @event.ManagerID);
            return View(@event);
        }
        [Authorize(Policy = "writepolicy")]
        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            ViewData["LocationID"] = new SelectList(_context.Locations, "ID", "Address", @event.LocationID);
            ViewData["ManagerID"] = new SelectList(_context.Managers, "ID", "FirstName", @event.ManagerID);
            return View(@event);
        }
        [Authorize(Policy = "writepolicy")]
        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Type,DateAndTime,LocationID,ManagerID,ParticipantID")] Event @event)
        {
            if (id != @event.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.ID))
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
            ViewData["LocationID"] = new SelectList(_context.Locations, "ID", "Address", @event.LocationID);
            ViewData["ManagerID"] = new SelectList(_context.Managers, "ID", "FirstName", @event.ManagerID);
            return View(@event);
        }
        [Authorize(Policy = "writepolicy")]
        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(l => l.Location)
                .Include(l => l.Manager)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }
        [Authorize(Policy = "writepolicy")]
        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.ID == id);
        }
    }
}
