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
    public class PortalSettingAPI : ControllerBase
    {
        private IPortalSetting _bl;
        public PortalSettingAPI(IPortalSetting bl)
        {
            _bl = bl;
        }
        [HttpPost]
        public async Task<IActionResult> Save([FromForm] SavePortalSettingVM model)
        {
            try
            {
                if (model.CompanyLogo == null || model.LoginPageBackground == null || model.PortalLogo == null)
                {
                    return BadRequest("Required Images are Missing!!!");
                }
                var _c = await _bl.SavePortalSetting(model);
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
                var _c = await _bl.GetPortalSettings();
                return Ok(_c);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
