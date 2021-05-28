using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.ViewModel.Index
{
    public class GetIndexVM
    {
        public int Id { get; set; }

        public string DefaultOperator { get; set; }

        public string DefaultOrder { get; set; }

        public string KeyWordMatching { get; set; }

        public string Element { get; set; }

        public decimal Weight { get; set; }

        public string RecentPostBonusCutoff { get; set; }

        public string Synonyms { get; set; }

        public string StopwordsToAdd { get; set; }

        public string ExportableListOfStopwords { get; set; }

        public string ContentStopwordToAdd { get; set; }

        public string ExportableListOfContentStopwords { get; set; }
    }
}
