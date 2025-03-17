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
    public class StoreProductRepository : Repository<StoreProduct>, IStoreProductRepository
    {
        private readonly ApplicationDbContext _db;

        public StoreProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(StoreProduct storeProduct)
        {
            var storeProductDB = _db.StoresProducts.FirstOrDefault(b => b.Id == storeProduct.Id);
            if(storeProductDB != null)
            {
                storeProductDB.Amount = storeProduct.Amount;

                _db.SaveChanges();
            }
        }
    }
}
