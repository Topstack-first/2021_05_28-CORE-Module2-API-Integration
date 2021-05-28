using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Api.Dtos;
using Core.Api.Mapper;
using Core.DAL.Models;
using Core.DAL.Services;
using CORE.Extract;
using CORE.Misc.Enums;
using CORE.Misc.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BulkExtractController : ControllerBase
    {
        private IBulkExtractRepository _bulkExtractRepository;
        private IDocumentRepository _documentRepository;
        private IUserRepository _userRepository;

        private string documentStorePath = @"\\42.1.63.84\Users\syafiq\Documents\CORE\Documents\";
        private string networkStorePath = @"\\42.1.63.84\Users\syafiq\Documents\CORE\Documents\";
        private IConfiguration _configuration;

        public BulkExtractController(IBulkExtractRepository bulkExtractRepository, IDocumentRepository documentRepository, IUserRepository userRepository, IConfiguration configuration)
        {
            _bulkExtractRepository = bulkExtractRepository;
            _documentRepository = documentRepository;
            _userRepository = userRepository;
            //
            // documentStorePath = _configuration["document:DocumentStorePath"];
            // networkStorePath = _configuration["document:NetworkStorePath"];
        }

        [HttpGet("GetBulkExtracts")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BulkExtractDto>))]
        public IActionResult GetBulkExtracts()
        {
            var bulkExtracts = _bulkExtractRepository.GetBulkExtracts();
            var bulkExtractDtoList = new List<BulkExtractDto>();
            foreach (var bulkExtract in bulkExtracts)
            {
                var bulkExtractDto = EntityMapper.MapBulkExtract(bulkExtract);
                bulkExtractDto.UserName = _userRepository.GetUserById(bulkExtractDto.UserId).UserName;
                bulkExtractDtoList.Add(bulkExtractDto);
            }
            
            return Ok(bulkExtractDtoList);
        }
        [HttpGet("GetOcrDocsByBulkId/{bulkExtractId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<DocumentDto>))]
        public IActionResult GetOcrDocsByBulkId(int bulkExtractId)
        {
            var documents = _documentRepository.GetOcrDocsByBulkId(bulkExtractId);
            var documentDtoList = new List<DocumentDto>();
            foreach (var doc in documents)
            {
                documentDtoList.Add(EntityMapper.MapDocument(doc));
            }
            return Ok(documentDtoList);
        }
        
        [HttpGet("GetImportedDocsByBulkId/{bulkExtractId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<DocumentDto>))]
        public IActionResult GetImportedDocsByBulkId(int bulkExtractId)
        {
            var documents = _documentRepository.GetImportedDocsByBulkId(bulkExtractId);
            var documentDtoList = new List<DocumentDto>();
            foreach (var doc in documents)
            {
                var documentDto = EntityMapper.MapDocument(doc);
                documentDtoList.Add(documentDto);
            }
            
            return Ok(documentDtoList);
        }
        
        [HttpPost("ImportDocuments")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult ImportDocuments([FromBody] string[] args, int userId)
        {
            string networkPath = args[0];
            string bulkExtractTitle = args[1];
            string bulkExtractDescription = args[2];
            string documentType = args[3];

            string searchPattern = "*.*";
            if(documentType == null)
            {
                documentType = "All Types of Files";
            }
            if(documentType.Equals("PDF Files Only"))
            {
                searchPattern = "*.pdf";
            }
            else if(documentType.Equals("LAS Files Only"))
            {
                searchPattern = "*.las";
            }
            if(!Directory.Exists(@networkPath))
            {
                return Ok(false);
            }
            string[] networkFiles = Directory.GetFiles(@networkPath, searchPattern, SearchOption.AllDirectories);

            BulkExtractDto addedBulkExtract = EntityMapper.MapBulkExtract(
                _bulkExtractRepository.AddBulkExtract(
                    EntityMapper.MapBulkExtractDto( new BulkExtractDto
                        {
                            BulkExtractTitle = bulkExtractTitle,
                            BulkExtractDescription = bulkExtractDescription,
                            BulkExtractPath = networkPath,
                            BulkExtractDate = DateTime.Today,
                            TotalDocuments = networkFiles.Length,
                            UploadedDocuments = networkFiles.Length,
                            UserId = userId,
                            DocumentType = documentType
                        }
                    )
                )
            );
            for(int i=0;i< networkFiles.Length;i++)
            {
                var documentPath = networkFiles[i];
                var extractedContent = _documentRepository.GetExtractedContentWithMetadata(documentPath);
                
                _documentRepository.AddDocument(EntityMapper.MapDocumentDto(
                    new DocumentDto
                    {
                        DocumentAuthor = userId,
                        DocumentTitle = System.IO.Path.GetFileName(documentPath),
                        DocumentPath = documentPath,
                        DocumentDate = DateTime.Today,
                        BulkExtractId = addedBulkExtract.BulkExtractId,
                        DocumentContent = extractedContent.Content,
                        DocumentType = documentType,
                        ContentExtracted = (!string.IsNullOrEmpty(extractedContent.Content) ? "1" : "0"),
                        CategoryName = EnumExtensions.GetDescriptionFromEnumValue((Category)extractedContent.Category),
                        SubcategoryName = EnumExtensions.GetDescriptionFromEnumValue((SubCategory)extractedContent.SubCategory),
                        LocationName = EnumExtensions.GetDescriptionFromEnumValue((Location)extractedContent.Location),
                        WellName = EnumExtensions.GetDescriptionFromEnumValue((Well)extractedContent.Well),
                        StakeholderName =  EnumExtensions.GetDescriptionFromEnumValue((Stakeholder)extractedContent.Stakeholder),
                        DepartmentName = EnumExtensions.GetDescriptionFromEnumValue((Department)extractedContent.Department),
                        EventName = EnumExtensions.GetDescriptionFromEnumValue((Event)extractedContent.Event),
                        DocUploadDate = DateTime.Now
                    }
                ));
            }
            
            return Ok(true);
        }
        
        [HttpPost("ProcessImportedDocuments")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<DocumentDto>))]
        public IActionResult ProcessImportedDocuments([FromBody] DocumentDto[] documentDtos)
        {
            if(documentDtos.Length > 0)
            {
                for (int i = 0; i < documentDtos.Length;i++)
                {
                    string webAddress = GetDocumentPathFromNetworkFile(documentDtos[i].DocumentPath);
                    if (!System.IO.File.Exists(webAddress))
                    {
                        System.IO.File.Copy(documentDtos[i].DocumentPath, webAddress);
                    }

                    if(System.IO.File.Exists(webAddress))
                    {
                        documentDtos[i].IsUploadedDocument = 1;
                        documentDtos[i].IsProcessedDocument = 1;
                        documentDtos[i].DocumentPath = webAddress;
                        
                        var content = DocumentExtractor.Extract(documentDtos[i].DocumentPath);
                        if (!string.IsNullOrEmpty(content))
                        {
                            documentDtos[i].DocumentContent = content;
                            documentDtos[i].ContentExtracted = "1";
                        }
                    }
                }
            }
            
            for (int i = 0; i < documentDtos.Length; i++)
            {
                documentDtos[i].UserName = _userRepository.GetUserById(documentDtos[i].DocumentAuthor)?.UserName;
                var doc = _documentRepository.UpdateDocument(EntityMapper.MapDocumentDto(documentDtos[i]));
                documentDtos[i] = EntityMapper.MapDocument(doc);
            }

            foreach (var bulkExtract in _bulkExtractRepository.GetBulkExtracts())
            {
                var initialProcessedDocument = bulkExtract.processed_documents;
                bulkExtract.processed_documents = initialProcessedDocument + documentDtos.Count(a=>a.BulkExtractId == bulkExtract.bulk_extract_id && a.IsProcessedDocument == 1);
                bulkExtract.uploaded_documents = bulkExtract.total_documents - bulkExtract.processed_documents;
                
                _bulkExtractRepository.UpdateBulkExtract(bulkExtract);
            }

            return Ok(documentDtos);
        }
        
        [ApiExplorerSettings(IgnoreApi=true)]
        private string GetDocumentPathFromNetworkFile(string param)
        {
            string[] splits = param.Split(new char[] { '/', '\\' });
            string filename = splits.Last();
            return networkStorePath + "/" + filename;
        }
    }
}
