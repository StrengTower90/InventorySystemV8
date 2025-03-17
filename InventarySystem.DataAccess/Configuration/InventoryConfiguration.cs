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
    public class InvetoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.StoreId).IsRequired();
            builder.Property(x => x.UserApplicationId).IsRequired();
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.EndDate).IsRequired();
            builder.Property(x => x.State).IsRequired();

            /* Relationship*/
            builder.HasOne(x => x.Store).WithMany()
                 .HasForeignKey(x => x.StoreId)
                 .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.UserApplication).WithMany()
                .HasForeignKey(x => x.UserApplicationId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
