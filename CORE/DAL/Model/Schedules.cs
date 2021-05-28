using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.DAL.Model
{
    public class Schedules
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        [Required]
        public string Schedule_Name { get; set; }
        public string Profile_Type { get; set; }
        [Required]
        public string Interval { get; set; }
        [Required]
        public DateTime RunTime { get; set; }
        [Required]
        public string Status { get; set; }

        public List<Backups> Backups { get; set; }
        [ForeignKey("Destination")]
        public int DestinationID { get; set; }
        public Destination Destination { get; set; }
    }
}
