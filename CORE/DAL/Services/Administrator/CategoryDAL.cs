using BeicipFranLabERP.BL.Repository.Administrator;
using BeicipFranLabERP.DAL.Model;
using BeicipFranLabERP.ViewModel.Category;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CORE.Models;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.DAL.Services.Administrator
{
    public class CategoryDAL: ICategory
    {
        private COREContext _context;
        public CategoryDAL(COREContext appDbContext)
        {
            _context = appDbContext;
        }

        public string GetCurrentUserId()
        {
            return "Test123";
        }
        public async Task<string> SaveCategory(SaveCategoryVM category)
        {
            string res = "";
           
                if (category.CategoryId > 0)
                {
                    var _c = await _context.Categories.Where(x => x.CategoryId == category.CategoryId).FirstOrDefaultAsync();
                    _c.BackgroundColor = category.BackgroundColor;
                    _c.IconUrl = category.IconUrl;
                    _c.Name = category.Name;
                    _c.ShortCode = category.ShortCode;
                    _c.TextColor = category.TextColor;
                    _c.BackgroundColor = category.BackgroundColor;
                    _c.CreatedBy = GetCurrentUserId();
                    _c.CreatedDate = Utility.Now();
                    _context.Categories.Update(_c);
                    await _context.SaveChangesAsync();
                    res = "Sucessfully Updated";
                }
                else
                {
                    var _c = new Category();
                _c.BackgroundColor = category.BackgroundColor;
                _c.IconUrl = category.IconUrl;
                _c.Name = category.Name;
                _c.ShortCode = category.ShortCode;
                _c.TextColor = category.TextColor;
                _c.BackgroundColor = category.BackgroundColor;
                _c.CreatedBy = GetCurrentUserId();
                _c.CreatedDate = Utility.Now();
                _context.Categories.Add(_c);
                    await _context.SaveChangesAsync();
                    res = "Sucessfully Added";
                }
           
            return res;
        }
        public async Task<string> DeleteCategory(int id)
        {
            string res = "";
            var _c = await _context.Categories.Where(x => x.CategoryId == id).FirstOrDefaultAsync();
            if (_c != null)
            {
                _context.Categories.Remove(_c);
                await _context.SaveChangesAsync();
                res = "Sucessfully Deleted";
            }
            else
            {
                res = "Invalid Category";
            }
            return res;
        }
        public async Task<List<GetCategoriesVM>> GetCategory(int id)
        {
            var _c = new List<Category>();
            if (id > 0)
            {
                _c =await  _context.Categories.Where(x => x.CategoryId == id).ToListAsync();
            }
            else
            {
                _c = await _context.Categories.ToListAsync();
            }

            var transForm = _c.AsEnumerable().Select(x => new GetCategoriesVM
            {
                BackgroundColor = x.BackgroundColor,
                CategoryId = x.CategoryId,
                IconUrl = x.IconUrl,
                Name = x.Name,
                ShortCode = x.ShortCode,
                TextColor = x.TextColor

            }).ToList();
            return transForm;
        }

    }
}
