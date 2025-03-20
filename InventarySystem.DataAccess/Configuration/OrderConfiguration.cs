using InventarySystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.DataAccess.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.UserApplicationId).IsRequired();
            builder.Property(x => x.OrderDate).IsRequired();
            builder.Property(x => x.TotalOrder).IsRequired();
            builder.Property(x => x.OrderStatus).IsRequired();
            builder.Property(x => x.PaymentStatus).IsRequired();
            builder.Property(x => x.ClientNames).IsRequired();
            builder.Property(x => x.ShippingNumber).IsRequired(false);
            builder.Property(x => x.Carrier).IsRequired(false);
            builder.Property(x => x.TransactionId).IsRequired(false);
            builder.Property(x => x.SessionId).IsRequired(false);
            builder.Property(x => x.Telephone).IsRequired(false);
            builder.Property(x => x.Address).IsRequired(false);
            builder.Property(x => x.City).IsRequired(false);
            builder.Property(x => x.Country).IsRequired(false);

            /* Relationships */
            builder.HasOne(x => x.UserApplication).WithMany()
                 .HasForeignKey(x => x.UserApplicationId)
                 .OnDelete(DeleteBehavior.NoAction);           
        }
    }
}
