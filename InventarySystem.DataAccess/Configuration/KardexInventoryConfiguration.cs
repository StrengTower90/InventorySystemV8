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
    public class KardexInvetoryConfiguration : IEntityTypeConfiguration<KardexInventory>
    {
        public void Configure(EntityTypeBuilder<KardexInventory> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.StoreProductId).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Details).IsRequired();
            builder.Property(x => x.PreviousStock).IsRequired();
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.Cost).IsRequired();
            builder.Property(x => x.Stock).IsRequired();
            builder.Property(x => x.Total).IsRequired();
            builder.Property(x => x.UserApplicationId).IsRequired();
            builder.Property(x => x.RegisterDate).IsRequired();


            /* Relationship*/
            builder.HasOne(x => x.StoreProduct).WithMany()
                 .HasForeignKey(x => x.StoreProductId)
                 .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.UserApplication).WithMany()
                .HasForeignKey(x => x.UserApplicationId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
