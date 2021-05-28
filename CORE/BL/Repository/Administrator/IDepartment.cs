using BeicipFranLabERP.ViewModel.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.BL.Repository.Administrator
{
    public interface IDepartment
    {
        Task<string> SaveDepartment(SaveDepartmentVM dpt);
       
        Task<string> DeleteDepartment(int id);
        
       Task<List<GetDepartmentsVM>> GetDepartment(int id);
        

    }
}
