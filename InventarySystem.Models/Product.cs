using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Serial Number is required")]
        [MaxLength(60)]
        public string SerialNumber { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(60)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Cost is required")]
        public double Cost { get; set; }

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "State is required")]
        public bool State { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Required(ErrorMessage = "Brand is required")]
        public int BrandId { get; set; }

        [ForeignKey("BrandId")]
        public Brand Brand { get; set; }

        //Recursivity Relationship
        public int? ParentId { get; set; }// "?" indicate that it's encforcing to store null in the database
        public virtual Product Parent { get; set; }  /* virtual instead using ForeingKey Attribute to specify the recursivity the name must be equal 
                                                      * like the key ParentId to Parent without Id */
    }
}
