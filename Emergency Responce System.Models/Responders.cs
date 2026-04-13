using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emergency_Responce_System.Models
{
	public class Responders
	{
		[Key]
		public int ResponderID { get; set; }

		public string Name { get; set; }
		public string Occupation { get; set; }

		// FK
		public int IncidentID { get; set; }

		// Navigation
		public Incidents Incident { get; set; }
	}
}
