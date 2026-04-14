using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergency_Responce_System.Models;
using Emergency_Responce_System.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Emergency_Responce_System.Controllers
{
	[Authorize] // user must be logged in
	public class IncidentsController : Controller
	{
		private readonly AppDbContext _context;

		public IncidentsController(AppDbContext context)
		{
			_context = context;
		}

		// GET: Incidents
		// All roles can view incidents
		[Authorize(Roles = "Admin,Citizen")]
		public async Task<IActionResult> Index()
		{
			if (User.IsInRole("Admin"))
			{
				// Admin sees ALL incidents
				return View(await _context.Incidents
					.Include(i => i.Updates)
					.Include(i => i.Responders)
					.ToListAsync());
			}
			else
			{
				// Citizen sees ONLY their incidents
				var userId = User.Identity?.Name;

				return View(await _context.Incidents
					.Where(i => i.UserId == userId)
					.Include(i => i.Updates)
					.Include(i => i.Responders)
					.ToListAsync());
			}
		}

		// GET: Incidents/Details/5
		// All roles can view details
		[Authorize(Roles = "Admin,Citizen")]
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) return NotFound();

			var incident = await _context.Incidents
				.Include(i => i.Updates)
				.Include(i => i.Responders)
				.FirstOrDefaultAsync(m => m.IncidentID == id);

			if (incident == null) return NotFound();

			// If NOT admin, only allow owner
			if (!User.IsInRole("Admin") && incident.UserId != User.Identity?.Name)
			{
				return Forbid();
			}

			return View(incident);
		}

		// GET: Incidents/Create
		// Citizen creates incidents
		[Authorize(Roles = "Citizen")]
		public IActionResult Create()
		{
			return View();
		}

		// POST: Incidents/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Citizen")]
		public async Task<IActionResult> Create([Bind("IncidentID,Title,Description,Location")] Incidents incidents)
		{
			if (ModelState.IsValid)
			{
				
				incidents.UserId = User.Identity?.Name;

				_context.Add(incidents);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(incidents);
		}

		// GET: Incidents/Edit/5
		// Admin updates incidents
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) return NotFound();

			var incidents = await _context.Incidents.FindAsync(id);
			if (incidents == null) return NotFound();

			return View(incidents);
		}

		// POST: Incidents/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int id, [Bind("IncidentID,Title,Description,Location")] Incidents incidents)
		{
			if (id != incidents.IncidentID) return NotFound();

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
						return NotFound();
					else
						throw;
				}
				return RedirectToAction(nameof(Index));
			}
			return View(incidents);
		}

		// GET: Incidents/Delete/5
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();

			var incidents = await _context.Incidents
				.FirstOrDefaultAsync(m => m.IncidentID == id);

			if (incidents == null) return NotFound();

			return View(incidents);
		}

		// POST: Incidents/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
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