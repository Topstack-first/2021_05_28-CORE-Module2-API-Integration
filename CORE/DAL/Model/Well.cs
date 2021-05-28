using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.DAL.Model
{
    public class Well
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int WellId { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [Required]
        [StringLength(250)]
        public string IconUrl { get; set; }
        [Required]
        [StringLength(250)]
        public string Alias { get; set; }
        [Required]
        [StringLength(250)]
        public string BlockName { get; set; }
        [Required]
        [StringLength(250)]
        public string FieldName { get; set; }
        [Required]
        [StringLength(250)]
        public string BasinName { get; set; }
        [Required]
        public decimal Latitude { get; set; }
        [Required]
        public decimal Longitude { get; set; }
        [Required]
        public decimal XEasting { get; set; }
        [Required]
        public decimal YNorthing { get; set; }
        [Required]
        public DateTime SpudDate { get; set; }
        [Required]
        public DateTime CompletionDate { get; set; }
        [Required]
        public decimal TD { get; set; }
        [Required]
        public string WellType { get; set; }
        [Required]
        public decimal MDDF { get; set; }
        [Required]
        public decimal TVDDF { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
