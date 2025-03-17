using InventarySystem.DataAccess.Data;
using InventarySystem.DataAccess.Repository.IRepository;
using InventarySystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.DataAccess.Repository
{
    public class KardexInventoryRepository : Repository<KardexInventory>, IKardexInventoryRepository
    {
        private readonly ApplicationDbContext _db;

        public KardexInventoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task RegisterKardex(int storeProductId, string type, string detail, int previousStock, int amount, string userId)
        {
            var storeProduct = await _db.StoresProducts.Include(b => b.Product).FirstOrDefaultAsync(b => b.Id == storeProductId);

            if(type=="Entry")
            {
                KardexInventory Kardex = new KardexInventory();
                Kardex.StoreProductId = storeProductId;
                Kardex.Type = type;
                Kardex.Details = detail;
                Kardex.PreviousStock = previousStock;
                Kardex.Amount = amount;
                Kardex.Cost = storeProduct.Product.Cost;
                Kardex.Stock = previousStock + amount;
                Kardex.Total = Kardex.Stock * Kardex.Cost;
                Kardex.UserApplicationId = userId;
                Kardex.RegisterDate = DateTime.Now;

                await _db.KardexInventory.AddAsync(Kardex);
                await _db.SaveChangesAsync();
            }
            if (type == "Exit")
            {
                KardexInventory Kardex = new KardexInventory();
                Kardex.StoreProductId = storeProductId;
                Kardex.Type = type;
                Kardex.Details = detail;
                Kardex.PreviousStock = previousStock;
                Kardex.Amount = amount;
                Kardex.Cost = storeProduct.Product.Cost;
                Kardex.Stock = previousStock - amount;
                Kardex.Total = Kardex.Stock * Kardex.Cost;
                Kardex.UserApplicationId = userId;
                Kardex.RegisterDate = DateTime.Now;

                await _db.KardexInventory.AddAsync(Kardex);
                await _db.SaveChangesAsync();
            }
        }
    }
}
