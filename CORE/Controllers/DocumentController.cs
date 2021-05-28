using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Api.Dtos;
using Core.Api.Mapper;
using Core.DAL.Models;
using CORE.DAL.Models;
using Core.DAL.Services;
using CORE.Misc.Enums;
using CORE.Extract;
using CORE.Misc.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private IDocumentRepository _documentRepository;
        private IUserRepository _userRepository;
        // private string documentStorePath = "./../../../data/documents/";
        // private string networkStorePath = "//192.168.105.86/data/documents";
        private string documentStorePath = @"\\42.1.63.84\Users\syafiq\Documents\CORE\Documents\";
        private string networkStorePath = @"\\42.1.63.84\Users\syafiq\Documents\CORE\Documents\";
        private IConfiguration _configuration;
        

        public DocumentController(IDocumentRepository documentRepository, IUserRepository userRepository, IConfiguration configuration)
        {
            _documentRepository = documentRepository;
            _userRepository = userRepository;
            _configuration = configuration;
            
            documentStorePath = _configuration["document:DocumentStorePath"];
            networkStorePath = _configuration["document:NetworkStorePath"];
        }

        [HttpGet("GetDocuments")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<DocumentDto>))]
        public IActionResult GetDocuments(IDocumentRepository.TYPE type, int userId) //TODO: add user id in global variable, to re-use everywhere
        {
            var documents = _documentRepository.GetDocuments(type, userId)
                .OrderByDescending(a=>a.doc_modified_publish_date);
            
            var documentDtoList = new List<DocumentDto>();
            foreach (var doc in documents)
            {
                var documentDto = EntityMapper.MapDocument(doc);
                documentDto.UserName =  _userRepository.GetUserById(doc.document_author)?.UserName;
                documentDtoList.Add(documentDto);
            }
            
            return Ok(documentDtoList);
        }
        
        [HttpGet("GetAllUsers")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
        public IActionResult GetAllUsers()
        {
            var users = _userRepository.GetAllUsers();
            
            var userDtoList = new List<UserDto>();
            foreach (var user in users)
            {
                var userDto = EntityMapper.MapUser(user);
                userDtoList.Add(userDto);
            }
            
            return Ok(userDtoList);
        }
        
        [HttpPost("AddDocument")]
        [ProducesResponseType(200, Type = typeof(DocumentDto))]
        public IActionResult AddDocument([FromBody] DocumentDto documentDto)
        {
            documentDto.CategoryName = (documentDto.CategoryName == "-1") ? "" : documentDto.CategoryName;
            documentDto.SubcategoryName = (documentDto.SubcategoryName == "-1") ? "" : documentDto.SubcategoryName;
            documentDto.DepartmentName = (documentDto.DepartmentName == "-1") ? "" : documentDto.DepartmentName;
            documentDto.EventName = (documentDto.EventName == "-1") ? "" : documentDto.EventName;
            documentDto.LocationName = (documentDto.LocationName == "-1") ? "" : documentDto.LocationName;
            documentDto.StakeholderName = (documentDto.StakeholderName == "-1") ? "" : documentDto.StakeholderName;
            documentDto.WellName = (documentDto.WellName == "-1") ? "" : documentDto.WellName;
            
            var documentFromDto = EntityMapper.MapDocumentDto(documentDto);

            if (!string.IsNullOrEmpty(documentFromDto.category_name))
            {
                documentFromDto.category_name = EnumExtensions.GetDescriptionFromEnumValue(EnumExtensions.ParseEnum<Category>(documentFromDto.category_name));
            }

            if (!string.IsNullOrEmpty(documentFromDto.subcategory_name))
            {
                documentFromDto.subcategory_name = EnumExtensions.GetDescriptionFromEnumValue(EnumExtensions.ParseEnum<SubCategory>(documentFromDto.subcategory_name));
            }
            
            if (!string.IsNullOrEmpty(documentFromDto.department_name))
            {
                documentFromDto.department_name = EnumExtensions.GetDescriptionFromEnumValue(EnumExtensions.ParseEnum<Department>(documentFromDto.department_name));
            }
            
            if (!string.IsNullOrEmpty(documentFromDto.event_name))
            {
                documentFromDto.event_name = EnumExtensions.GetDescriptionFromEnumValue(EnumExtensions.ParseEnum<Event>(documentFromDto.event_name));
            }
            
            if (!string.IsNullOrEmpty(documentFromDto.location_name))
            {
                documentFromDto.location_name = EnumExtensions.GetDescriptionFromEnumValue(EnumExtensions.ParseEnum<Location>(documentFromDto.location_name));
            }
            
            if (!string.IsNullOrEmpty(documentFromDto.stakeholder_name))
            {
                documentFromDto.stakeholder_name = EnumExtensions.GetDescriptionFromEnumValue(EnumExtensions.ParseEnum<Stakeholder>(documentFromDto.stakeholder_name));
            }
            
            if (!string.IsNullOrEmpty(documentFromDto.well_name))
            {
                documentFromDto.well_name = EnumExtensions.GetDescriptionFromEnumValue(EnumExtensions.ParseEnum<Well>(documentFromDto.well_name));
            }
            
            if (documentFromDto.document_author == null || documentFromDto.document_author == 0)
            {
                documentFromDto.document_author = 1;
            }
            
            documentFromDto.doc_upload_date = DateTime.Now;
            
            var documentFromDb = _documentRepository.AddDocument(documentFromDto);
            DocumentDto result = EntityMapper.MapDocument(documentFromDb);

            result.UserName = _userRepository.GetUserById(documentFromDto.document_author)?.UserName;

            return Ok(result);
        }
        
        [HttpPost("UploadDocument"), DisableRequestSizeLimit]
        //[Consumes("multipart/form-data")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(415)]
        public IActionResult UploadDocument(IFormFile uploadFile, [FromForm] string uploadedFilename, [FromForm] int documentId)
        {
            var targetUploadedFilename = documentStorePath + uploadedFilename;
            
            if (uploadFile == null)
            {
                return Ok(false);
            }
            if (System.IO.File.Exists(targetUploadedFilename))
            {
                System.IO.File.Delete(targetUploadedFilename);
            }
            using (var stream = new FileStream(targetUploadedFilename, FileMode.Create))
            {
                uploadFile.CopyTo(stream);
            }
            
            var content = DocumentExtractor.Extract(targetUploadedFilename);
            var docFromDb = _documentRepository.GetDocument(documentId);
            docFromDb.document_content = content;
            docFromDb.document_date = System.IO.File.GetCreationTime(targetUploadedFilename);
            docFromDb.doc_modified_publish_date = DateTime.Now;
            
            var doc = _documentRepository.UpdateDocument(docFromDb);

            return Ok(true);
        }
        
        [HttpPost("UpdateDocument")]
        [ProducesResponseType(200, Type = typeof(DocumentDto))]
        public IActionResult UpdateDocument([FromBody] DocumentDto documentDto)
        {
            var documentFromDto = EntityMapper.MapDocumentDto(documentDto);
            var enumResult = 0;
            
            if (!string.IsNullOrEmpty(documentDto.CategoryName) && int.TryParse(documentDto.CategoryName, out enumResult))
            {
                documentFromDto.category_name = EnumExtensions.GetDescriptionFromEnumValue(EnumExtensions.ParseEnum<Category>(documentDto.CategoryName));
            }

            if (!string.IsNullOrEmpty(documentDto.SubcategoryName) && int.TryParse(documentDto.SubcategoryName, out enumResult))
            {
                documentFromDto.subcategory_name = EnumExtensions.GetDescriptionFromEnumValue(EnumExtensions.ParseEnum<SubCategory>(documentDto.SubcategoryName));
            }
            
            if (!string.IsNullOrEmpty(documentDto.DepartmentName) && int.TryParse(documentDto.DepartmentName, out enumResult))
            {
                documentFromDto.department_name = EnumExtensions.GetDescriptionFromEnumValue(EnumExtensions.ParseEnum<Department>(documentDto.DepartmentName));
            }
            
            if (!string.IsNullOrEmpty(documentDto.EventName) && int.TryParse(documentDto.EventName, out enumResult))
            {
                documentFromDto.event_name = EnumExtensions.GetDescriptionFromEnumValue(EnumExtensions.ParseEnum<Event>(documentDto.EventName));
            }
            
            if (!string.IsNullOrEmpty(documentDto.LocationName) && int.TryParse(documentDto.LocationName, out enumResult))
            {
                documentFromDto.location_name = EnumExtensions.GetDescriptionFromEnumValue(EnumExtensions.ParseEnum<Location>(documentDto.LocationName));
            }
            
            if (!string.IsNullOrEmpty(documentDto.StakeholderName) && int.TryParse(documentDto.StakeholderName, out enumResult))
            {
                documentFromDto.stakeholder_name = EnumExtensions.GetDescriptionFromEnumValue(EnumExtensions.ParseEnum<Stakeholder>(documentDto.StakeholderName));
            }
            
            if (!string.IsNullOrEmpty(documentDto.WellName) && int.TryParse(documentDto.WellName, out enumResult))
            {
                documentFromDto.well_name = EnumExtensions.GetDescriptionFromEnumValue(EnumExtensions.ParseEnum<Well>(documentDto.WellName));
            }
            
            DocumentDto result = EntityMapper.MapDocument(_documentRepository.UpdateDocument(documentFromDto));

            return Ok(result);
        }
        
        [HttpPost("DeleteDocumentNormal")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult DeleteDocumentNormal([FromBody] DocumentDto documentDto)
        {
            bool result = _documentRepository.DeleteDocumentNormal(EntityMapper.MapDocumentDto(documentDto));
            return Ok(result);
        }
        [HttpPost("DeleteDocumentPermanently")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult DeleteDocumentPermanently([FromBody] DocumentDto documentDto)
        {
            bool result = _documentRepository.DeleteDocumentPermanently(EntityMapper.MapDocumentDto(documentDto));
            return Ok(result);
        }
        [HttpPost("RestoreDocument")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult RestoreDocument([FromBody] DocumentDto documentDto)
        {
            bool result = _documentRepository.RestoreDocument(EntityMapper.MapDocumentDto(documentDto));
            return Ok(result);
        }

        [HttpPost("BulkUpdateDocuments")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult BulkUpdateDocuments([FromBody] DocumentDto[] documentDtos)
        {
            Document[] docs = new Document[documentDtos.Length];
            for(int i=0;i<documentDtos.Length;i++)
            {
                docs[i] = EntityMapper.MapDocumentDto(documentDtos[i]);
            }
            bool result = _documentRepository.BulkUpdateDocuments(docs);
            return Ok(result);
        }
        [HttpPost("BulkDeleteDocuments")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult BulkDeleteDocuments([FromBody] DocumentDto[] documentDtos)
        {
            Document[] docs = new Document[documentDtos.Length];
            for (int i = 0; i < documentDtos.Length; i++)
            {
                docs[i] = EntityMapper.MapDocumentDto(documentDtos[i]);
            }
            bool result = _documentRepository.BulkDeleteDocuments(docs);
            return Ok(result);
        }

        [HttpPost("BulkRestoreDocuments")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult BulkRestoreDocuments([FromBody] DocumentDto[] documentDtos)
        {
            Document[] docs = new Document[documentDtos.Length];
            for (int i = 0; i < documentDtos.Length; i++)
            {
                docs[i] = EntityMapper.MapDocumentDto(documentDtos[i]);
            }
            bool result = _documentRepository.BulkRestoreDocuments(docs);
            return Ok(result);
        }
        
        [HttpPost("GetExtractedContent")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult GetExtractedContent(IFormFile uploadFile, [FromForm] string uploadedFilename)
        {
            var destinationFileName = documentStorePath + uploadedFilename;
            if (uploadFile == null)
            {
                return Ok(false);
            }
            if (System.IO.File.Exists(destinationFileName))
            {
                System.IO.File.Delete(destinationFileName);
            }
            using (var stream = new FileStream(destinationFileName, FileMode.Create))
            {
                uploadFile.CopyTo(stream);
            }
            
            var result = _documentRepository.GetExtractedContentWithMetadata(destinationFileName);

            return Ok(result);
        }
        
        [HttpPost("GetExtractedContentFromExistingNetworkFile")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult GetExtractedContentFromExistingNetworkFile([FromForm] string uploadedFilename)
        {
            ExtractedContentFromDocument result = null;
            if (System.IO.File.Exists(uploadedFilename))
            {
                result = _documentRepository.GetExtractedContentWithMetadata(uploadedFilename);
            }

            return Ok(result);
        }

        [HttpPost("BulkDeleteDocumentsPermanently")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult BulkDeleteDocumentsPermanently([FromBody] DocumentDto[] documentDtos)
        {
            Document[] docs = new Document[documentDtos.Length];
            for (int i = 0; i < documentDtos.Length; i++)
            {
                docs[i] = EntityMapper.MapDocumentDto(documentDtos[i]);
            }
            bool result = _documentRepository.BulkDeleteDocumentsPermanently(docs);
            return Ok(result);
        }
        [HttpPost("GetWebAddressForFile")]
        [ProducesResponseType(200, Type = typeof(string))]
        public IActionResult GetWebAddressForFile(string param)
        {
            string[] splits = param.Split(new char[]{ '/','\\'});
            string filename = splits.Last();
            return Ok(networkStorePath + filename);
        }
    }
}
