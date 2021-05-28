using BeicipFranLabERP.BL.Repository.Administrator;
using BeicipFranLabERP.DAL.Model;
using BeicipFranLabERP.ViewModel.StakeHolder;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using CORE.Models;

namespace BeicipFranLabERP.DAL.Services.Administrator
{
   
    public class StakeHolderDAL: IStakeHolder
    {
        private COREContext _context;
        public StakeHolderDAL(COREContext appDbContext)
        {
            _context = appDbContext;
        }

        public string GetCurrentUserId()
        {
            return "Test123";
        }
        public async Task<string> SaveStakeHolder(SaveStakeHolderVM skh)
        {
            string res = "";
            if (skh.StakeHolderId > 0)
            {
                var _c = await _context.StakeHolders.Where(x => x.StakeHolderId == skh.StakeHolderId).FirstOrDefaultAsync();
                _c.Name = skh.Name;
                _c.IconUrl = skh.IconUrl;
                _c.Alias = skh.IconUrl;
                _c.CreatedBy = GetCurrentUserId();
                _c.CreatedDate = Utility.Now();
                _context.StakeHolders.Update(_c);
                await _context.SaveChangesAsync();
                res = "Sucessfully Updated";
            }
            else
            {
                var _c = new StakeHolder();
                _c.Name = skh.Name;
                _c.IconUrl = skh.IconUrl;
                _c.Alias = skh.IconUrl;
                _c.CreatedBy = GetCurrentUserId();
                _c.CreatedDate = Utility.Now();
                _context.StakeHolders.Add(_c);
                await _context.SaveChangesAsync();
                res = "Sucessfully Added";
            }
            return res;
        }
        public async Task<string> DeleteStakeHolder(int id)
        {
            string res = "";
            var _c = await _context.StakeHolders.Where(x => x.StakeHolderId == id).FirstOrDefaultAsync();
            if (_c != null)
            {
                _context.StakeHolders.Remove(_c);
                await _context.SaveChangesAsync();
                res = "Sucessfully Deleted";
            }
            else
            {
                res = "Invalid StakeHolder";
            }
            return res;
        }
        public async Task<List<GetStakeHolderVM>> GetStakeHolder(int id)
        {
            var _c = new List<StakeHolder>();
            if (id > 0)
                _c = await _context.StakeHolders.Where(x => x.StakeHolderId == id).ToListAsync();
            else
                _c = await _context.StakeHolders.ToListAsync();


            var transForm = _c.AsEnumerable().Select(x => new GetStakeHolderVM
            {

                IconUrl = x.IconUrl,
                Name = x.Name,
                 Alias=x.Alias,
                StakeHolderId = x.StakeHolderId

            }).ToList();
            return transForm;
        }

    }
}
