using BeicipFranLabERP.BL.Repository.Administrator;
using BeicipFranLabERP.DAL.Model;
using BeicipFranLabERP.ViewModel.Location;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using CORE.Models;

namespace BeicipFranLabERP.DAL.Services.Administrator
{
   
    public class LocationDAL: ILocation
    {
        private COREContext _context;
        public LocationDAL(COREContext appDbContext)
        {
            _context = appDbContext;
        }

        public string GetCurrentUserId()
        {
            return "Test123";
        }
        public async Task<string> SaveLocation(SaveLocationVM loc)
        {
            string res = "";
            if (loc.LocationId > 0)
            {
                var _c = await _context.Locations.Where(x => x.LocationId == loc.LocationId).FirstOrDefaultAsync();
                _c.Name = loc.Name;
                _c.IconUrl = loc.IconUrl;
              _c.CreatedBy = GetCurrentUserId();
                _c.CreatedDate = Utility.Now();
                _context.Locations.Update(_c);
                await _context.SaveChangesAsync();
                res = "Sucessfully Updated";
            }
            else
            {
                var _c = new Location();
                _c.Name = loc.Name;
                _c.IconUrl = loc.IconUrl;
                _c.CreatedBy = GetCurrentUserId();
                _c.CreatedDate = Utility.Now();
                _context.Locations.Add(_c);
                await _context.SaveChangesAsync();
                res = "Sucessfully Added";
            }
            return res;
        }
        public async Task<string> DeleteLocation(int id)
        {
            string res = "";
            var _c = await _context.Locations.Where(x => x.LocationId == id).FirstOrDefaultAsync();
            if (_c != null)
            {
                _context.Locations.Remove(_c);
                await _context.SaveChangesAsync();
                res = "Sucessfully Deleted";
            }
            else
            {
                res = "Invalid Location";
            }
            return res;
        }
        public async Task<List<GetLocationVM>> GetLocation(int id)
        {
            var _c = new List<Location>();
            if (id > 0)
                _c = await _context.Locations.Where(x => x.LocationId == id).ToListAsync();
            else
                _c = await _context.Locations.ToListAsync();


            var transForm = _c.AsEnumerable().Select(x => new GetLocationVM
            {

                IconUrl = x.IconUrl,
                Name = x.Name,
                LocationId = x.LocationId

            }).ToList();
            return transForm;
        }

    }
}
