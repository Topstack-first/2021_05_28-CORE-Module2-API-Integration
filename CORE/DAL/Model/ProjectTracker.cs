using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.DAL.Model
{
    public class ProjectTracker
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string MilestoneName { get; set; }
        [Required]
        public DateTime MilestoneDate { get; set; }
        [Required]
        public int DaystoML { get; set; }
        [Required]
        public int ExistingPercentage { get; set; }
        [Required]
        public int ProposedPersentage { get; set; }

    }
}
