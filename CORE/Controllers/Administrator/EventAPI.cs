using BeicipFranLabERP.BL.Repository.Administrator;
using BeicipFranLabERP.ViewModel.Event;
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
    public class EventAPI : ControllerBase
    {
        private IEvent  _bl;
        public EventAPI(IEvent bl)
        {
            _bl = bl;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int Id=0)
        {
            try
            {
                var _c = await _bl.GetEvent(Id);
                return Ok(_c);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Save(SaveEventVM eve)
        {
            try
            {
                var _c = await _bl.SaveEvent(eve);
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
                var _c = await _bl.DeleteEvent(Id);
                return Ok(_c);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
