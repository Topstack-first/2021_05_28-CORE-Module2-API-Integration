using Core.DAL.Contexts;
using Core.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CORE.DAL.Models;
using CORE.Extract;
using CORE.Misc.Enums;
using CORE.Misc.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Core.DAL.Services
{
    public class DocumentRepository : IDocumentRepository
	{
		private CoreDBContext _dbContext;

		public DocumentRepository(CoreDBContext context)
		{
			_dbContext = context;
		}

		public ICollection<Document> GetDocuments(IDocumentRepository.TYPE type, int userId)
		{
			if(type == IDocumentRepository.TYPE.ALL)
            {
				return _dbContext.Document.Where(doc => doc.deleted == 0 && doc.is_processed_document == 1 && !(doc.document_author != userId && doc.content_permission != null && doc.content_permission == (int?) ContentPermission.Private)).ToList();
			}
			else if(type == IDocumentRepository.TYPE.MINE)
            {
				return _dbContext.Document.Where(doc => doc.deleted == 0 && doc.document_author == userId && doc.is_processed_document == 1).ToList();
			}
			else if(type == IDocumentRepository.TYPE.APPROVED)
            {
				return _dbContext.Document.Where(doc => doc.document_approval.Equals("Approved") && doc.deleted == 0 && doc.is_processed_document == 1 && !(doc.document_author != userId && doc.content_permission != null && doc.content_permission == (int?) ContentPermission.Private)).ToList();
			}
			else if (type == IDocumentRepository.TYPE.IN_REVIEW)
			{
				return _dbContext.Document.Where(doc => doc.document_approval.Equals("In Review") && doc.deleted == 0 && doc.is_processed_document == 1 && !(doc.document_author != userId && doc.content_permission != null && doc.content_permission == (int?) ContentPermission.Private)).ToList();
			}
			else if (type == IDocumentRepository.TYPE.REJECTED)
			{
				return _dbContext.Document.Where(doc => doc.document_approval.Equals("Rejected") && doc.deleted == 0  && doc.is_processed_document == 1 && !(doc.document_author != userId && doc.content_permission != null && doc.content_permission == (int?) ContentPermission.Private)).ToList();
			}
			else if (type == IDocumentRepository.TYPE.OCR_QUEUE)
			{
				return _dbContext.Document.Where(doc => doc.ocr_status != null && doc.deleted == 0  && doc.is_processed_document == 1).ToList();
			}
			else if (type == IDocumentRepository.TYPE.TRASH)
			{
				return _dbContext.Document.Where(doc => doc.deleted == 1  && doc.is_processed_document == 1 && !(doc.document_author != userId && doc.content_permission != null && doc.content_permission == (int?) ContentPermission.Private)).ToList();
			}
			
			return _dbContext.Document.Where(doc => true  && doc.is_processed_document == 1).ToList();
		}
		public ICollection<Document> GetOcrDocsByBulkId(int bulkExtractId)
        {
			return _dbContext.Document.Where(doc => doc.ocr_status != null && doc.deleted == 0 && doc.bulk_extract_id == bulkExtractId).ToList();
		}
		public ICollection<Document> GetImportedDocsByBulkId(int bulkExtractId)
        {
			return _dbContext.Document
				.Where(doc => (doc.is_processed_document == 0 || doc.is_processed_document == null) && 
				              doc.deleted == 0 && 
				              doc.bulk_extract_id == bulkExtractId).ToList();
		}
		
		public Document AddDocument(Document doc)
        {
			_dbContext.Document.Add(doc);
			Save();
			return _dbContext.Document.OrderBy(c => c.document_id).LastOrDefault();
		}
		
		public Document GetDocument(int id)
		{
			return _dbContext.Document.FirstOrDefault(a=>a.document_id == id);
		}
		
		public Document UpdateDocument(Document doc)
        {
            try
            {
				_dbContext.Document.Update(doc);
			}
			catch(Exception e)
            {

            }
			Save();
			return _dbContext.Document.Where(d=>d.document_id == doc.document_id).FirstOrDefault();
		}
		
		public bool DeleteDocumentNormal(Document doc)
        {
			doc.deleted = 1;
			_dbContext.Document.Update(doc);
			return Save();
		}
		public bool RestoreDocument(Document doc)
        {
			doc.deleted = 0;
			_dbContext.Document.Update(doc);
			return Save();
		}
		public bool DeleteDocumentPermanently(Document doc)
        {
			_dbContext.Document.Remove(doc);
			return Save();
		}


		public bool BulkUpdateDocuments(Document[] docs)
        {
			_dbContext.Document.UpdateRange(docs);
			return Save();
		}
		
		public bool BulkDeleteDocuments(Document[] docs)
        {
			for(int i=0; i< docs.Length;i++)
            {
				docs[i].deleted = 1;
            }
			_dbContext.Document.UpdateRange(docs);
			return Save();
		}
		public bool BulkDeleteDocumentsPermanently(Document[] docs)
        {
			_dbContext.Document.RemoveRange(docs);
			return Save();
		}
		
		public bool BulkRestoreDocuments(Document[] docs)
        {
			for (int i = 0; i < docs.Length; i++)
			{
				docs[i].deleted = 0;
			}
			_dbContext.Document.UpdateRange(docs);
			return Save();
		}

		public ExtractedContentFromDocument GetExtractedContentWithMetadata(string filename)
		{
			var matchedCategory = Category.NotAvailable;
			var matchedSubCategory = SubCategory.NotAvailable;
			var matchedLocation = Location.NotAvailable;
			var matchedWell = Well.NotAvailable;
			var matchedStakeholder = Stakeholder.NotAvailable;
			var matchedDepartment = Department.NotAvailable;
			var matchedEvent = Event.NotAvailable;
			var isAbleToExtract = false;
			var content = "";

			try
			{
				content = DocumentExtractor.Extract(filename);
				isAbleToExtract = !string.IsNullOrEmpty(content);

				foreach (var category in Enum.GetValues(typeof(Category)))
				{
					if (content.Contains(EnumExtensions.GetDescriptionFromEnumValue((Category)category)))
					{
						matchedCategory = (Category)category;
						break;
					}
				}
			
				foreach (var subCategory in Enum.GetValues(typeof(SubCategory)))
				{
					if (content.Contains(EnumExtensions.GetDescriptionFromEnumValue((SubCategory)subCategory)))
					{
						matchedSubCategory = (SubCategory)subCategory;
						break;
					}
				}
			
				foreach (var location in Enum.GetValues(typeof(Location)))
				{
					if (content.Contains(EnumExtensions.GetDescriptionFromEnumValue((Location)location)))
					{
						matchedLocation = (Location)location;
						break;
					}
				}
			
				foreach (var well in Enum.GetValues(typeof(Well)))
				{
					if (content.Contains(EnumExtensions.GetDescriptionFromEnumValue((Well)well)))
					{
						matchedWell = (Well)well;
						break;
					}
				}
			
				foreach (var stakeholder in Enum.GetValues(typeof(Stakeholder)))
				{
					if (content.Contains(EnumExtensions.GetDescriptionFromEnumValue((Stakeholder)stakeholder)))
					{
						matchedStakeholder = (Stakeholder)stakeholder;
						break;
					}
				}
			
				foreach (var department in Enum.GetValues(typeof(Department)))
				{
					if (content.Contains(EnumExtensions.GetDescriptionFromEnumValue((Department)department)))
					{
						matchedDepartment = (Department)department;
						break;
					}
				}
			
				foreach (var singleEvent in Enum.GetValues(typeof(Event)))
				{
					if (content.Contains(EnumExtensions.GetDescriptionFromEnumValue((Event)singleEvent)))
					{
						matchedEvent = (Event)singleEvent;
						break;
					}
				}
			}
			
			catch (Exception e)
			{
				Console.WriteLine(e);
				isAbleToExtract = false;
			}
			
			return new ExtractedContentFromDocument()
			{
				Content = content,
				Event = (int)matchedEvent,
				Category = (int)matchedCategory,
				SubCategory = (int)matchedSubCategory,
				Department = (int)matchedDepartment,
				Stakeholder = (int)matchedStakeholder,
				Location = (int)matchedLocation,
				Well = (int)matchedWell,
				IsAbleToExtract = isAbleToExtract
			};
		}

		public bool Save()
		{
			var saved = _dbContext.SaveChanges();
			return saved >= 0 ? true : false;
		}
	}
}
