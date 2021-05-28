using BeicipFranLabERP.BL.Repository.Administrator;
using BeicipFranLabERP.ViewModel.Well;
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
    public class WellApi : ControllerBase
    {
        private IWell _bl;
        public WellApi(IWell bl)
        {
            _bl = bl;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int Id = 0)
        {
            try
            {
                var _c = await _bl.GetWell(Id);
                return Ok(_c);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Save(SaveWellVM stake)
        {
            try
            {
                var _c = await _bl.SaveWell(stake);
                return Ok(_c);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                var _c = await _bl.DeleteWell(Id);
                return Ok(_c);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
