using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.ViewModel.Category
{
    public class SaveCategoryVM
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string ShortCode { get; set; }
        public string IconUrl { get; set; }
        public string TextColor { get; set; }
        public string BackgroundColor { get; set; }
        public string UserId { get; set; }

    }
}
