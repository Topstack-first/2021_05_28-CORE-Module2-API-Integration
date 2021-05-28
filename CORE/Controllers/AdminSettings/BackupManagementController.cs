using BeicipFranLabERP.BL.Repository.AdminSetting;
using BeicipFranLabERP.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.Controllers.AdminSettings
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackupManagementController : ControllerBase
    {


        private IBackupManagement _bl;
        public BackupManagementController(IBackupManagement bl)
        {
            _bl = bl;
        }


        [HttpPost]
        public async Task<IActionResult> Save(SaveNewBackupVM model)
        {
            try
            {
                var _c = await _bl.SaveNewBackup(model);
                return Ok(_c);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("RestoreBackup")]
        public async Task<IActionResult> RestoreBackup(SaveNewBackupVM model)
        {
            try
            {
                var _c = await _bl.RestoreTheBackup(model);
                return Ok(_c);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Save The Backup Schedule and according to this data backup is automatically generated in the destination folder
        /// </summary>
        /// <param name="model">Parameter Name Interval can be 'Once daily' or 'Weekly'(Required)
        /// ----
        /// Parameter Name Destination is the destination folder location where backup is to be stored e.g 'E://NewFolder//' (Required)
        /// ----
        /// Parameter Name Backup_Type has 2 options which  is 'full' for the database backup and 'log' is for logs only backup
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveBackupSchedule")]
        
        public async Task<IActionResult> SaveBackupSchedule(SaveBackupSchedule model)
        {
            try
            {
                var _c = await _bl.SaveBackupSchedule(model);
                return Ok(_c);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
