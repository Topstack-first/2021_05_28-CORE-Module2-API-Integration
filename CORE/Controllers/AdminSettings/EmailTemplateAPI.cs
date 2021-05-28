using BeicipFranLabERP.BL.Repository.AdminSetting;
using BeicipFranLabERP.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.Controllers.AdminSettings
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailTemplateAPI : ControllerBase
    {
        private IEmailTemplate _bl;
        public EmailTemplateAPI(IEmailTemplate bl)
        {
            _bl = bl;
        }



        [HttpPost]
        public async Task<IActionResult> Save(EmailTeplateElementVM model)
        {
            try
            {
                var _c = await _bl.SaveEmailTemplate(model);
                return Ok(_c);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var obj = await _bl.GetEmailTemplate();
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





        //[HttpGet]
        //[Route("backup")]
        //public IActionResult backup()
        //{
        //    try
        //    {
        //        ServerConnection serverConnection = new ServerConnection("DESKTOP-HRIP034\\CHALLENGE");
        //        Server server = new Server(serverConnection);
        //        Database database = server.Databases["p1"];
        //        Backup backup = new Backup();
        //        backup.Action = BackupActionType.Database;
        //        backup.BackupSetDescription = "AdventureWorks - full backup";
        //        backup.BackupSetName = "AdventureWorks backup";
        //        backup.Database = "p1";

        //        BackupDeviceItem deviceItem = new BackupDeviceItem("p1.bak", DeviceType.File);
        //        backup.Devices.Add(deviceItem);
        //        backup.Incremental = false;
        //        backup.LogTruncation = BackupTruncateLogType.Truncate;
        //        backup.SqlBackup(server);





        //        SqlConnection sqlconn = new SqlConnection("Data Source=DESKTOP-HRIP034\\CHALLENGE;Initial Catalog=p1;Integrated Security=True;Pooling=False");
        //        SqlCommand sqlcmd = new SqlCommand();
        //        SqlDataAdapter da = new SqlDataAdapter();

        //        // Backup destibation
        //        string backupDestination = "C:\\New folder";
        //        // check if backup folder exist, otherwise create it.
        //        if (!System.IO.Directory.Exists(backupDestination))
        //        {
        //            System.IO.Directory.CreateDirectory("D:\\SQLBackUpFolder");
        //        }
        //        try
        //        {
        //            sqlconn.Open();
        //            sqlcmd = new SqlCommand("backup database p1 to disk='" + backupDestination+ "\\" + ".Bak'", sqlconn);

        //            sqlcmd.ExecuteNonQuery();
        //            //Close connection
        //            sqlconn.Close();
        //        }
        //        catch (Exception ex)
        //        {

        //        }












        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}





    }
}
