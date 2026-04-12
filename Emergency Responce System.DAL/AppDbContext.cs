using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Emergency_Responce_System.Models;


namespace Emergency_Responce_System.DAL
{
	public class AppDbContext : IdentityDbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options)
		{
		}

		public DbSet<Incidents> Incidents { get; set; }
		public DbSet<IncidentUpdates> IncidentUpdates { get; set; }
		public DbSet<Responders> Responders { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<IncidentUpdates>()
				.HasOne(u => u.Incident)
				.WithMany(i => i.Updates)
				.HasForeignKey(u => u.IncidentID);

			modelBuilder.Entity<Responders>()
				.HasOne(r => r.Incident)
				.WithMany(i => i.Responders)
				.HasForeignKey(r => r.IncidentID);
		}
	}
}