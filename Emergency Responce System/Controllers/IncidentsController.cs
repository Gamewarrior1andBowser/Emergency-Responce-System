using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Emergency_Responce_System.Models;
using Emergency_Responce_System.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Emergency_Responce_System.Controllers
{
	public class IncidentsController : Controller
	{
		private readonly AppDbContext _context;

		public IncidentsController(AppDbContext context)
		{
			_context = context;
		}

		// GET: Incidents
		public async Task<IActionResult> Index()
		{
			return View(await _context.Incidents
				.Include(i => i.Updates)
				.Include(i => i.Responders)
				.ToListAsync());
		}


		// GET: Incidents/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var incidents = await _context.Incidents
				.Include(i => i.Updates)
				.Include(i => i.Responders)
				.FirstOrDefaultAsync(m => m.IncidentID == id);

			if (incidents == null)
			{
				return NotFound();
			}

			return View(incidents);
		}

		// GET: Incidents/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var incidents = await _context.Incidents.FindAsync(id);
			if (incidents == null)
			{
				return NotFound();
			}
			return View(incidents);
		}

		// POST: Incidents/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("IncidentID,Title,Description,Location")] Incidents incidents)
		{
			if (id != incidents.IncidentID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(incidents);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!IncidentsExists(incidents.IncidentID))
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
			return View(incidents);
		}

		// GET: Incidents/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var incidents = await _context.Incidents
				.FirstOrDefaultAsync(m => m.IncidentID == id);
			if (incidents == null)
			{
				return NotFound();
			}

			return View(incidents);
		}


		// POST: Incidents/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var incidents = await _context.Incidents.FindAsync(id);
			if (incidents != null)
			{
				_context.Incidents.Remove(incidents);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool IncidentsExists(int id)
		{
			return _context.Incidents.Any(e => e.IncidentID == id);
		}
	}
}
