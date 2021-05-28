using Core.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Core.Api.Dtos
{
    public class BulkExtractDto
    {
        public int BulkExtractId { get; set; }
        public string BulkExtractTitle { get; set; }
        public string BulkExtractDescription { get; set; }
        public string BulkExtractPath { get; set; }
        public DateTime? BulkExtractDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int TotalDocuments { get; set; }
        public int ProcessedDocuments { get; set; }
        public int UploadedDocuments { get; set; }
        public string DocumentType { get; set; }
    }
}
