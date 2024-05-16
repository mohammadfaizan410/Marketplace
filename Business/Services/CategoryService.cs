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
    }
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(Db db) : base(db)
        {
        }

        public List<Category> GetAllCategories()
        {
            return _db.Categories.ToList();
        }
    }
}
