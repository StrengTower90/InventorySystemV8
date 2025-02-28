using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.Models
{
    public class Store
    {
        [Key] //Data notation that establish the first propertie is the primary key
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")] // Will show a message in the view when the prop isn't present in the request
        [MaxLength(60, ErrorMessage = "the name must be 60 characters maximun")] // restricts the max-length allowed
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(100, ErrorMessage = "Description must to have a maximun of 100 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "State is required ")]
        public bool State { get; set; }
    }
}
