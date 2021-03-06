using BeicipFranLabERP.BL.Repository.Administrator;
using BeicipFranLabERP.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectTrackingAPI : ControllerBase
    {
         private IProjectTracker _bl;
        public ProjectTrackingAPI(IProjectTracker bl)
        {
            _bl = bl;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int Id = 0)
        {
            try
            {
                var _c = await _bl.GetProjectTracking(Id);
                return Ok(_c);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
    }
}

