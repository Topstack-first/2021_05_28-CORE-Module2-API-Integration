using BeicipFranLabERP.BL.Repository.AdminSetting;
using BeicipFranLabERP.DAL.Model;
using BeicipFranLabERP.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CORE.Models;

namespace BeicipFranLabERP.DAL.Services.AdminSetting
{
    public class CorePortalDAL : ICoreSetting
    {
        private COREContext _context;
        public CorePortalDAL(COREContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<CoreSettingVM> GetCoreSettings()
        {

            var obj = await _context.AdministratorSettings.Select(a => new CoreSettingVM
            {
                DefaultCategory = a.DefaultCategory,
                DefaultEvent = a.DefaultEvent,
                DefaultLocation = a.DefaultLocation,
                DefaultSubcategory = a.DefaultSubcategory,
                DefaultWell = a.DefaultWell,
                DocumentPathMaxLength = a.DocumentPathMaxLength,
                DuplicateDocumentDetection = a.DuplicateDocumentDetection,
                InsertSpecialCharacters = a.InsertSpecialCharacters,
                MainFolderOnNetwork = a.MainFolderOnNetwork,
                MaxDocumentSize = a.MaxDocumentSize,
                MinCharsforExtractedDocContent = a.MinCharsforExtractedDocContent,
              
            }).FirstOrDefaultAsync();
            return  obj;
        }

       

        public async Task<string> SaveCoreSetting(CoreSettingVM model)
        {

            var isFirstEntry = _context.AdministratorSettings.FirstOrDefault();
            var op_type = isFirstEntry == null ? "add" : "update";

            var obj = new AdministratorSetting();
            if (op_type == "update")
            {
                obj = isFirstEntry;
            }
            obj.MainFolderOnNetwork = model.MainFolderOnNetwork;
            obj.DuplicateDocumentDetection = model.DuplicateDocumentDetection;
            obj.InsertSpecialCharacters = model.InsertSpecialCharacters;
            obj.MaxDocumentSize = model.MaxDocumentSize;
            obj.MinCharsforExtractedDocContent = model.MinCharsforExtractedDocContent;
            obj.DefaultCategory = model.DefaultCategory;
            obj.DefaultSubcategory = model.DefaultSubcategory;
            obj.DefaultEvent = model.DefaultEvent;
            obj.DefaultLocation = model.DefaultLocation;
            obj.DefaultWell = model.DefaultWell;
            if (op_type == "add")
                _context.AdministratorSettings.Add(obj);
            await _context.SaveChangesAsync();
        
          var  response = op_type == "add" ? "Successfully Added" : "Successfully Updated";
            return response;

        }
    }
}
