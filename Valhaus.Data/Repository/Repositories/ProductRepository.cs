using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valhaus.Data.Data;
using Valhaus.Data.Repository.IRepository;
using Valhaus.Models;
using Valhaus.Models.Models;

namespace Valhaus.Data.Repository.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }

        public void Update(Product product)
        {
            //var db_product = _db.Products.FirstOrDefault(p => p.Id == product.Id);
            //if (db_product is null)
            //    return;

            //db_product.Title        = product.Title;
            //db_product.Description  = product.Description;
            //db_product.SKU          = product.SKU;
            //db_product.CategoryId   = product.CategoryId;
            //db_product.ListPrice    = product.ListPrice;
            //db_product.Price        = product.Price;
            //db_product.Price50      = product.Price50;
            //db_product.Price100     = product.Price100;
            //db_product.ImageUrl     = product.ImageUrl;

            _db.Products.Update(product);

        }
    }
}
