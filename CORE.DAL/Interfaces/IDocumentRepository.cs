using Core.DAL.Models;
using System.Collections.Generic;
using CORE.DAL.Models;

namespace Core.DAL.Services
{
    public interface IDocumentRepository
    {
        public enum TYPE
        {
            ALL,
            MINE,
            APPROVED,
            IN_REVIEW,
            REJECTED,
            OCR_QUEUE,
            TRASH
        }
        ICollection<Document> GetDocuments(TYPE type, int userId);
        ICollection<Document> GetOcrDocsByBulkId(int bulkExtractId);
        ICollection<Document> GetImportedDocsByBulkId(int bulkExtractId);

        Document AddDocument(Document doc);
        Document GetDocument(int id);
        Document UpdateDocument(Document doc);
        bool DeleteDocumentNormal(Document doc);
        bool RestoreDocument(Document doc);
        bool DeleteDocumentPermanently(Document doc);

        bool BulkUpdateDocuments(Document[] docs);
        bool BulkDeleteDocuments(Document[] docs);
        bool BulkDeleteDocumentsPermanently(Document[] docs);
        bool BulkRestoreDocuments(Document[] docs);
        ExtractedContentFromDocument GetExtractedContentWithMetadata(string filename);
    }
}
