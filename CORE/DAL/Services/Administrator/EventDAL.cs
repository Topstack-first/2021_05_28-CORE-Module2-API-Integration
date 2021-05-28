using BeicipFranLabERP.BL.Repository.Administrator;
using BeicipFranLabERP.DAL.Model;
using BeicipFranLabERP.ViewModel.Event;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using CORE.Models;

namespace BeicipFranLabERP.DAL.Services.Administrator
{
   
    public class EventDAL:IEvent
    {
        private COREContext _context;
        public EventDAL(COREContext appDbContext)
        {
            _context = appDbContext;
        }

        public string GetCurrentUserId()
        {
            return "Test123";
        }
        public async Task<string> SaveEvent(SaveEventVM eve)
        {
            string res = "";
            if (eve.EventId > 0)
            {
                var _c = await _context.Events.Where(x => x.EventId == eve.EventId).FirstOrDefaultAsync();
                _c.Name = eve.Name;
                _c.IconUrl = eve.IconUrl;
                _c.CreatedBy = GetCurrentUserId();
                _c.CreatedDate = Utility.Now();
                _context.Events.Update(_c);
                await _context.SaveChangesAsync();
                res = "Sucessfully Updated";
            }
            else
            {
                var _c = new Event();
                _c.Name = eve.Name;
                _c.IconUrl = eve.IconUrl;
                _c.CreatedBy = GetCurrentUserId();
                _c.CreatedDate = Utility.Now();
                _context.Events.Add(_c);
                await _context.SaveChangesAsync();
                res = "Sucessfully Added";
            }
            return res;
        }
        public async Task<string> DeleteEvent(int id)
        {
            string res = "";
            var _c = await _context.Events.Where(x => x.EventId == id).FirstOrDefaultAsync();
            if (_c != null)
            {
                _context.Events.Remove(_c);
                await _context.SaveChangesAsync();
                res = "Sucessfully Deleted";
            }
            else
            {
                res = "Invalid Event";
            }
            return res;
        }
        public async Task<List<GetEventVM>> GetEvent(int id)
        {
            var _c = new List<Event>();
            if (id > 0)
                _c = await _context.Events.Where(x => x.EventId == id).ToListAsync();
            else
                _c = await _context.Events.ToListAsync();


            var transForm = _c.AsEnumerable().Select(x => new GetEventVM
            {
                
                IconUrl = x.IconUrl,
                Name = x.Name,
                EventId = x.EventId

            }).ToList();
            return transForm;
        }

    }
}
