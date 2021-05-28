using BeicipFranLabERP.BL.Repository.Administrator;
using BeicipFranLabERP.ViewModel.Department;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using CORE.Models;

namespace BeicipFranLabERP.DAL.Services.Administrator
{
    
    public class DepartmentDAL: IDepartment
    {
        private COREContext _context;
        public DepartmentDAL(COREContext appDbContext)
        {
            _context = appDbContext;
        }

        public string GetCurrentUserId()
        {
            return "Test123";
        }
        public async Task<string> SaveDepartment(SaveDepartmentVM dpt)
        {
            string res = "";
            if (dpt.DepartmentId > 0)
            {
                var _c = await _context.Departments.Where(x => x.DepartmentId == dpt.DepartmentId).FirstOrDefaultAsync();
                _c.Name = dpt.Name;
                _c.IconUrl = dpt.IconUrl;
                _c.Alias = dpt.Alias;
                _c.CreatedBy = GetCurrentUserId();
                _c.CreatedDate = Utility.Now();
                _context.Departments.Update(_c);
                await _context.SaveChangesAsync();
                res = "Sucessfully Updated";
            }
            else
            {
                var _c = new Department();
                _c.Name = dpt.Name;
                _c.IconUrl = dpt.IconUrl;
                _c.Alias = dpt.Alias;
                _c.CreatedBy = GetCurrentUserId();
                _c.CreatedDate = Utility.Now();
                _context.Departments.Add(_c);
                await _context.SaveChangesAsync();
                res = "Sucessfully Added";
            }
            return res;
        }
        public async Task<string> DeleteDepartment(int id)
        {
            string res = "";
            var _c = await _context.Departments.Where(x => x.DepartmentId == id).FirstOrDefaultAsync();
            if (_c != null)
            {
                _context.Departments.Remove(_c);
                await _context.SaveChangesAsync();
                res = "Sucessfully Deleted";
            }
            else
            {
                res = "Invalid Department";
            }
            return res;
        }
        public async Task<List<GetDepartmentsVM>> GetDepartment(int id)
        {
            var _c = new List<Department>();
            if (id > 0)
                _c = await _context.Departments.Where(x => x.DepartmentId == id).ToListAsync();
            else
                _c = await _context.Departments.ToListAsync();


            var transForm = _c.AsEnumerable().Select(x => new GetDepartmentsVM
            {
                Alias = x.Alias,
                IconUrl = x.IconUrl,
                Name = x.Name,
                DepartmentId=x.DepartmentId

            }).ToList();
            return transForm;
        }

    }
}
