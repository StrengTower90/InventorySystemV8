﻿using InventarySystem.DataAccess.Data;
using InventarySystem.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.DataAccess.Repository
{
    public class WorkUnit : IWorkUnit
    {
        private readonly ApplicationDbContext _db;
        public IStoreRepository Store { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IBrandRepository Brand { get; private set; }
        public IProductRepository Product { get; private set; }
        public IUserApplicationRepository UserApplication { get; private set; }

        public WorkUnit(ApplicationDbContext db)
        {
            _db = db;
            Store = new StoreRepository(_db);
            Category = new CategoryRepository(_db);
            Brand = new BrandRepository(_db);
            Product = new ProductRepository(_db);
            UserApplication = new UserApplicationRepository(_db);
        }

        public void Dispose()
        {
            _db.Dispose(); // Release all date saved in memory
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
