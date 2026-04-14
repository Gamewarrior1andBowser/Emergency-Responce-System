using Emergency_Responce_System.Models;
using Emergency_Responce_System.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Emergency_Responce_System.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly AppDbContext _context;

		public HomeController(ILogger<HomeController> logger, AppDbContext context)
		{
			_logger = logger;
			_context = context;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		public async Task<IActionResult> Dashboard()
		{
			var incidents = await _context.Incidents
				.Include(i => i.Updates)
				.Include(i => i.Responders)
				.ToListAsync();

			return View(incidents);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}