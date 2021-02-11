﻿using System;
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
    public class EventAssignmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventAssignmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EventAssignments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EventAssignments.Include(e => e.Event).Include(e => e.EventManager);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: EventAssignments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventAssignment = await _context.EventAssignments
                .Include(e => e.Event)
                .Include(e => e.EventManager)
                .FirstOrDefaultAsync(m => m.EventManagerID == id);
            if (eventAssignment == null)
            {
                return NotFound();
            }

            return View(eventAssignment);
        }

        // GET: EventAssignments/Create
        public IActionResult Create()
        {
            ViewData["EventID"] = new SelectList(_context.Events, "EventID", "Title");
            ViewData["EventManagerID"] = new SelectList(_context.EventManagers, "ID", "FirstName");
            return View();
        }

        // POST: EventAssignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventManagerID,EventID")] EventAssignment eventAssignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventID"] = new SelectList(_context.Events, "EventID", "Title", eventAssignment.EventID);
            ViewData["EventManagerID"] = new SelectList(_context.EventManagers, "ID", "FirstName", eventAssignment.EventManagerID);
            return View(eventAssignment);
        }

        // GET: EventAssignments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventAssignment = await _context.EventAssignments.FindAsync(id);
            if (eventAssignment == null)
            {
                return NotFound();
            }
            ViewData["EventID"] = new SelectList(_context.Events, "EventID", "Title", eventAssignment.EventID);
            ViewData["EventManagerID"] = new SelectList(_context.EventManagers, "ID", "FirstName", eventAssignment.EventManagerID);
            return View(eventAssignment);
        }

        // POST: EventAssignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventManagerID,EventID")] EventAssignment eventAssignment)
        {
            if (id != eventAssignment.EventManagerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventAssignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventAssignmentExists(eventAssignment.EventManagerID))
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
            ViewData["EventID"] = new SelectList(_context.Events, "EventID", "Title", eventAssignment.EventID);
            ViewData["EventManagerID"] = new SelectList(_context.EventManagers, "ID", "FirstName", eventAssignment.EventManagerID);
            return View(eventAssignment);
        }

        // GET: EventAssignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventAssignment = await _context.EventAssignments
                .Include(e => e.Event)
                .Include(e => e.EventManager)
                .FirstOrDefaultAsync(m => m.EventManagerID == id);
            if (eventAssignment == null)
            {
                return NotFound();
            }

            return View(eventAssignment);
        }

        // POST: EventAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventAssignment = await _context.EventAssignments.FindAsync(id);
            _context.EventAssignments.Remove(eventAssignment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventAssignmentExists(int id)
        {
            return _context.EventAssignments.Any(e => e.EventManagerID == id);
        }
    }
}
