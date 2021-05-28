using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.DAL.Model
{
    public class SEIndex
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string DefaultOperator { get; set; }
        [Required]
        public string DefaultOrder { get; set; }
        [Required]
        public string KeyWordMatching { get; set; }
        [Required]
        public string Element { get; set; }
        [Required]
        public decimal Weight { get; set; }
        [Required]
        public string RecentPostBonusCutoff { get; set; }
        [Required]
        public string Synonyms { get; set; }
        [Required]
        public string StopwordsToAdd { get; set; }
        [Required]
        public string ExportableListOfStopwords { get; set; }
        [Required]
        public string ContentStopwordToAdd { get; set; }
        [Required]
        public string ExportableListOfContentStopwords { get; set; }
    }
}
