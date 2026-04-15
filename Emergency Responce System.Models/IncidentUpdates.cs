using System;
using System.ComponentModel.DataAnnotations;

namespace Emergency_Responce_System.Models
{
	public class IncidentUpdates
	{
		[Key]
		public int UpdateID { get; set; }

		public DateTime? Date { get; set; }
		public string Status { get; set; }
		public string Description { get; set; }

		// FK

		public int IncidentID { get; set; }

		// Navigation
		public Incidents Incident { get; set; }
	}
}
