using BeicipFranLabERP.BL.Repository;
using BeicipFranLabERP.BL.Repository.AdminSetting;
using BeicipFranLabERP.DAL.Model;
using BeicipFranLabERP.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BeicipFranLabERP.DAL.Services.GeneralDAL;
using CORE.Models;

namespace BeicipFranLabERP.DAL.Services.AdminSetting
{
    public class PortalSettingDAL : IPortalSetting
    {
        private IGeneral _generalServices;
        private COREContext _context;
        public PortalSettingDAL(COREContext appDbContext,IGeneral generalServices)
        {
            _generalServices = generalServices;
            _context = appDbContext;
        }

        public async Task<PortalSettingObjVM> GetPortalSettings()
        {
            var obj = await _context.PortalSettings.Select(a => new PortalSettingObjVM
            {
                Id = a.PortalSettingId,
                CompanyLogo = _generalServices.GetFileLocationWithWebRootPath(a.CompanyLogo),
                CompanyName = a.CompanyName,
                CorePortalShortName = a.CorePortalShortName,
                Favicon = _generalServices.GetFileLocationWithWebRootPath( a.Favicon),
                LoginPageBackground = _generalServices.GetFileLocationWithWebRootPath(a.LoginPageBackground),
                PortalLogo = _generalServices.GetFileLocationWithWebRootPath(a.PortalLogo),
                SiteTitle = a.SiteTitle
            }).FirstOrDefaultAsync();

            return obj;





        }

        public async Task<string> SavePortalSetting(SavePortalSettingVM model)
        {
            var isFirstEntry = _context.PortalSettings.FirstOrDefault();
            var op_type = isFirstEntry == null ? "add" : "update";

            var obj = new PortalSetting();
            if (op_type == "update")
            {
                obj = isFirstEntry;
            }

            if (op_type == "add")
            {
                obj.CompanyLogo = _generalServices.SaveImages(model.CompanyLogo, ImageFoldersPath_.CompanyLogoImage);
                obj.PortalLogo= _generalServices.SaveImages(model.PortalLogo, ImageFoldersPath_.PortalLogoImage);
                obj.Favicon = _generalServices.SaveImages(model.Favicon, ImageFoldersPath_.FaviconImage);
                obj.LoginPageBackground = _generalServices.SaveImages(model.LoginPageBackground, ImageFoldersPath_.LoginPageBg);
            }
            else
            {
                if (model.CompanyLogo != null)
                {
                    obj.CompanyLogo = _generalServices.SaveImages(model.CompanyLogo, ImageFoldersPath_.CompanyLogoImage);
                }
                if (model.PortalLogo != null)
                {
                    obj.PortalLogo = _generalServices.SaveImages(model.PortalLogo, ImageFoldersPath_.PortalLogoImage);
                }
                if (model.Favicon != null)
                {
                    obj.Favicon = _generalServices.SaveImages(model.Favicon, ImageFoldersPath_.FaviconImage);
                }
                if(model.LoginPageBackground!=null)
                {
                    obj.LoginPageBackground = _generalServices.SaveImages(model.LoginPageBackground, ImageFoldersPath_.LoginPageBg);
                }

            }
            obj.CompanyName = model.CompanyName;
            obj.SiteTitle = model.SiteTitle;
            obj.CorePortalShortName = model.CorePortalShortName;
            if (op_type == "add")
                _context.PortalSettings.Add(obj);
            await _context.SaveChangesAsync();
            var response = op_type == "add" ? "Successfully Added" : "Successfully Updated";
            return response;
        }
    }
}
