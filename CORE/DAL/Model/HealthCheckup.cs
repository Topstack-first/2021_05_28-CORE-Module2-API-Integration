using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.DAL.Model
{
    public class HealthCheckup
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Document_title { get; set; }
        [Required]
        public string Department_name { get; set; }
        [Required]
        public string Document_path { get; set; }
        [Required]
        public string Document_status { get; set; }
    }
}
