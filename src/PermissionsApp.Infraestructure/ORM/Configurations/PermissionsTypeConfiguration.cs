using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PermissionsApp.Domain.Entities;

namespace PermissionsApp.Infraestructure.ORM.Configurations;

public class PermissionsTypeConfiguration : IEntityTypeConfiguration<PermissionType>
{
    public void Configure(EntityTypeBuilder<PermissionType> builder)
    {
        builder
            .ToTable("PermissionsType");
    }
}
