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
    public class InventoryDetailsRepository : Repository<InventoryDetails>, IInventoryDetailsRepository
    {
        private readonly ApplicationDbContext _db;

        public InventoryDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(InventoryDetails inventoryDetails)
        {
            var inventoryDetailsDB = _db.InventoryDetails.FirstOrDefault(b => b.Id == inventoryDetails.Id);
            if(inventoryDetailsDB != null)
            {
                inventoryDetailsDB.PreviousStock = inventoryDetails.PreviousStock;
                inventoryDetailsDB.Amount = inventoryDetails.Amount;

                _db.SaveChanges();
            }
        }
    }
}
