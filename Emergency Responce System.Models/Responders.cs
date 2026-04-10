using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emergency_Responce_System.Models
{
    public class Responders
    {
        public Incidents IncidentID{ get; set; }
        public string Name{ get; set; }
        public int ResponderID{ get; set; }
        public string Occupation { get; set; }
    }
}
