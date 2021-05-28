using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.DAL.Models
{
	[Table("User")]
	public class User
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

	}
}
