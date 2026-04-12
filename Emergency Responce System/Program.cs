using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Emergency_Response_System.DAL;

namespace Emergency_Response_System
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add MVC
			builder.Services.AddControllersWithViews();

			// ✅ Register YOUR DbContext
			builder.Services.AddDbContext<AppDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			// ✅ Identity setup (users + roles)
			builder.Services.AddDefaultIdentity<IdentityUser>(options =>
				options.SignIn.RequireConfirmedAccount = false)
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<AppDbContext>();

			var app = builder.Build();

			// ✅ Seed roles + admin
			SeedRolesAndAdminUserAsync(app.Services).GetAwaiter().GetResult();

			// Middleware
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();
			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			// Routing
			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.MapRazorPages();

			app.Run();
		}

		// ✅ Seed roles + admin user
		static async Task SeedRolesAndAdminUserAsync(IServiceProvider serviceProvider)
		{
			using (var scope = serviceProvider.CreateScope())
			{
				var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

				// Roles
				string[] roles = { "Admin", "Dispatcher", "Citizen" };

				foreach (var role in roles)
				{
					if (!await roleManager.RoleExistsAsync(role))
					{
						await roleManager.CreateAsync(new IdentityRole(role));
					}
				}

				// Admin user
				var adminEmail = "admin@system.com";

				var adminUser = await userManager.FindByEmailAsync(adminEmail);

				if (adminUser == null)
				{
					var newAdmin = new IdentityUser
					{
						UserName = adminEmail,
						Email = adminEmail,
						EmailConfirmed = true
					};

					await userManager.CreateAsync(newAdmin, "Admin123!");
					await userManager.AddToRoleAsync(newAdmin, "Admin");
				}
			}
		}
	}
}