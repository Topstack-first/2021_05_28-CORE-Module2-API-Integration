using BeicipFranLabERP.BL.Repository.Administrator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.Controllers.Maintenance.HealthCheckUp
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthSpecialCharacterAPI : ControllerBase
    {
        private IHealth _bl;
        public HealthSpecialCharacterAPI(IHealth bl)
        {
            _bl = bl;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int Id = 0)
        {
            try
            {

                var c = await _bl.GetSpecialCharacters(Id);
                return Ok(c);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
