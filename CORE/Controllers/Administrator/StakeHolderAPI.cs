using BeicipFranLabERP.BL.Repository.Administrator;
using BeicipFranLabERP.ViewModel.StakeHolder;
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
    public class StakeHolderAPI : ControllerBase
    {
        private IStakeHolder _bl;
        public StakeHolderAPI(IStakeHolder bl)
        {
            _bl = bl;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int Id=0)
        {
            try
            {
                var _c = await _bl.GetStakeHolder(Id);
                return Ok(_c);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Save(SaveStakeHolderVM stake)
        {
            try
            {
                var _c = await _bl.SaveStakeHolder(stake);
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
                var _c =await  _bl.DeleteStakeHolder(Id);
                return Ok(_c);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
    }
}
