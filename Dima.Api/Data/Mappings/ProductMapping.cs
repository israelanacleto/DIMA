using System.Text.Json;
using Dima.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
            .HasMaxLength(80);

        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasMaxLength(255);

        builder.Property(x => x.Slug)
            .IsRequired(false)
            .HasMaxLength(80);

        builder.Property(x => x.Price)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.Benefits)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null!) ?? new List<string>(),
                new ValueComparer<List<string>>(
                    (a, b) => (a ?? new List<string>()).SequenceEqual(b ?? new List<string>()),
                    v => v == null ? 0 : v.Aggregate(0, (h, s) => HashCode.Combine(h, s.GetHashCode())),
                    v => v == null ? new List<string>() : v.ToList()
                )
            );

        builder.Property(x => x.SubscriptionDurationInDays)
            .IsRequired()
            .HasDefaultValue(0);

        builder
            .HasIndex(u => u.Slug, "IX_Product_Slug")
            .IsUnique();
    }
}
