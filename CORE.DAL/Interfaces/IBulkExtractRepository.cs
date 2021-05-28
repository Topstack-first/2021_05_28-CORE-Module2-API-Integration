﻿using Core.DAL.Models;
using System.Collections.Generic;

namespace Core.DAL.Services
{
    public interface IBulkExtractRepository
    {
        ICollection<BulkExtract> GetBulkExtracts();

        BulkExtract AddBulkExtract(BulkExtract bulkExtract);
        BulkExtract UpdateBulkExtract(BulkExtract bulkExtract);
        BulkExtract GetBulkExtractById(int bulkExtractId);
    }
}
