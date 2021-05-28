using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CORE.Misc.Enums;

namespace Core.Api.Dtos
{
    public class DocumentDto
    {
        public int DocumentId { get; set; }
        public string DocumentTitle { get; set; }
        public string DocumentType { get; set; }
        public string DocumentPath { get; set; }
        public int DocumentAuthor { get; set; }
        public string UserName { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string WellName { get; set; }
        public int WellIndex { get; set; }
        public string CategoryName { get; set; }
        public string SubcategoryName { get; set; }
        public string DepartmentName { get; set; }
        public string StakeholderName { get; set; }
        public string EventName { get; set; }
        public string LocationName { get; set; }
        public string UploadStatus { get; set; }
        public string ContentExtracted { get; set; }
        public int? IsUploadedDocument { get; set; }
        public int? IsProcessedDocument { get; set; }
        public DateTime? DocModifiedPublishDate { get; set; }
        public DateTime? DocUploadDate { get; set; }
        public string DocumentDescription { get; set; }
        public string DocumentAddress { get; set; }
        public string DocumentCustomContent { get; set; }
        public string DocumentContent { get; set; }
        public string DocumentApproval { get; set; }
        public string TagName { get; set; }
        public int Deleted { get; set; }
        public string OcrStatus { get; set; }
        public int? BulkExtractId { get; set; }
        public int LocationIndex { get; set; }
        public int EventIndex { get; set; }
        public int StakeholderIndex { get; set; }
        public int DepartmentIndex { get; set; }
        public int SubcategoryIndex { get; set; }
        public int CategoryIndex { get; set; }
        public ContentPermission ContentPermission { get; set; }
    }
}
