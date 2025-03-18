using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(80)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(200)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [MaxLength(60)]
        public string Country { get; set; }

        [Required(ErrorMessage = "City is required")]
        [MaxLength(60)]
        public string City { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(100)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Telephone is required")]
        [MaxLength(60)]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Store of Sales is required")]
        public int StoreSaleId { get; set; }

        [ForeignKey("StoreSaleId")]
        public Store Store { get; set; }

        public string CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public UserApplication CreatedBy { get; set; }
        public string UpdatedById { get; set; }

        [ForeignKey("UpdatedById")]
        public UserApplication UpdatedBy { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
