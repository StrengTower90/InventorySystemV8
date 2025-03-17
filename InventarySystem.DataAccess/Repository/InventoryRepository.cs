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
    public class InventoryRepository : Repository<Inventory>, IInventoryRepository
    {
        private readonly ApplicationDbContext _db;

        public InventoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> RetrieveAllDropdownList(string obj)
        {
            if(obj == "Store")
            {
                return _db.Stores.Where(b => b.State == true).Select(b => new SelectListItem
                {
                    Text = b.Name,
                    Value = b.Id.ToString()
                });

            }
            return null;
        }

        public void Update(Inventory inventory)
        {
            var inventoryDB = _db.Inventories.FirstOrDefault(b => b.Id == inventory.Id);
            if(inventoryDB != null)
            {
                inventoryDB.StoreId = inventory.StoreId;
                inventoryDB.EndDate = inventory.EndDate;
                inventoryDB.Store = inventory.Store;

                _db.SaveChanges();
            }
        }
    }
}
