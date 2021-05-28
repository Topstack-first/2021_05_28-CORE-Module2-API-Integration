using BeicipFranLabERP.ViewModel.StakeHolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.BL.Repository.Administrator
{
    public interface IStakeHolder
    {
        Task<string> SaveStakeHolder(SaveStakeHolderVM skh);
        
        Task<string> DeleteStakeHolder(int id);
        
        Task<List<GetStakeHolderVM>> GetStakeHolder(int id);
        

    }
}
