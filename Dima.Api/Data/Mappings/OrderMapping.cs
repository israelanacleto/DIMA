using Dima.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings;

public class OrderMapping : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Order");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Number)
            .IsRequired()
            .HasColumnType("CHAR")
            .HasMaxLength(8);
        
        builder.Property(x => x.ExternalReference)
            .IsRequired(false)
            .HasColumnType("VARCHAR")
            .HasMaxLength(60);
        
        builder.Property(x => x.Gateway)
            .IsRequired()
            .HasColumnType("SMALLINT");
        
        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("DATETIME2");
        
        builder.Property(x => x.UpdatedAt)
            .IsRequired()
            .HasColumnType("DATETIME2");
        
        builder.Property(x => x.Status)
            .IsRequired()
            .HasColumnType("SMALLINT");
        
        builder.Property(x => x.UserId)
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(160);

        builder
            .HasOne(x => x.Product)
            .WithMany()
            .HasConstraintName("FK_Order_Product")
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(x => x.Voucher)
            .WithMany()
            .HasConstraintName("FK_Order_Voucher")
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(x => x.SubscriptionStartDate)
            .IsRequired(false)
            .HasColumnName("SubscriptionStartDate")
            .HasColumnType("DATETIME2");
        
        builder.Property(x => x.SubscriptionEndDate)
            .IsRequired(false)
            .HasColumnName("SubscriptionEndDate")
            .HasColumnType("DATETIME2");

        builder.Ignore(x => x.IsPremiumActive);
        
        
        builder
            .HasIndex(u => u.Number, "IX_Order_Number")
            .IsUnique();
        builder.HasIndex(u => u.ExternalReference, "IX_Order_ExternalReference");
        builder.HasIndex(u => u.UserId, "IX_Order_UserId");
        builder.HasIndex(u => u.Status, "IX_Order_Status");
        builder.HasIndex(u => u.Gateway, "IX_Order_Gateway");
    }
}