using CORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CORE.Models
{
    public class UserWithToken : User
    {
        
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public UserWithToken(User user)
        {
            this.UserId = user.UserId;
            this.UserName = user.UserName;
            this.UserEmail = user.UserEmail;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.DepartmentId = user.DepartmentId;
            this.Department = user.Department;
            this.RoleId = user.RoleId;

        }
    }
}