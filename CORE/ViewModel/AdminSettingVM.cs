using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.ViewModel
{
    public class CoreSettingVM
    {

        [Required]
        public string MainFolderOnNetwork { get; set; }

        [Required]
        public string DuplicateDocumentDetection { get; set; }

        [Required]
        public string InsertSpecialCharacters { get; set; }
        [Required]

        public int DocumentPathMaxLength { get; set; }
        [Required]
        public int MaxDocumentSize { get; set; }
        [Required]
        public int MinCharsforExtractedDocContent { get; set; }
        [Required]
        public string DefaultCategory { get; set; }
        [Required]
        public string DefaultSubcategory { get; set; }
        [Required]

        public string DefaultEvent { get; set; }

        [Required]
        public string DefaultLocation { get; set; }

        [Required]
        public string DefaultWell { get; set; }

    }



    public class SavePortalSettingVM {
    
        public IFormFile CompanyLogo { get; set; }
        
        public IFormFile PortalLogo { get; set; }
        [Required]
        public IFormFile Favicon { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public IFormFile LoginPageBackground { get; set; }
        [Required]
        public string SiteTitle { get; set; }
        [Required]
        public string CorePortalShortName { get; set; }


    }

    public class PortalSettingObjVM
    {
        public int Id { get; set; }

        public string CompanyLogo { get; set; }

        public string PortalLogo { get; set; }

        public string Favicon { get; set; }
        
        public string CompanyName { get; set; }
   
        public string LoginPageBackground { get; set; }
    
        public string SiteTitle { get; set; }
        
        public string CorePortalShortName { get; set; }


    }

    public class EmailTeplateElementVM
    {
        [Required]
        public string DateAutoMatchRegexes { get; set; }
        [Required]
        public string EmailTemplateName { get; set; }
        [Required]
        public string EmailSubject { get; set; }
        [Required]
        public string EmailBody { get; set; }
        [Required]
        public string EmailSignBottomPartoftheEmailBody { get; set; }
    }
}
