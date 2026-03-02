using System.Text.Json;
using Dima.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings;

public class ProductMapping : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);

        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(255);
        
        builder.Property(x => x.Slug)
            .IsRequired(false)
            .HasColumnType("VARCHAR")
            .HasMaxLength(80);

        builder.Property(x => x.Price)
            .IsRequired()
            .HasColumnType("MONEY");

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasColumnType("BIT");

        builder.Property(x => x.Benefits)
            .HasColumnType("NVARCHAR(MAX)")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null!) ?? new List<string>()
            );
        
        builder.Property(x => x.SubscriptionDurationInDays)
            .IsRequired()
            .HasDefaultValue(0)
            .HasColumnType("INT");
        
        builder
            .HasIndex(u => u.Slug,"IX_Product_Slug")
            .IsUnique();
    }
}