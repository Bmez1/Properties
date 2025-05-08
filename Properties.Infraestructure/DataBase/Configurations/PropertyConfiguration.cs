using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Properties.Domain.Entities;

namespace Properties.Infraestructure.DataBase.Configurations;

internal sealed class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(100);
        builder.Property(x => x.CodeInternal).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Price).HasPrecision(18, 2);

        builder
        .HasOne(p => p.Owner)
        .WithMany(p => p.Properties)
        .HasForeignKey(h => h.OwnerId)
        .OnDelete(DeleteBehavior.SetNull);

        builder
        .HasMany(p => p.Traces)
        .WithOne(p => p.Property)
        .HasForeignKey(h => h.PropertyId)
        .OnDelete(DeleteBehavior.Restrict);

        builder
        .HasMany(p => p.Images)
        .WithOne(p => p.Property)
        .HasForeignKey(h => h.PropertyId)
        .OnDelete(DeleteBehavior.Cascade);

    }
}
