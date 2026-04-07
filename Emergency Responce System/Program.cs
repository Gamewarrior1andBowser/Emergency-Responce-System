using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Emergency_Responce_System.BLL;
using Emergency_Responce_System.DAL;

namespace Emergency_Response_System {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register DbContext with connection string
            builder.Services.AddDbContext<RaceLeagueContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>() // Add support for roles
                .AddEntityFrameworkStores<RaceLeagueContext>();

            // Register DAL and BLL services
            builder.Services.AddScoped<TournamentRepository>();
            builder.Services.AddScoped<TournamentService>();

            var app = builder.Build();


            // Seed roles and admin user here
            SeedRolesAndAdminUserAsync(app.Services).GetAwaiter().GetResult();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment()) {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }

        static async Task SeedRolesAndAdminUserAsync(IServiceProvider serviceProvider) {
            using (IServiceScope scope = serviceProvider.CreateScope()) {
                RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                UserManager<IdentityUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                // Define roles
                string[] roles = { "Admin", "User" };
                foreach (string role in roles) {
                    if (!await roleManager.RoleExistsAsync(role)) {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Create an admin user
                IdentityUser adminUser = new IdentityUser {
                    UserName = "admin@RaceLeague.com",
                    Email = "admin@RaceLeague.com",
                    EmailConfirmed = true
                };
                if (await userManager.FindByEmailAsync(adminUser.Email) == null) {
                    await userManager.CreateAsync(adminUser, "AdminPassword123!");
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
