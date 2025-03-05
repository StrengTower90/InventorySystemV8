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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Category category)
        {
            var categoryBD = _db.Category.FirstOrDefault(b => b.Id == category.Id);
            if(categoryBD != null)
            {
                categoryBD.Name = category.Name;
                categoryBD.Description = category.Description;
                categoryBD.State = category.State;
                _db.SaveChanges();
            }
        }
    }
}
