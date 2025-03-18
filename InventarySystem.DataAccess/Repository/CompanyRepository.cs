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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Company company)
        {
            var storeBD = _db.Company.FirstOrDefault(b => b.Id == company.Id);
            if(storeBD != null)
            {
                storeBD.Name = company.Name;
                storeBD.Description = company.Description;
                storeBD.Country = company.Country;
                storeBD.City = company.City;
                storeBD.Address = company.Address;
                storeBD.Telephone = company.Telephone;
                storeBD.StoreSaleId = company.StoreSaleId;
                storeBD.UpdatedById = company.UpdatedById;
                storeBD.UpdateDate = company.UpdateDate;
                _db.SaveChanges();
            }
        }
    }
}
