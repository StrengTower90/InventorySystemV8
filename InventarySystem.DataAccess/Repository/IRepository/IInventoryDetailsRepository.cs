using InventarySystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.DataAccess.Repository.IRepository
{
    public interface IInventoryDetailsRepository : IRepository<InventoryDetails>
    {
        void Update(InventoryDetails inventoryDetails);

    }
}
