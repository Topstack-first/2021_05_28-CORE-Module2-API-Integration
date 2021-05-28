using BeicipFranLabERP.BL.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.DAL.Services
{
    public class GeneralDAL : IGeneral
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;
        public GeneralDAL(  IWebHostEnvironment env
            , IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _env = env;
        }

        public string SaveImages(IFormFile received_file, string destination_folders)
        {
            string file_new_name = null;
            string newFileName = "";
            if (received_file != null)
            {
                newFileName= destination_folders+ received_file.FileName.Trim() + "-"+ Path.GetExtension(received_file.FileName);
                file_new_name = received_file.FileName.Trim() + "-" + Path.GetExtension(received_file.FileName);
                string path = Path.Combine(_env.WebRootPath, destination_folders + file_new_name);
                received_file.CopyTo(new FileStream(path, FileMode.Create));
            }
            return newFileName;
        }
        public string GetFileLocationWithWebRootPath(string file_new_name)
        {
            return Path.Combine(_env.WebRootPath,file_new_name);
        }
        public static class ImageFoldersPath_
        {
            public static string CompanyLogoImage = "Images/AdminSettings/CompanyLogo/";
            public static string PortalLogoImage = "Images/AdminSettings/PortalLogo/";
            public static string FaviconImage = "Images/AdminSettings/Favicon/";
            public static string LoginPageBg = "Images/AdminSettings/LoginPageBackground/";
        }
    }
}
