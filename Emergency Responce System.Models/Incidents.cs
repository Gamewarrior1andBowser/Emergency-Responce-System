using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emergency_Responce_System.Models
{
    public class Incidents
    {
        public string Description{ get; set; }
        public string Location{ get; set; }
        public string Title{ get; set; }
        public int IncidentID { get; set; }
        //public Responders ResponderID;
    }
}
