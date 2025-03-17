using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.Models.ViewModels
{
    public class InventoryVM
    {
        public Inventory Inventory { get; set; }
        public InventoryDetails InventoryDetail { get; set; }
        public IEnumerable<InventoryDetails> InventoryDetails { get; set; }
        public IEnumerable<SelectListItem> StoreList { get; set; }
    }
}
