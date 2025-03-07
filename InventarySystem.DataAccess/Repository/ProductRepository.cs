using InventarySystem.DataAccess.Data;
using InventarySystem.DataAccess.Repository.IRepository;
using InventarySystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> RetrieveAllDropdownList(string obj)
        {
            if(obj == "Category")
            {
                return _db.Category.Where(c => c.State == true).Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
                
            }
            if(obj == "Brand")
            {
                return _db.Brand.Where(c => c.State == true).Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
            }
            if(obj == "Product")
            {
                return _db.Product.Where(c => c.State == true).Select(c => new SelectListItem
                {
                    Text = c.Description,
                    Value = c.Id.ToString()
                });
            }
            return null;
        }

        public void Update(Product product)
        {
            var productDB = _db.Product.FirstOrDefault(b => b.Id == product.Id);
            if(productDB != null)
            {
                if(product.ImageUrl != null)
                {
                    productDB.ImageUrl = product.ImageUrl;
                }
                productDB.SerialNumber = product.SerialNumber;
                productDB.Description = product.Description;
                productDB.Price = product.Price;
                productDB.Cost = product.Cost;
                productDB.CategoryId = product.CategoryId;
                productDB.BrandId = product.BrandId;
                productDB.ParentId = product.ParentId;
                productDB.State = product.State;

                _db.SaveChanges();
            }
        }
    }
}
