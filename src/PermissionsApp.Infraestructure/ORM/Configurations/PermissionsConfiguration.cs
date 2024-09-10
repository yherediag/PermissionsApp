using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PermissionsApp.Domain.Entities;

namespace PermissionsApp.Infraestructure.ORM.Configurations;

public class PermissionsConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder
            .ToTable("Permissions");

        builder
            .HasKey(x => x.PermissionId)
            .HasName("PK_PermissionId");

        builder
            .HasOne(permission => permission.PermissionType)
            .WithMany(type => type.Permissions)
            .HasForeignKey(permission => permission.PermissionTypeId);
    }
}
