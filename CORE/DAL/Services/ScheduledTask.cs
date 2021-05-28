using BeicipFranLabERP.BackgroundServices;
using BeicipFranLabERP.BL.Repository.AdminSetting;
using BeicipFranLabERP.DAL.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CORE.Models;

namespace BeicipFranLabERP.DAL.Services
{
    public class ScheduledTask : ScheduledProcessor
    {
        private readonly IServiceScopeFactory scopeFactory;

        public IConfiguration Configuration;
        public ScheduledTask(IServiceScopeFactory scopeFactory,  IConfiguration _Configuration) : base(scopeFactory)
        {
            this.scopeFactory = scopeFactory;
            Configuration = _Configuration;
        }

        protected override string Schedule => "*/1 * * * *"; // every 1 min 

        public override Task ProcessInScope(IServiceProvider scopeServiceProvider)
      {
            using (var scope = scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<COREContext>();
                var backup_schedules = _context.Schedules.Include(o => o.Backups).Include(s => s.Destination).Where(a => a.Status == "active").ToList();
                foreach (var backup_sch in backup_schedules)
                {
                    var last_backup = backup_sch.Backups.OrderByDescending(d => d.Id).FirstOrDefault();
                    var scheduled_interval = backup_sch.Interval;
                    if (scheduled_interval == "Once daily")
                    {
                        if (last_backup == null)
                        {
                            CreateScheduledBackup(backup_sch);

                        }
                        else if (last_backup.Backup_Datetime.Date == DateTime.Now.Date)
                        {
                            break;
                        }
                        else
                        {
                            CreateScheduledBackup(backup_sch);
                        }
                    }
                    else if (scheduled_interval == "Weekly")
                    {
                        var last_week_date = DateTime.Now.AddDays(-7).Date;
                        if (last_backup == null)
                        {
                            CreateScheduledBackup(backup_sch);

                        }
                        else if (last_backup.Backup_Datetime.Date > last_week_date)
                        {
                            break;
                        }
                        else
                        {
                            CreateScheduledBackup(backup_sch);
                        }
                    }
                }
            }



            return Task.CompletedTask;
        }

        public void CreateScheduledBackup(Schedules model)
        {

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

            string user = builder.UserID;
            string pass = builder.Password;
            string server_name = builder.DataSource;
            var db_name = builder.InitialCatalog;
            ServerConnection serverConnection = new ServerConnection(server_name, user, pass);
            Server server = new Server(serverConnection);
            Database database = server.Databases[db_name];
            Backup bkpDBFull = new Backup();
            if (model.Profile_Type == "full")
            {
                bkpDBFull.Action = BackupActionType.Database;
            }
            else if (model.Profile_Type == "log")
            {
                bkpDBFull.Action = BackupActionType.Log;
            }
            bkpDBFull.Complete += Backup_Completed;
            /* Specify the name of the database to back up */
            bkpDBFull.Database = db_name;
            string path = model.Destination.DestinationName;
            bkpDBFull.Devices.AddDevice(path + db_name + "_" + DateTime.Now.ToString("dd-MM-yyyy-hhmmss") + ".bak", DeviceType.File);
            bkpDBFull.BackupSetName = db_name;
            bkpDBFull.BackupSetDescription = db_name+" database  backup by schdule";
            //bkpDBFull.ExpirationDate = DateTime.Today.AddDays(10);
            bkpDBFull.Initialize = false;
            bkpDBFull.SqlBackup(server);
            SaveBackupRecord(model);

        }




        private static void Backup_Completed(object sender, ServerMessageEventArgs args)
        {
           
        }

        public  void SaveBackupRecord(Schedules model)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<COREContext>();

                var backup_obj = new Backups();
                backup_obj.Backup_Datetime = DateTime.Now;
                backup_obj.FileSize = "";
                var schedule = _context.Schedules.Include(r => r.Backups).FirstOrDefault(a => a.Id == model.Id);
                schedule.Backups.Add(backup_obj);
                _context.SaveChanges();
            }
        }


    }
}
