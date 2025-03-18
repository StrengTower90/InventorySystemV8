using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserApplicationId { get; set; }

        [ForeignKey("UserApplicationId")]
        public UserApplication UserApplication { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime SendDate { get; set; }
        public string ShippingNumber { get; set; }
        public string Carrier { get; set; }

        [Required]
        public double TotalOrder { get; set; }

        [Required]
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime MaximumPaymentDate { get; set; }
        public string TransactionId { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ClientNames { get; set; }


    }
}
