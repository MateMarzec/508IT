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
    public class ManagerCredentialsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagerCredentialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ManagerCredentials
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ManagersCredentials.Include(m => m.EventManager);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ManagerCredentials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var managerCredentials = await _context.ManagersCredentials
                .Include(m => m.EventManager)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (managerCredentials == null)
            {
                return NotFound();
            }

            return View(managerCredentials);
        }

        // GET: ManagerCredentials/Create
        public IActionResult Create()
        {
            ViewData["ManagerID"] = new SelectList(_context.EventManagers, "ID", "FirstName");
            return View();
        }

        // POST: ManagerCredentials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ManagerID,Username,Password,Email,ID,FirstName,LastName,PhoneNumber")] ManagerCredentials managerCredentials)
        {
            if (ModelState.IsValid)
            {
                _context.Add(managerCredentials);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManagerID"] = new SelectList(_context.EventManagers, "ID", "FirstName", managerCredentials.ManagerID);
            return View(managerCredentials);
        }

        // GET: ManagerCredentials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var managerCredentials = await _context.ManagersCredentials.FindAsync(id);
            if (managerCredentials == null)
            {
                return NotFound();
            }
            ViewData["ManagerID"] = new SelectList(_context.EventManagers, "ID", "FirstName", managerCredentials.ManagerID);
            return View(managerCredentials);
        }

        // POST: ManagerCredentials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ManagerID,Username,Password,Email,ID,FirstName,LastName,PhoneNumber")] ManagerCredentials managerCredentials)
        {
            if (id != managerCredentials.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(managerCredentials);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManagerCredentialsExists(managerCredentials.ID))
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
            ViewData["ManagerID"] = new SelectList(_context.EventManagers, "ID", "FirstName", managerCredentials.ManagerID);
            return View(managerCredentials);
        }

        // GET: ManagerCredentials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var managerCredentials = await _context.ManagersCredentials
                .Include(m => m.EventManager)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (managerCredentials == null)
            {
                return NotFound();
            }

            return View(managerCredentials);
        }

        // POST: ManagerCredentials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var managerCredentials = await _context.ManagersCredentials.FindAsync(id);
            _context.ManagersCredentials.Remove(managerCredentials);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ManagerCredentialsExists(int id)
        {
            return _context.ManagersCredentials.Any(e => e.ID == id);
        }
    }
}
