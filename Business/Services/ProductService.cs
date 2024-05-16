﻿using Business.Models;
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
        public interface IProductService
        {
            bool Add(ProductModel model);
        bool Delete(int id);
        bool Update(ProductModel model);
        IQueryable<ProductModel> Query();
        }
     public class ProductService : BaseService, IProductService
        {
        public ProductService(Db db) : base(db)
        {
        }

        public List<Product> GetProductsForUser(int userId)
        {
            var products = _db.Products
                .Where(p => p.Userid == userId)
                .ToList();

            return products;
        }

        public bool Add(ProductModel product)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == product.UserId);
            if (user == null)
            {
                return false;
            }
            var existingCategories = _db.Categories.Where(c => product.CategoryIdsInput.Contains(c.Id)).ToList();
            foreach(var c in existingCategories)
            {
                Console.WriteLine(c.CategoryName);
            }
            var productEntity = new Product()
            {
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                User = user,
            };
            foreach (var category in existingCategories)
            {
                var productCategory = new ProductCategory
                {
                    Product = productEntity,
                    Category = category
                };
                productEntity.ProductCategories.Add(productCategory);
            }

            _db.Products.Add(productEntity);
            _db.SaveChanges();
            return true;

        }

        public IQueryable<ProductModel> Query()
        {
            return _db.Products
                .OrderByDescending(e => e.ProductName)
                .Select(e => new ProductModel()
                {
                    Id = e.Id,
                    ProductName = e.ProductName,
                    ProductDescription = e.ProductDescription,
                    UserId = e.Userid,
                    CategoryIdsInput = e.ProductCategories.Select(pc => pc.CategoryId).ToList(),
                    CategoryNamesOutput = e.ProductCategories.Select(pc => pc.Category.CategoryName).ToList(),
                });
        }

        public bool Delete(int id) {
            var productEntity = _db.Products.FirstOrDefault(p => p.Id == id);
            _db.Products.Remove(productEntity);
            _db.SaveChanges();

            return true;
        }
        public bool Update(ProductModel model)
        {
            var productEntity = _db.Products
                .Include(p => p.ProductCategories) // Include ProductCategories navigation property
                .FirstOrDefault(p => p.Id == model.Id);

            if (productEntity == null)
            {
                return false; // Product not found
            }

            productEntity.ProductName = model.ProductName;
            productEntity.ProductDescription = model.ProductDescription;

            var categoriesToRemove = productEntity.ProductCategories
                .Where(pc => !model.CategoryIdsInput.Contains(pc.CategoryId))
                .ToList();

            foreach (var categoryToRemove in categoriesToRemove)
            {
                productEntity.ProductCategories.Remove(categoryToRemove);
            }

            foreach (var categoryId in model.CategoryIdsInput)
            {
                if (!productEntity.ProductCategories.Any(pc => pc.CategoryId == categoryId))
                {
                    var productCategory = new ProductCategory
                    {
                        Product = productEntity,
                        CategoryId = categoryId,
                    };
                    _db.ProductCategories.Add(productCategory);
                }
            }

            _db.SaveChanges();

            return true;
        }


    }
}
