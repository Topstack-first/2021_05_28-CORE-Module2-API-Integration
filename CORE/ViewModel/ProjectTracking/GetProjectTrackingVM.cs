using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.ViewModel.ProjectTracking
{
    public class GetProjectTrackingVM
    {
        
        public string ProjectName { get; set; }
        
        public string MilestoneName { get; set; }
    
        public DateTime MilestoneDate { get; set; }
     
        public int DaystoML { get; set; }
       
        public int ExistingPercentage { get; set; }
       
        public int ProposedPersentage { get; set; }
    }
}
