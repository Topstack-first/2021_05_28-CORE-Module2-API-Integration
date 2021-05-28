using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CORE.Models
{
    public class UpdateUserPasswordbyId
    {
        public int UserID { get; set; }
        public string NewPassword { get; set; }
    }
}
