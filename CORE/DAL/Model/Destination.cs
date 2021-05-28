using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.DAL.Model
{
    public class Destination
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string DestinationName { get; set; }
        
        public string DestinationType { get; set; }
    
        public string ServerAddress { get; set; }
       
        public string Username { get; set; }
    
        public string Password { get; set; }
       
        public string RemotePath { get; set; }
        
        public string MigrationURL { get; set; }
        
        public int ArchiveLimit { get; set; }
        //[ForeignKey("Backups")]
        //public int Backup_ID { get; set; }
        //public Backups Backups { get; set; }
    }
}
