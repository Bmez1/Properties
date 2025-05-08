using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Properties.Domain.Entities;

namespace Properties.Infraestructure.DataBase.Configurations;

internal sealed class OwnerConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(100);

        builder
        .HasMany(o => o.Properties)
        .WithOne()
        .HasForeignKey(h => h.OwnerId);
    }
}
