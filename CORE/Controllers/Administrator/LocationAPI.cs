using BeicipFranLabERP.BL.Repository.Administrator;
using BeicipFranLabERP.ViewModel.Location;
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
    public class LocationAPI : ControllerBase
    {
        private ILocation _bl;
        public LocationAPI(ILocation bl)
        {
            _bl = bl;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int Id=0)
        {
            try
            {
                var _c = await _bl.GetLocation(Id);
                return Ok(_c);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Save(SaveLocationVM loc)
        {
            try
            {
                var _c = await _bl.SaveLocation(loc);
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
                var _c = await _bl.DeleteLocation(Id);
                return Ok(_c);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
