using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.DAL.Model
{
    public class PortalSetting
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PortalSettingId { get; set; }
        [Required]
        public string CompanyLogo { get; set; }
        [Required]
        public string PortalLogo { get; set; }
        [Required]
        public string Favicon { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string LoginPageBackground { get; set; }
        [Required]
        public string SiteTitle { get; set; }
        [Required]
        public string CorePortalShortName { get; set; }
    }
}
