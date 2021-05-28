using BeicipFranLabERP.BL.Repository.AdminSetting;
using BeicipFranLabERP.DAL.Model;
using BeicipFranLabERP.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CORE.Models;

namespace BeicipFranLabERP.DAL.Services.AdminSetting
{
    public class EmailTemplateDAL : IEmailTemplate

    {
        private COREContext _context;
        public EmailTemplateDAL(COREContext appDbContext)
        {
            _context = appDbContext;
        }

        public Task<EmailTeplateElementVM> GetEmailTemplate()
        {
            var list = _context.CORE_Email_Templates.Select(a => new EmailTeplateElementVM
            {
                DateAutoMatchRegexes = a.DateAutoMatchRegexes,
                EmailBody = a.EmailBody,
                EmailSignBottomPartoftheEmailBody = a.EmailSignBottomPartoftheEmailBody,
                EmailSubject = a.EmailSubject,
                EmailTemplateName = a.EmailTemplateName
            }).FirstOrDefaultAsync();
            return list;
        }

       
        public async Task<string> SaveEmailTemplate(EmailTeplateElementVM model)
        {
            var isFirstEntry = _context.CORE_Email_Templates.FirstOrDefault();
            var obj = isFirstEntry == null ? new CORE_Email_Template() : isFirstEntry;

            obj.DateAutoMatchRegexes = model.DateAutoMatchRegexes;
            obj.EmailBody = model.EmailBody;
            obj.EmailSignBottomPartoftheEmailBody = model.EmailSignBottomPartoftheEmailBody;
            obj.EmailSubject = model.EmailSubject;
            obj.EmailTemplateName = model.EmailTemplateName;
            if (isFirstEntry == null)
            {
                _context.CORE_Email_Templates.Add(obj);
            }
            await _context.SaveChangesAsync();
            return isFirstEntry == null? "Email template is Added Successfully":"Email Template is Updated Successfully";
        }

    }
}
