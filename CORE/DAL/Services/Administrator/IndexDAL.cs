using BeicipFranLabERP.BL.Repository.Administrator;
using BeicipFranLabERP.DAL.Model;
using BeicipFranLabERP.ViewModel.Index;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CORE.Models;

namespace BeicipFranLabERP.DAL.Services.Administrator
{
    public class IndexDAL: IIndex
    {
        private COREContext _context;
        public IndexDAL(COREContext appDbContext)
        {
            _context = appDbContext;
        }

        public string GetCurrentUserId()
        {
            return "Test123";
        }
        public async Task<string> SaveIndex(AddIndexVM ind)
        {
            string res = "";

            if (ind.Id> 0)
            {
                var _c = await _context.SEIndwxes.Where(x => x.Id ==ind.Id).FirstOrDefaultAsync();
                _c.ContentStopwordToAdd = ind.ContentStopwordToAdd;
                _c.DefaultOperator = ind.DefaultOperator;
                _c.DefaultOrder = ind.DefaultOrder;
                _c.Element = ind.Element;
                _c.ExportableListOfContentStopwords = ind.ExportableListOfContentStopwords;
                _c.ExportableListOfStopwords = ind.ExportableListOfStopwords;
                _c.KeyWordMatching = ind.KeyWordMatching;
                _c.RecentPostBonusCutoff = ind.RecentPostBonusCutoff;
                _c.StopwordsToAdd = ind.StopwordsToAdd;
                _c.Synonyms = ind.Synonyms;
                _c.Weight = ind.Weight;
                _context.SEIndwxes.Add(_c);
                await _context.SaveChangesAsync();
                res = "Sucessfully Updated";
            }
            else
            {
                var _c = new SEIndex();
                _c.ContentStopwordToAdd = ind.ContentStopwordToAdd;
                _c.DefaultOperator = ind.DefaultOperator;
                 _c.DefaultOrder = ind.DefaultOrder;
                 _c.Element = ind.Element;
                 _c.ExportableListOfContentStopwords = ind.ExportableListOfContentStopwords;
                 _c.ExportableListOfStopwords = ind.ExportableListOfStopwords;
                 _c.KeyWordMatching = ind.KeyWordMatching;
                _c.RecentPostBonusCutoff = ind.RecentPostBonusCutoff;
                 _c.StopwordsToAdd = ind.StopwordsToAdd;
                  _c.Synonyms = ind.Synonyms;
                _c.Weight = ind.Weight;
                  _context.SEIndwxes.Add(_c);
                await _context.SaveChangesAsync();
                res = "Sucessfully Added";
            }

            return res;
        }
        public async Task<string> DeleteIndex(int id)
        {
            string res = "";
            var _c = await _context.SEIndwxes.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (_c != null)
            {
                _context.SEIndwxes.Remove(_c);
                await _context.SaveChangesAsync();
                res = "Sucessfully Deleted";
            }
            else
            {
                res = "Invalid Index";
            }
            return res;
        }
        public async Task<List<GetIndexVM>> GetIndex(int id)
        {
            var _c = new List<SEIndex>();
            if (id > 0)
            {
                _c = await _context.SEIndwxes.Where(x => x.Id == id).ToListAsync();
            }
            else
            {
                _c = await _context.SEIndwxes.ToListAsync();
            }

            var transForm = _c.AsEnumerable().Select(ind => new GetIndexVM
            {
                ContentStopwordToAdd = ind.ContentStopwordToAdd,
            DefaultOperator = ind.DefaultOperator,
            DefaultOrder = ind.DefaultOrder,
            Element = ind.Element,
            ExportableListOfContentStopwords = ind.ExportableListOfContentStopwords,
            ExportableListOfStopwords = ind.ExportableListOfStopwords,
           KeyWordMatching = ind.KeyWordMatching,
            RecentPostBonusCutoff = ind.RecentPostBonusCutoff,
            StopwordsToAdd = ind.StopwordsToAdd,
            Synonyms = ind.Synonyms,
            Weight = ind.Weight,

        }).ToList();
            return transForm;
        }

    }
}
