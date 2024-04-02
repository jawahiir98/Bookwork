using BookWork.DataAccess.Data;
using BookWork.DataAccess.Repository.IRepository;
using BookWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWork.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext db;
        public ProductRepository(ApplicationDbContext _db) : base(_db)
        {
            db = _db;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Product product)
        {
            var obj = db.Products.FirstOrDefault(u => u.Id == product.Id);
            if(obj != null)
            {
                obj.Title = product.Title;
                obj.Description = product.Description;
                obj.Price = product.Price;
                obj.ListPrice = product.ListPrice;
                obj.ISBN = product.ISBN;
                obj.Description = product.Description;
                obj.Author = product.Author;
                obj.Price50 = product.Price50;
                obj.Price100 = product.Price100;
                obj.Category = product.Category;
                obj.CategoryId = product.CategoryId;
                if(obj.ImageUrl != null)
                {
                    obj.ImageUrl = product.ImageUrl;
                }
            }
        }
    }
}
