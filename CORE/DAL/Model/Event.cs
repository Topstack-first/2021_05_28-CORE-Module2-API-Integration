using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.DAL.Model
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
       
        [StringLength(250)]
        public string IconUrl { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
