using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CORE.Models
{
    public partial class Department
    {
        public Department()
        {
            Users = new HashSet<User>();
        }

        // public string DepartmentName { get; set; }

        public virtual ICollection<User> Users { get; set; }
        
        [Key]
        public int DepartmentId { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
       
        [StringLength(250)]
        public string Alias { get; set; }
        [StringLength(250)]
        public string IconUrl { get; set; }
        
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
