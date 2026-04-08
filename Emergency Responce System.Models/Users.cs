using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emergency_Responce_System.Models
{
    public class Users
    {
        public string Name;
        public int UserID;
        public string Email;
        public int Phone;
        private readonly string Password;
        public string Role;
    }
}
