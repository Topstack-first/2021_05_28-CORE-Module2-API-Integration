using BeicipFranLabERP.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.BL.Repository.AdminSetting
{
  public   interface IEmailTemplate
    {


        Task<string> SaveEmailTemplate(EmailTeplateElementVM model);
        Task<EmailTeplateElementVM> GetEmailTemplate();

    }
}
