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
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        private readonly ApplicationDbContext _db;

        public BrandRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Brand brand)
        {
            var brandBD = _db.Brand.FirstOrDefault(b => b.Id == brand.Id);
            if(brandBD != null)
            {
                brandBD.Name = brand.Name;
                brandBD.Description = brand.Description;
                brandBD.State = brand.State;
                _db.SaveChanges();
            }
        }
    }
}
