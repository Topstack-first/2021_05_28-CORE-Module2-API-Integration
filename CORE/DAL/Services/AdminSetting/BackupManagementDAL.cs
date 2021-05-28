using BeicipFranLabERP.BL.Repository.AdminSetting;
using BeicipFranLabERP.DAL.Model;
using BeicipFranLabERP.ViewModel;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CORE.Models;

namespace BeicipFranLabERP.DAL.Services.AdminSetting
{
    public class BackupManagementDAL : IBackupManagement
    {
        public IConfiguration Configuration;

        private COREContext _context;
        public BackupManagementDAL( IConfiguration config, COREContext context)
        {
            Configuration = config;
            _context = context;
        }

        public async Task<string> RestoreTheBackup(SaveNewBackupVM model)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            string user = builder.UserID;
            string pass = builder.Password;
            string server_name = builder.DataSource;
            ServerConnection serverConnection = new ServerConnection(server_name, user, pass);
            Server server = new Server(serverConnection);
            Database database = server.Databases["BecipERP"];
            Restore restoreDB = new Restore();
            restoreDB.Database = "BecipERP";
            if (model.BackupType == "full")
            {
                restoreDB.Action = RestoreActionType.Database;
            }
            else if (model.BackupType == "log")
            {
                restoreDB.Action = RestoreActionType.Log;
            }
            restoreDB.Devices.AddDevice(model.DestinationName, DeviceType.File);

            restoreDB.ReplaceDatabase = true;

            restoreDB.NoRecovery = false;

            /* Wiring up events for progress monitoring */
            restoreDB.PercentComplete += CompletionStatusInPercent;
            restoreDB.Complete += Restore_Completed;
            restoreDB.SqlRestoreAsync(server);
            return  "";
        }

        public async Task<string> SaveNewBackup(SaveNewBackupVM model)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

            string user = builder.UserID;
            string pass = builder.Password;
            string server_name = builder.DataSource;
            var db_name = builder.InitialCatalog;
            ServerConnection serverConnection = new ServerConnection(server_name, user,pass);
            Server server = new Server(serverConnection);
            Database database = server.Databases[db_name];
            Backup bkpDBFull = new Backup();
            if (model.BackupType == "full")
            {
                bkpDBFull.Action = BackupActionType.Database;
            }
            else if(model.BackupType == "log")
            {
                bkpDBFull.Action = BackupActionType.Log;
            }
            /* Specify the name of the database to back up */
            bkpDBFull.Database = db_name;
            string path = model.DestinationName;
            bkpDBFull.Devices.AddDevice(path+ db_name + "_"+DateTime.Now.ToString("dd-MM-yyyy-hhmmss")+".bak", DeviceType.File);
            bkpDBFull.BackupSetName = db_name+" Backup";
            bkpDBFull.BackupSetDescription = db_name+" Backup";
            //bkpDBFull.ExpirationDate = DateTime.Today.AddDays(10);
            bkpDBFull.Initialize = false;
              bkpDBFull.SqlBackupAsync(server);





            var obj = new Backups();
            obj.Backup_Datetime = DateTime.Now;
            obj.Profile_Title ="";
            obj.Profile_Type = "";
            obj.FileSize = "unknown";
            _context.Backups.Add(obj);
           await _context.SaveChangesAsync();
            return "Backup Successfully";

        }
        private static void CompletionStatusInPercent(object sender, PercentCompleteEventArgs args)
        {
            //Console.Clear();
            Console.WriteLine("Percent completed: {0}%.", args.Percent);
        }
        private static void Backup_Completed(object sender, ServerMessageEventArgs args)
        {
            Console.WriteLine("Backup completed.");
            Console.WriteLine(args.Error.Message);
        }
        private static void Restore_Completed(object sender, ServerMessageEventArgs args)
        {
            Console.WriteLine("Restore completed.");
            Console.WriteLine(args.Error.Message);
        }

        public async Task<string> SaveBackupSchedule(SaveBackupSchedule model)
        {


            var obj = new Schedules() {
             Destination=new Destination()};
            obj.Interval = model.Interval;
            obj.Schedule_Name = model.Schedule_Name;
            obj.RunTime =DateTime.Now;
            obj.Profile_Type = model.Backup_Type;
            obj.Status = "active";
            var destination_obj = new Destination();
            destination_obj.DestinationName = model.DestinationName;
            destination_obj.DestinationType = "";
            destination_obj.ArchiveLimit = 0;
            obj.Destination = destination_obj;

             await _context.Schedules.AddAsync(obj);

             await _context.SaveChangesAsync();

            return "Saved Successfully";
        }
    }
}
