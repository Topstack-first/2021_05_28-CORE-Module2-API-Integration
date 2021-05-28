using Core.DAL.Contexts;
using Core.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DAL.Services
{
    public class BulkExtractRepository:IBulkExtractRepository
    {
        private CoreDBContext _dbContext;

        public BulkExtractRepository(CoreDBContext context)
        {
            _dbContext = context;
        }

        public ICollection<BulkExtract> GetBulkExtracts()
        {
            return _dbContext.BulkExtract
                .Where(d => true)
                .OrderByDescending(a=>a.bulk_extract_date)
                .ToList();
        }
        public  BulkExtract AddBulkExtract(BulkExtract bulkExtract)
        {
            _dbContext.BulkExtract.Add(bulkExtract);
            Save();
            return  _dbContext.BulkExtract.OrderByDescending(c=>c.bulk_extract_id).FirstOrDefault();
        }

        public BulkExtract UpdateBulkExtract(BulkExtract bulkExtract)
        {
            try
            {
                _dbContext.BulkExtract.Update(bulkExtract);
            }
            catch(Exception e)
            {

            }
            Save();
            
            return _dbContext.BulkExtract.Where(d=>d.bulk_extract_id == bulkExtract.bulk_extract_id).FirstOrDefault();
        }
        
        public BulkExtract GetBulkExtractById(int bulkExtractId)
        {
            return _dbContext.BulkExtract.FirstOrDefault(a=>a.bulk_extract_id == bulkExtractId);
        }
        
        public bool Save()
        {
            var saved = _dbContext.SaveChanges();
            return saved >= 0 ? true : false;
        }
    }
}