using BeicipFranLabERP.ViewModel.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.BL.Repository.Administrator
{
    public interface ICategory
    {
         Task<string> SaveCategory(SaveCategoryVM category);

         Task<string> DeleteCategory(int id);

         Task<List<GetCategoriesVM>> GetCategory(int id);
       

    }
}
