using InventarySystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.DataAccess.Repository.IRepository
{
    public interface IKardexInventoryRepository : IRepository<KardexInventory>
    {
        Task RegisterKardex(int storeProductId, string type, string detail, int previousStock, int amount, string userId);
    }
}
