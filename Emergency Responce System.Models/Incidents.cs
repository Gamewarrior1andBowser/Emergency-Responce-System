using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emergency_Responce_System.Models
{
	public class Incidents
	{
		[Key]
		public int IncidentID { get; set; }

		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? Location { get; set; }

		public string? UserId { get; set; }

		public ICollection<IncidentUpdates>? Updates { get; set; }
		public ICollection<Responders>? Responders { get; set; }
	}
}