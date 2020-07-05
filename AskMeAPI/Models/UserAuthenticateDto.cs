using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMeAPI.Models
{
    public class UserAuthenticateDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
