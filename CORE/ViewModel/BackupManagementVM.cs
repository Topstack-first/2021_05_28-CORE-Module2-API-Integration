using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.ViewModel
{
    public class SaveNewBackupVM
    {
        /// <summary>
        /// 2 types 'full' for full backup and 'log' for logs  only backup
        /// </summary>
        public string BackupType { get; set; }
        //public string ProfileTitle { get; set; }
        //public string ProfileType { get; set; }
        /// <summary>
        /// it is destination folder name like E://shaibaa//
        /// </summary>
        public string DestinationName { get; set; }

        //public string DestinationType { get; set; }

        //public string ServerAddress { get; set; }

        //public string Username { get; set; }

        //public string Password { get; set; }

        //public string RemotePath { get; set; }

        //public string MigrationURL { get; set; }

        //public int ArchiveLimit { get; set; }
    }


   
    public class SaveBackupSchedule {

        public string Schedule_Name { get; set; }
        /// <summary>
        /// has 2 options which  is 'full' for the database backup and 'log' is for logs only backup
        /// </summary>
        [Required]
        public string Backup_Type { get; set; }
        /// <summary>
        /// Interval can be 'Once daily' or 'Weekly'(Required)
        /// </summary>
        [Required]
        public string Interval { get; set; }// Interval can be 'Once daily' or 'Weekly'

        //public DateTime RunTime { get; set; }

        //public string Status { get; set; }
        /// <summary>
        /// is the destination folder location where backup is to be stored e.g 'E://NewFolder//' 
        /// </summary>
        [Required]
        public string DestinationName { get; set; }
        //public string DestinationType { get; set; }

    }




}
