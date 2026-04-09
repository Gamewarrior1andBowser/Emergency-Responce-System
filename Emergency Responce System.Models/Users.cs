using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emergency_Responce_System.Models
{
    public class Users
    {
        public string Name { get; set; }
        public int UserID { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        private readonly string Password;
        //private readonly string Password { get; set; }   Not sure of this one
        public string Role { get; set; }
    }
}
