using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.Models
{
    public class Inventory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserApplicationId { get; set; }

        [ForeignKey("UserApplicationId")]
        public UserApplication UserApplication { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store Store { get; set; }

        [Required]
        public bool State { get; set; }
    }
}
