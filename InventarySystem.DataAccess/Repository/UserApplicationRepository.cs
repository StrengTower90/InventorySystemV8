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
    public class UserApplicationRepository : Repository<UserApplication>, IUserApplicationRepository
    {
        private readonly ApplicationDbContext _db;

        public UserApplicationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
