using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Emergency_Responce_System.Models;
using Emergency_Response_System.DAL;
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
