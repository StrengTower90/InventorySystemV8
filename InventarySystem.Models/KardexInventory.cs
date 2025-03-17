using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.Models
{
    public class KardexInventory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StoreProductId { get; set; }

        [ForeignKey("StoreProductId")]
        public StoreProduct StoreProduct { get; set; }

        [Required]
        [MaxLength(100)]
        public string Type { get; set; }

        [Required]
        public string Details { get; set; }

        [Required]
        public int PreviousStock { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public double Cost { get; set; }

        [Required]
        public int Stock { get; set; }

        public double Total { get; set; }

        [Required]
        public string UserApplicationId { get; set; }

        [ForeignKey("UserApplicationId")]
        public UserApplication UserApplication { get; set; }

        public DateTime RegisterDate { get; set; }
    }
}
