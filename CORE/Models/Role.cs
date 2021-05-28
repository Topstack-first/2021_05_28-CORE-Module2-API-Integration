using System;
using System.Collections.Generic;

#nullable disable

namespace CORE.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int? RolePermission { get; set; }
        public byte[] CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
