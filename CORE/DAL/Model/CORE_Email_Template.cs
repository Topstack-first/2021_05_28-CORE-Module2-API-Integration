using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.DAL.Model
{
    public class CORE_Email_Template
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CORE_Email_TemplateId { get; set; }
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
