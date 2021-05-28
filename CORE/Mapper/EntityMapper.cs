using Core.Api.Dtos;
using Core.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CORE.Misc.Enums;
using CORE.Misc.Extensions;

namespace Core.Api.Mapper
{
	public static class EntityMapper
	{
		public static UserDto MapUser(User user)
		{
			return new UserDto
			{
				UserId = user.UserId,
				Username = user.UserName,
				Password = user.UserPassword,
                FirstName = user.FirstName,
                LastName = user.LastName,
				RoleId = user.RoleId,
			};
		}
		
		public static DocumentDto MapDocument(Document document)
		{
			return new DocumentDto
			{

                DocumentId = document.document_id,
                DocumentTitle = document.document_title,
                DocumentType = document.document_type,
                DocumentPath = document.document_path,
                DocumentAuthor = document.document_author,
                DocumentDate = document.document_date,
                WellName = document.well_name,
                WellIndex = (string.IsNullOrEmpty(document.well_name)) ? -1 : (int)EnumExtensions.GetValueFromDescription<Well>(document.well_name),
                CategoryName = document.category_name,
                CategoryIndex = (string.IsNullOrEmpty(document.category_name)) ? -1 : (int)EnumExtensions.GetValueFromDescription<Category>(document.category_name),
                SubcategoryName = document.subcategory_name,
                SubcategoryIndex = (string.IsNullOrEmpty(document.subcategory_name)) ? -1 : (int)EnumExtensions.GetValueFromDescription<SubCategory>(document.subcategory_name),
                DepartmentName = document.department_name,
                DepartmentIndex = (string.IsNullOrEmpty(document.department_name)) ? -1 : (int)EnumExtensions.GetValueFromDescription<Department>(document.department_name),
                StakeholderName = document.stakeholder_name,
                StakeholderIndex = (string.IsNullOrEmpty(document.stakeholder_name)) ? -1 : (int)EnumExtensions.GetValueFromDescription<Stakeholder>(document.stakeholder_name),
                EventName = document.event_name,
                EventIndex = (string.IsNullOrEmpty(document.event_name)) ? -1 : (int)EnumExtensions.GetValueFromDescription<Event>(document.event_name),
                LocationName = document.location_name,
                LocationIndex = (string.IsNullOrEmpty(document.location_name)) ? -1 : (int)EnumExtensions.GetValueFromDescription<Location>(document.location_name),
                UploadStatus = document.upload_status,
                ContentExtracted = document.content_extracted,
                IsUploadedDocument = document.is_uploaded_document,
                IsProcessedDocument = document.is_processed_document,
                DocModifiedPublishDate = document.doc_modified_publish_date,
                DocUploadDate = document.doc_upload_date,
                DocumentDescription = document.document_description,
                DocumentAddress = document.document_address,
                DocumentCustomContent = document.document_custom_content,
                DocumentContent = document.document_content,
                DocumentApproval = document.document_approval,
                TagName = document.tag_name,
                Deleted = document.deleted,
                OcrStatus = document.ocr_status,
                BulkExtractId = document.bulk_extract_id
            };
		}
        public static Document MapDocumentDto(DocumentDto documentDto)
        {
            return new Document
            {
                document_id = documentDto.DocumentId,
                document_title = documentDto.DocumentTitle,
                document_type = documentDto.DocumentType,
                document_path = documentDto.DocumentPath,
                document_author = documentDto.DocumentAuthor,
                document_date = documentDto.DocumentDate,
                well_name = documentDto.WellName,
                category_name = documentDto.CategoryName,
                subcategory_name = documentDto.SubcategoryName,
                department_name = documentDto.DepartmentName,
                stakeholder_name = documentDto.StakeholderName,
                event_name = documentDto.EventName,
                location_name = documentDto.LocationName,
                upload_status = documentDto.UploadStatus,
                content_extracted = documentDto.ContentExtracted,
                is_uploaded_document = documentDto.IsUploadedDocument,
                is_processed_document = documentDto.IsProcessedDocument,
                doc_modified_publish_date = documentDto.DocModifiedPublishDate,
                doc_upload_date = documentDto.DocUploadDate,
                document_description = documentDto.DocumentDescription,
                document_address = documentDto.DocumentAddress,
                document_custom_content = documentDto.DocumentCustomContent,
                document_content = documentDto.DocumentContent,
                document_approval = documentDto.DocumentApproval,
                tag_name = documentDto.TagName,
                deleted = documentDto.Deleted,
                ocr_status = documentDto.OcrStatus,
                bulk_extract_id = documentDto.BulkExtractId,
                content_permission = (int)documentDto.ContentPermission
            };
        }
        public static BulkExtractDto MapBulkExtract(BulkExtract bulkExtract)
        {
            return new BulkExtractDto
            {
                BulkExtractId = bulkExtract.bulk_extract_id,
                BulkExtractTitle = bulkExtract.bulk_extract_title,
                BulkExtractDescription = bulkExtract.bulk_extract_description,
                BulkExtractPath = bulkExtract.bulk_extract_path,
                BulkExtractDate = bulkExtract.bulk_extract_date,
                UserId = bulkExtract.user_id,
                TotalDocuments = bulkExtract.total_documents,
                ProcessedDocuments = bulkExtract.processed_documents,
                UploadedDocuments = bulkExtract.uploaded_documents,
                DocumentType = bulkExtract.document_type
            };
        }
        public static BulkExtract MapBulkExtractDto(BulkExtractDto bulkExtractDto)
        {
            return new BulkExtract
            {
                bulk_extract_id = bulkExtractDto.BulkExtractId,
                bulk_extract_title = bulkExtractDto.BulkExtractTitle,
                bulk_extract_description = bulkExtractDto.BulkExtractDescription,
                bulk_extract_path = bulkExtractDto.BulkExtractPath,
                bulk_extract_date = bulkExtractDto.BulkExtractDate,
                user_id = bulkExtractDto.UserId,
                total_documents = bulkExtractDto.TotalDocuments,
                processed_documents = bulkExtractDto.ProcessedDocuments,
                uploaded_documents = bulkExtractDto.UploadedDocuments,
                document_type = bulkExtractDto.DocumentType
            };
        }
    }
}
