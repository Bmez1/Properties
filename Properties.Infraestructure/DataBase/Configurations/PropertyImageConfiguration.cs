using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Properties.Domain.Entities;

namespace Properties.Infraestructure.DataBase.Configurations;

internal sealed class PropertyImageConfiguration : IEntityTypeConfiguration<PropertyImage>
{
    public void Configure(EntityTypeBuilder<PropertyImage> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.File).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Enabled).IsRequired();
    }
}
