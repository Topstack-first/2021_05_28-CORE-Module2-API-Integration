using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.DAL.Model
{
    
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string RoleName { get; set; }
        public string IsActive { get; set; }
        public string ProfileImagePath { get; set; }

    }
}
