using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.Models.ViewModels
{
    public class CartShoppingVM
    {
        public Company Company { get; set; }
        public Product Product { get; set; }
        public int Stock { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public IEnumerable<ShoppingCart> ShoppingCartsList { get; set; }
        public Order Order { get; set; }
    }
}
