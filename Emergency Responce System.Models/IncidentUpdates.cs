using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emergency_Responce_System.Models
{
    public class IncidentUpdates
    {
        public DateTime Date{ get; set; }
        public string Status{ get; set; }
        public int UpdateID{ get; set; }
        public string Description{ get; set; }
        public Incidents IncidentID { get; set; }
    }
}
