using System;
using System.Collections.Generic;

#nullable disable

namespace CORE.Models
{
    public partial class Permission
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public int? BitWiseValue { get; set; }
    }
}
