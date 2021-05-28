using BeicipFranLabERP.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.BL.Repository.AdminSetting
{
   public interface ICoreSetting
    {
        Task<string> SaveCoreSetting(CoreSettingVM model);
        Task<CoreSettingVM> GetCoreSettings();

    }
}
