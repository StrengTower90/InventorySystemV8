using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.Models
{
    public class UserApplication : IdentityUser
    {
        [Required(ErrorMessage ="Name is required")]
        [MaxLength(80)]
        public string Name { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        [MaxLength(80)]
        public string LastNames { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(200)]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [MaxLength(60)]
        public string City { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [MaxLength(60)]
        public string Country { get; set; }

        [NotMapped] // this DataAnnotation set that this props doesn't allow to the database oly as a reference
        public string Role { get; set; }
    }
}
