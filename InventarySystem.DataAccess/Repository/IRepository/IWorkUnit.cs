using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.DataAccess.Repository.IRepository
{
    public interface IWorkUnit : IDisposable // this class allows to release any resource unused in the system
    {
        IStoreRepository Store { get; }
        ICategoryRepository Category { get;  }
        IBrandRepository Brand { get; }
        IProductRepository Product { get; }
        IUserApplicationRepository UserApplication { get; }
        IStoreProductRepository StoreProduct { get; }
        IInventoryRepository Inventory { get; }
        IInventoryDetailsRepository InventoryDetails { get; }
        IKardexInventoryRepository KardexInventory { get; }
        ICompanyRepository Company { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IOrderRepository Order { get;  }
        IOrderDetailRepository OrderDetail { get; }
        Task Save();
    }
}
