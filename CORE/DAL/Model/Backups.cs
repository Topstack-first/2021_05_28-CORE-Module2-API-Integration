using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.DAL.Model
{
    public class Backups
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        [Required]
        public DateTime Backup_Datetime { get; set; }
       
        public string Profile_Type { get; set; }

        public string FileSize { get; set; }
        
        public string Profile_Title { get; set; }

        [ForeignKey("Schedules")]
        public int? ScheduleID { get; set; }
        public Schedules Schedules { get; set; }
    }
}
