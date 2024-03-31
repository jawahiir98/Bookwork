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
            db.Update(product);
        }
    }
}
