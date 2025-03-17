using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.Models
{
    public class StoreProduct
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store Store { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int Amount { get; set; }


    }
}
