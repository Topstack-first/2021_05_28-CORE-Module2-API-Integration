using BeicipFranLabERP.BL.Repository.Administrator;
using BeicipFranLabERP.DAL.Model;
using BeicipFranLabERP.ViewModel.HealthCheckup;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CORE.Models;

namespace BeicipFranLabERP.DAL.Services.Maintenance
{
    public class HealthDAL : IHealth
    {
        private COREContext _context;
        public HealthDAL(COREContext appDbContext)
        {
            _context = appDbContext;
        }

        public string GetCurrentUserId()
        {
            return "Test123";
        }

        public async Task<List<GetHealthCheckUpVM>> GetBrokenLinks(int id)
        {
            var _docx = await _context.HealthCheckups.ToListAsync();
            var _t = new List<GetHealthCheckUpVM>();

            foreach (var item in _docx)
            {
                if (File.Exists(item.Document_path))
                {

                }
                else
                {
                    var c = new GetHealthCheckUpVM();
                    c.Department = item.Department_name;
                    c.DocumentTitle = item.Document_title;
                    c.Id = item.Id;
                    c.Path = item.Document_path;
                    c.Status = item.Document_status;
                    _t.Add(c);
                }
            }

            return _t;
        }
        public async Task<List<GetHealthCheckUpVM>> GetSpecialCharacters(int id)
        {
            var _coreSet = await _context.AdministratorSettings.FirstOrDefaultAsync();
            int maxPath = _coreSet.DocumentPathMaxLength;
            var _docx = await _context.HealthCheckups.Where(x => x.Document_title.Contains(_coreSet.InsertSpecialCharacters)).ToListAsync();
            var transForm = _docx.AsEnumerable().Select(x => new GetHealthCheckUpVM
            {

                Department = x.Department_name,
                DocumentTitle = x.Document_title,
                Id = x.Id,
                Path = x.Document_path,
                Status = x.Document_status
            }).ToList();


            return transForm;
        }

        public async Task<List<GetHealthCheckUpVM>> GetDocumentWithoutContent(int id)
        {
            var _docx = await _context.HealthCheckups.ToListAsync();
            var _t = new List<GetHealthCheckUpVM>();

            foreach (var item in _docx)
            {
                if (File.Exists(item.Document_path))
                {
                    long length = new System.IO.FileInfo(item.Document_path).Length;

                    if (length > 1)
                    {

                    }
                    else
                    {
                        var c = new GetHealthCheckUpVM();
                        c.Department = item.Department_name;
                        c.DocumentTitle = item.Document_title;
                        c.Id = item.Id;
                        c.Path = item.Document_path;
                        c.Status = item.Document_status;
                        _t.Add(c);
                    }

                }
                else
                {
                
                }
            }

            return _t;
        }

        public async Task<List<GetHealthCheckUpVM>> GetDuplicateDocuments(int id)
        {
            var _docx = await _context.HealthCheckups.ToListAsync();

            IEnumerable<HealthCheckup> duplicates = from x in _docx
                                                    group x by x into g
                                                    where g.Count() > 1
                                                    select g.Key;
            var transForm = duplicates.AsEnumerable().Select(x => new GetHealthCheckUpVM
            {

                Department = x.Department_name,
                DocumentTitle = x.Document_title,
                Id = x.Id,
                Path = x.Document_path,
                Status = x.Document_status
            }).ToList();

            return transForm;
        }
        public async Task<List<GetHealthCheckUpVM>> GetTooLongPaths(int id)
        {
            var _coreSet = await _context.AdministratorSettings.FirstOrDefaultAsync();
            int maxPath = _coreSet.DocumentPathMaxLength;
            var _docx = await _context.HealthCheckups.Where(x => x.Document_path.Length > maxPath).ToListAsync();
            var transForm = _docx.AsEnumerable().Select(x => new GetHealthCheckUpVM
            {

                Department = x.Department_name,
                DocumentTitle = x.Document_title,
                Id = x.Id,
                Path = x.Document_path,
                Status = x.Document_status
            }).ToList();


            return transForm;
        }

    }
}
