using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(60, ErrorMessage = "Name must be a maximum of 60 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(100, ErrorMessage = "Description must be lower or equal to 60 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "State is required")]
        public bool State { get; set; }
    }
}
