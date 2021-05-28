using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.ViewModel.Department
{
    public class SaveDepartmentVM
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string IconUrl { get; set; }
        public string UserId { get; set; }
    }
}
