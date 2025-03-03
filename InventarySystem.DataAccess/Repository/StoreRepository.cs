using InventarySystem.DataAccess.Data;
using InventarySystem.DataAccess.Repository.IRepository;
using InventarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.DataAccess.Repository
{
    public class StoreRepository : Repository<Store>, IStoreRepository
    {
        private readonly ApplicationDbContext _db;

        public StoreRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Store store)
        {
            var storeBD = _db.Stores.FirstOrDefault(b => b.Id == store.Id);
            if(storeBD != null)
            {
                storeBD.Name = store.Name;
                storeBD.Description = store.Description;
                storeBD.State = store.State;
                _db.SaveChanges();
            }
        }
    }
}
