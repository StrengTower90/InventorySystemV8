using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.Models.ViewModels
{
    public class OrderDetailVM
    {
        public Company Company { get; set; }
        public Order Order { get; set; }
        public IEnumerable<OrderDetail> OrderDetailList { get; set; }
    }
}
