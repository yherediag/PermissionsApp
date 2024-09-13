using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PermissionsApp.Application.Commands.ModifyPermission;
using PermissionsApp.Application.Commands.RequestPermission;
using PermissionsApp.Application.DTOs;
using PermissionsApp.Application.Queries.GetPermission;
using PermissionsApp.Application.Queries.GetPermissions;
using PermissionsApp.Application.Services;
using PermissionsApp.Domain.Entities;
using PermissionsApp.Domain.Primitives;
using PermissionsApp.Domain.Repositories;
using PermissionsApp.WebAPI;
using System.Net;
using System.Net.Http.Json;

namespace PermissionsApp.IntegrationTests.ControllersTests
{
    public class PermissionsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private const string RESOURCE_NAME = "/api/Permissions";
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public PermissionsControllerTests(WebApplicationFactory<Program> factory)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var mockPermissionsRepo = new Mock<IPermissionsRepository>();
                    mockPermissionsRepo.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((int pageNumber, int pageSize, CancellationToken cancellationToken) =>
                        {
                            var permissions = MockPermissions().ToList();
                            return (permissions, permissions.Count);
                        });
                    mockPermissionsRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((int id, CancellationToken cancellationToken) =>
                        {
                            return MockPermissions().FirstOrDefault(p => p.PermissionId == id);
                        });

                    var mockUnitOfWork = new Mock<IUnitOfWork>();
                    // IPermissionsRepository
                    mockUnitOfWork.Setup(uow => uow.Permissions).Returns(mockPermissionsRepo.Object);
                    // IPermissionsTypeRepository
                    mockUnitOfWork.Setup(uow => uow.PermissionsType).Returns(new Mock<IPermissionsTypeRepository>().Object);
                    mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
                    // IUnitOfWork
                    services.AddSingleton(mockUnitOfWork.Object);
                    // EventBus
                    services.AddSingleton(new Mock<IEventBus>().Object);
                    // IElasticsearchService
                    services.AddSingleton(new Mock<IElasticsearchService<ElasticPermission>>().Object);
                });
            });

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetPermissions_ShouldReturnListOfPermissions()
        {
            // Act
            var response = await _client.GetAsync(RESOURCE_NAME);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<GetPermissionsResponse>();
            result.Should().NotBeNull();
            result.Permissions.Should().NotBeNull();
            result.Permissions.Should().HaveCountGreaterThan(0);
            result.TotalCount.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetPermission_ShouldReturnPermissionById()
        {
            // Act
            var response = await _client.GetAsync($"{RESOURCE_NAME}/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var permission = await response.Content.ReadFromJsonAsync<GetPermissionDto>();
            permission.Should().NotBeNull();
            permission!.PermissionId.Should().Be(1);
        }

        [Fact]
        public async Task RequestPermission_ShouldCreateNewPermission()
        {
            // Arrange
            var command = new RequestPermissionCommand("John", "Doe", 1);

            // Act
            var response = await _client.PostAsJsonAsync(RESOURCE_NAME, command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var location = response.Headers.Location!.ToString();
            location.Should().Contain(RESOURCE_NAME);
        }

        [Fact]
        public async Task ModifyPermissions_ShouldUpdatePermission()
        {
            // Arrange
            var command = new ModifyPermissionCommand(1, "UpdatedName", "UpdatedSurname", 2);

            // Act
            var response = await _client.PutAsJsonAsync($"{RESOURCE_NAME}/1", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task RemovePermissions_ShouldDeletePermission()
        {
            // Act
            var response = await _client.DeleteAsync($"{RESOURCE_NAME}/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        private IEnumerable<Permission> MockPermissions()
        {
            // Create some PermissionType instances
            var permissionType1 = new PermissionType
            {
                PermissionTypeId = 1,
                Description = "Annual Leave"
            };

            var permissionType2 = new PermissionType
            {
                PermissionTypeId = 2,
                Description = "Sick Leave"
            };

            // Create a list of Permission instances
            List<Permission> permissions =
            [
                new Permission
                {
                    PermissionId = 1,
                    EmployeeName = "John",
                    EmployeeSurname = "Doe",
                    PermissionTypeId = permissionType1.PermissionTypeId,
                    PermissionType = permissionType1,
                    CreatedDate = DateTimeOffset.UtcNow,
                    LastModifiedDate = DateTimeOffset.UtcNow,
                    IsDeleted = false
                },
                new Permission
                {
                    PermissionId = 2,
                    EmployeeName = "Jane",
                    EmployeeSurname = "Smith",
                    PermissionTypeId = permissionType2.PermissionTypeId,
                    PermissionType = permissionType2,
                    CreatedDate = DateTimeOffset.UtcNow,
                    LastModifiedDate = DateTimeOffset.UtcNow,
                    IsDeleted = false
                },
                new Permission
                {
                    PermissionId = 3,
                    EmployeeName = "Alice",
                    EmployeeSurname = "Johnson",
                    PermissionTypeId = permissionType1.PermissionTypeId,
                    PermissionType = permissionType1,
                    CreatedDate = DateTimeOffset.UtcNow,
                    LastModifiedDate = DateTimeOffset.UtcNow,
                    IsDeleted = false
                }
            ];

            return permissions;
        }
    }
}
