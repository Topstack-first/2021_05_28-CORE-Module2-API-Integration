using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.ViewModel.Well
{
    public class SaveWellVM
    {
        public int WellId { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public string Alias { get; set; }
        public string BlockName { get; set; }
        public string FieldName { get; set; }
        public string BasinName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal XEasting { get; set; }
        public decimal YNorthing { get; set; }
        public DateTime SpudDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public decimal TD { get; set; }
        public string WellType { get; set; }
        public decimal MDDF { get; set; }
        public decimal TVDDF { get; set; }

    }
}
