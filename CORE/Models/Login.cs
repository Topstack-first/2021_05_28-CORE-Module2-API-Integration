using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CORE.Models
{
    public partial class Login
    {
        public int UserId { get; set; }
        public string UserNameEmail { get; set; }
        public string UserPassword { get; set; }
    }
}
