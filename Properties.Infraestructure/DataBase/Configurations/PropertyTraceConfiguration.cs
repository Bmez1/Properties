using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Properties.Domain.Entities;

namespace Properties.Infraestructure.DataBase.Configurations;

internal sealed class PropertyTraceConfiguration : IEntityTypeConfiguration<PropertyTrace>
{
    public void Configure(EntityTypeBuilder<PropertyTrace> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Value).HasPrecision(18, 2);
        builder.Property(x => x.Tax).HasPrecision(18, 2);
    }
}
