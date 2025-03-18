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
    public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.UserApplicationId).IsRequired();
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.Amount).IsRequired();

            /* Relationships */
            builder.HasOne(x => x.UserApplication).WithMany()
                 .HasForeignKey(x => x.UserApplicationId)
                 .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Product).WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
