using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.ViewModel.Event
{
    public class SaveEventVM
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public string UserId { get; set; }
    }
}
