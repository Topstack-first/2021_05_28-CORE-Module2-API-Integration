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
    public class CoreSettingAPI : ControllerBase
    {
        private ICoreSetting _bl;
        public CoreSettingAPI(ICoreSetting bl)
        {
            _bl = bl;
        }
        [HttpPost]
        public async Task<IActionResult> Save(CoreSettingVM model)
        {
            try
            {
                var _c = await _bl.SaveCoreSetting(model);
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
                var obj = await _bl.GetCoreSettings();
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}
