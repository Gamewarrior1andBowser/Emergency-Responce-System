using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Emergency_Responce_System.DAL;
using Emergency_Responce_System.Models;

namespace Emergency_Responce_System.Controllers
{
	public class RespondersController : Controller
	{
		private readonly AppDbContext _context;

		public RespondersController(AppDbContext context)
		{
			_context = context;
		}

		// GET: Responders
		public async Task<IActionResult> Index()
		{
			var responders = await _context.Responders
				.Include(r => r.Incident)
				.ToListAsync();

			return View(responders);
		}

		// GET: Responders/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) return NotFound();

			var responder = await _context.Responders
				.Include(r => r.Incident)
				.FirstOrDefaultAsync(m => m.ResponderID == id);

			if (responder == null) return NotFound();

			return View(responder);
		}

		// ✅ FIXED CREATE (GET)
		public IActionResult Create(int incidentId)
		{
			var responder = new Responders
			{
				IncidentID = incidentId
			};

			return View(responder);
		}

		// ✅ FIXED CREATE (POST)
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ResponderID,Name,Occupation,IncidentID")] Responders responder)
		{
			if (ModelState.IsValid)
			{
				_context.Add(responder);
				await _context.SaveChangesAsync();

				// 🔥 redirect back to incident
				return RedirectToAction("Details", "Incidents", new { id = responder.IncidentID });
			}

			return View(responder);
		}

		// GET: Responders/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) return NotFound();

			var responder = await _context.Responders.FindAsync(id);
			if (responder == null) return NotFound();

			return View(responder);
		}

		// POST: Responders/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ResponderID,Name,Occupation,IncidentID")] Responders responder)
		{
			if (id != responder.ResponderID) return NotFound();

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(responder);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!RespondersExists(responder.ResponderID))
						return NotFound();
					else
						throw;
				}

				return RedirectToAction("Details", "Incidents", new { id = responder.IncidentID });
			}

			return View(responder);
		}

		// GET: Responders/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();

			var responder = await _context.Responders
				.Include(r => r.Incident)
				.FirstOrDefaultAsync(m => m.ResponderID == id);

			if (responder == null) return NotFound();

			return View(responder);
		}

		// POST: Responders/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var responder = await _context.Responders.FindAsync(id);

			if (responder != null)
			{
				int incidentId = responder.IncidentID;

				_context.Responders.Remove(responder);
				await _context.SaveChangesAsync();

				// 🔥 go back to incident
				return RedirectToAction("Details", "Incidents", new { id = incidentId });
			}

			return RedirectToAction(nameof(Index));
		}

		private bool RespondersExists(int id)
		{
			return _context.Responders.Any(e => e.ResponderID == id);
		}
	}
}