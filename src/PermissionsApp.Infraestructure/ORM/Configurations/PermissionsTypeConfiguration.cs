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

        builder
            .HasData([
                new() { PermissionTypeId = 1, Description = "Leader" },
                new() { PermissionTypeId = 2, Description = "Analyst" },
                new() { PermissionTypeId = 3, Description = "Developer" },
                new() { PermissionTypeId = 4, Description = "Tester" },
                new() { PermissionTypeId = 99, Description = "Admin" },
            ]);
    }
}
