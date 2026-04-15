using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Emergency_Responce_System.DAL;
using Emergency_Responce_System.Models;

namespace Emergency_Responce_System.Controllers
{
    public class IncidentUpdatesController : Controller
    {
        private readonly AppDbContext _context;

        public IncidentUpdatesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: IncidentUpdates
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.IncidentUpdates.Include(i => i.Incident);
            return View(await appDbContext.ToListAsync());
        }

        // GET: IncidentUpdates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidentUpdates = await _context.IncidentUpdates
                .Include(i => i.Incident)
                .FirstOrDefaultAsync(m => m.UpdateID == id);
            if (incidentUpdates == null)
            {
                return NotFound();
            }

            return View(incidentUpdates);
        }

		// GET: IncidentUpdates/Create
		public IActionResult Create(int incidentId)
		{
			var update = new IncidentUpdates
			{
				IncidentID = incidentId
			};

			return View(update);
		}

		// POST: IncidentUpdates/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(IncidentUpdates incidentUpdates)
		{
			Console.WriteLine("POST HIT");
			Console.WriteLine("IncidentID: " + incidentUpdates.IncidentID);

			incidentUpdates.Date = DateTime.Now;

			_context.Add(incidentUpdates);
			await _context.SaveChangesAsync();

			Console.WriteLine("SAVED SUCCESS");

			return RedirectToAction("Details", "Incidents", new { id = incidentUpdates.IncidentID });
		}

		// GET: IncidentUpdates/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidentUpdates = await _context.IncidentUpdates.FindAsync(id);
            if (incidentUpdates == null)
            {
                return NotFound();
            }
            ViewData["IncidentID"] = new SelectList(_context.Incidents, "IncidentID", "IncidentID", incidentUpdates.IncidentID);
            return View(incidentUpdates);
        }

        // POST: IncidentUpdates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UpdateID,Date,Status,Description,IncidentID")] IncidentUpdates incidentUpdates)
        {
            if (id != incidentUpdates.UpdateID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incidentUpdates);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncidentUpdatesExists(incidentUpdates.UpdateID))
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
            ViewData["IncidentID"] = new SelectList(_context.Incidents, "IncidentID", "IncidentID", incidentUpdates.IncidentID);
            return View(incidentUpdates);
        }

        // GET: IncidentUpdates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidentUpdates = await _context.IncidentUpdates
                .Include(i => i.Incident)
                .FirstOrDefaultAsync(m => m.UpdateID == id);
            if (incidentUpdates == null)
            {
                return NotFound();
            }

            return View(incidentUpdates);
        }

        // POST: IncidentUpdates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incidentUpdates = await _context.IncidentUpdates.FindAsync(id);
            if (incidentUpdates != null)
            {
                _context.IncidentUpdates.Remove(incidentUpdates);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncidentUpdatesExists(int id)
        {
            return _context.IncidentUpdates.Any(e => e.UpdateID == id);
        }
    }
}
