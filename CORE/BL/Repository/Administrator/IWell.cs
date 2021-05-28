using BeicipFranLabERP.ViewModel.Well;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.BL.Repository.Administrator
{
    public interface IWell
    {
        Task<string> SaveWell(SaveWellVM well);

        Task<string> DeleteWell(int id);

        Task<List<GetWellVM>> GetWell(int id);
    }
}
