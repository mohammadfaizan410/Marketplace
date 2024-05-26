using Business.Models;
using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface ICategoryService
    {
        public List<Category> GetAllCategories();
        public IQueryable<CategoryModel> Query();
        public void RemoveCategory(int id);
        public void AddCategory(CategoryModel category);
        public void UpdateCategory(CategoryModel category);  
    }
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(Db db) : base(db)
        {


        }

        public IQueryable<CategoryModel> Query()
        {
            return _db.Categories
                .Select(e => new CategoryModel()
                {
                    Id = e.Id,
                    CategoryName = e.CategoryName
                });
        }
        public List<Category> GetAllCategories()
        {
            return _db.Categories.ToList();
        }

        public void AddCategory(CategoryModel categoryModel)
        {
            var catEntity = new Category()
            {
                CategoryName = categoryModel.CategoryName,
            };
            _db.Categories.Add(catEntity);
            _db.SaveChanges();
        }

        public void RemoveCategory(int id)
        {
            var catEntity = _db.Categories.FirstOrDefault(r => r.Id == id);
            _db.Categories.Remove(catEntity);
            _db.SaveChanges();
        }

        public void UpdateCategory(CategoryModel cat)
        {
            var catEntity = _db.Categories.FirstOrDefault(r => r.Id == cat.Id);
            catEntity.CategoryName = cat.CategoryName;
            _db.Update(catEntity);
            _db.SaveChanges();

        }
    }
}
