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
    public class InventoryDetailsConfiguration : IEntityTypeConfiguration<InventoryDetails>
    {
        public void Configure(EntityTypeBuilder<InventoryDetails> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.InventoryId).IsRequired();
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.PreviousStock).IsRequired();
            builder.Property(x => x.Amount).IsRequired();

            /* Relationship*/
            builder.HasOne(x => x.Inventory).WithMany()
                .HasForeignKey(x => x.InventoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Product).WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
