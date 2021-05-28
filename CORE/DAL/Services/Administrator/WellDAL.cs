using BeicipFranLabERP.BL.Repository.Administrator;
using BeicipFranLabERP.DAL.Model;
using BeicipFranLabERP.ViewModel.Well;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CORE.Models;

namespace BeicipFranLabERP.DAL.Services.Administrator
{
    public class WellDAL: IWell
    {
        private COREContext _context;
        public WellDAL(COREContext appDbContext)
        {
            _context = appDbContext;
        }

        public string GetCurrentUserId()
        {
            return "Test123";
        }
        public async Task<string> SaveWell(SaveWellVM well)
        {
            string res = "";

            if (well.WellId > 0)
            {
                var _c = await _context.Wells.FirstOrDefaultAsync(x => x.WellId == well.WellId);
                _c.Name = well.Name;
                _c.Alias = well.Alias;
                _c.IconUrl = well.IconUrl;
                _c.FieldName = well.FieldName;
                _c.BlockName = well.BlockName;
                _c.BasinName = well.BasinName;
                _c.Latitude = well.Latitude;
                _c.Longitude = well.Longitude;
                _c.XEasting = well.XEasting;
                _c.WellType = well.WellType;
                _c.YNorthing = well.YNorthing;
                _c.SpudDate = well.SpudDate;
                _c.CompletionDate = well.CompletionDate;
                _c.TD = well.TD;
                _c.MDDF = well.MDDF;
                _c.TVDDF = well.TVDDF;
                _c.UpdatedBy = GetCurrentUserId();
                _c.CreatedDate = Utility.Now();
                _context.Wells.Update(_c);
                await _context.SaveChangesAsync();
                res = "Sucessfully Updated";

            }
            else
            {
                var _c = new Well();


                _c.Name = well.Name;
                _c.Alias = well.Alias;
                _c.FieldName = well.FieldName;
                _c.BlockName = well.BlockName;
                _c.BasinName = well.BasinName;
                _c.Latitude = well.Latitude;
                _c.Longitude = well.Longitude;
                _c.IconUrl = well.IconUrl;
                _c.XEasting = well.XEasting;
                _c.WellType = well.WellType;
                _c.YNorthing = well.YNorthing;
                _c.SpudDate = well.SpudDate;
                _c.CompletionDate = well.CompletionDate;
                _c.TD = well.TD;
                _c.MDDF = well.MDDF;
                _c.TVDDF = well.TVDDF;
                _c.CreatedBy = GetCurrentUserId();
                _c.CreatedDate = Utility.Now();
                _context.Wells.Add(_c);
                await _context.SaveChangesAsync();
                res = "Sucessfully Added";
            }

            return res;
        }
        public async Task<string> DeleteWell(int id)
        {
            string res = "";
            var _c = await _context.Wells.Where(x => x.WellId == id).FirstOrDefaultAsync();
            if (_c != null)
            {
                _context.Wells.Remove(_c);
                await _context.SaveChangesAsync();
                res = "Sucessfully Deleted";
            }
            else
            {
                res = "Invalid Well";
            }
            return res;
        }
        public async Task<List<GetWellVM>> GetWell(int id)
        {
            var _c = new List<Well>();
            if (id > 0)
            {
                _c = await _context.Wells.Where(x => x.WellId == id).ToListAsync();
            }
            else
            {
                _c = await _context.Wells.ToListAsync();
            }

            var transForm = _c.AsEnumerable().Select(x => new GetWellVM
            {
                 IconUrl=x.IconUrl,
                  WellType=x.WellType,
                   WellId=x.WellId,
               Name = x.Name,
            Alias = x.Alias,
            FieldName = x.FieldName,
            BlockName = x.BlockName,
            BasinName = x.BasinName,
            Latitude = x.Latitude,
            Longitude = x.Longitude,
            XEasting = x.XEasting,
            YNorthing = x.YNorthing,
            SpudDate = x.SpudDate,
            CompletionDate = x.CompletionDate,
            TD = x.TD,
            MDDF = x.MDDF,
            TVDDF = x.TVDDF

        }).ToList();
            return transForm;
        }

    }
}
