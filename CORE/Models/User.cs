using System;
using System.Collections.Generic;

#nullable disable

namespace CORE.Models
{
    public partial class User
    {
        public User()
        {
            RefreshTokens = new HashSet<RefreshToken>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int? UserPost { get; set; }
        public bool? UserAccountStatus { get; set; }
        public string UserApprovalStatus { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string UserPassword { get; set; }
        public string UserProfilePic { get; set; }
        public string UserBiography { get; set; }
        public byte[] CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public int? RoleId { get; set; }
        public int? DepartmentId { get; set; }
        public int? DocumentAttached { get; set; }
        public bool? DeleteStatus { get; set; }

        public virtual Department Department { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
