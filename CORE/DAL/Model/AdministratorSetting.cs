using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.DAL.Model
{
    public class AdministratorSetting
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdministratorSettingId { get; set; }
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

        public string DefaultEvent  { get; set; }

        [Required]
        public string DefaultLocation { get; set; }

        [Required]
        public string DefaultWell { get; set; }









    }
}
