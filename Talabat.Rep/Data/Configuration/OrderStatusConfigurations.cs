using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Configuration
{
    public class OrderStatusConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
           builder.Property(o => o.Status)
            .HasConversion(
                OrderStatus => OrderStatus.ToString(),
                OrderStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), OrderStatus)
            );

            builder.Property(o => o.Subtotal)
                .HasColumnType("decimal(18,2)");

            builder.OwnsOne(o => o.ShippingAddress, shA => shA.WithOwner());

            builder.HasOne(o => o.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);


        }
    }       
}
