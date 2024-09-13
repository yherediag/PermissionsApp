using AutoMapper;
using FluentAssertions;
using Moq;
using PermissionsApp.Application.Commands.RequestPermission;
using PermissionsApp.Application.DTOs;
using PermissionsApp.Application.Services;
using PermissionsApp.Domain.Entities;
using PermissionsApp.Domain.Events;
using PermissionsApp.Domain.Repositories;

namespace PermissionsApp.UnitTests.CommandTests;

public class RequestPermissionHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IElasticsearchService<ElasticPermission>> _mockElasticsearch;
    private readonly RequestPermissionHandler _handler;

    public RequestPermissionHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockElasticsearch = new Mock<IElasticsearchService<ElasticPermission>>();
        _handler = new RequestPermissionHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockElasticsearch.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenElasticsearchFails()
    {
        // Arrange
        var command = new RequestPermissionCommand("John", "Doe", 1);
        var permission = new Permission
        {
            PermissionId = 123,
            EmployeeName = "John",
            EmployeeSurname = "Doe",
            PermissionTypeId = 1,
            CreatedDate = DateTimeOffset.UtcNow,
            LastModifiedDate = DateTimeOffset.UtcNow
        };

        _mockMapper.Setup(m => m.Map<Permission>(command))
            .Returns(permission);
        _mockUnitOfWork.Setup(uow => uow.Permissions.AddAsync(permission, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockElasticsearch.Setup(es => es.RequestOrModify(
            permission.PermissionId,
            It.IsAny<ElasticPermission>()
        ))
        .ThrowsAsync(new Exception("Elasticsearch failed"));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Elasticsearch failed");
    }

    [Fact]
    public async Task Handle_ShouldAddDomainEvent_WhenPermissionIsAdded()
    {
        // Arrange
        var command = new RequestPermissionCommand("John", "Doe", 1);
        var permission = new Permission
        {
            PermissionId = 123,
            EmployeeName = "John",
            EmployeeSurname = "Doe",
            PermissionTypeId = 1,
            CreatedDate = DateTimeOffset.UtcNow,
            LastModifiedDate = DateTimeOffset.UtcNow
        };

        _mockMapper.Setup(m => m.Map<Permission>(command))
            .Returns(permission);
        _mockUnitOfWork.Setup(uow => uow.Permissions.AddAsync(permission, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        permission.DomainEvents.Should().Contain(e => e is RequestPermissionEvent);
    }

    [Fact]
    public async Task Handle_ShouldCallAddDomainEvent_WhenPermissionIsAdded()
    {
        // Arrange
        var command = new RequestPermissionCommand("John", "Doe", 1);
        var permission = new Permission
        {
            PermissionId = 123,
            EmployeeName = "John",
            EmployeeSurname = "Doe",
            PermissionTypeId = 1,
            CreatedDate = DateTimeOffset.UtcNow,
            LastModifiedDate = DateTimeOffset.UtcNow
        };

        _mockMapper.Setup(m => m.Map<Permission>(command))
            .Returns(permission);
        _mockUnitOfWork.Setup(uow => uow.Permissions.AddAsync(permission, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockElasticsearch.Setup(es => es.RequestOrModify(
            permission.PermissionId,
            It.IsAny<ElasticPermission>()
        ))
        .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockUnitOfWork.Verify(uow => uow.Permissions.AddAsync(permission, It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockElasticsearch.Verify(es => es.RequestOrModify(
            permission.PermissionId,
            It.Is<ElasticPermission>(ep =>
                ep.PermissionId == permission.PermissionId &&
                ep.EmployeeName == permission.EmployeeName &&
                ep.EmployeeSurname == permission.EmployeeSurname
            )
        ), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnCorrectPermissionId_WhenDataIsValid()
    {
        // Arrange
        var command = new RequestPermissionCommand("John", "Doe", 1);
        var permission = new Permission
        {
            PermissionId = 123,
            EmployeeName = "John",
            EmployeeSurname = "Doe",
            PermissionTypeId = 1,
            CreatedDate = DateTimeOffset.UtcNow,
            LastModifiedDate = DateTimeOffset.UtcNow
        };

        _mockMapper.Setup(m => m.Map<Permission>(command))
            .Returns(permission);
        _mockUnitOfWork.Setup(uow => uow.Permissions.AddAsync(permission, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockElasticsearch.Setup(es => es.RequestOrModify(
            permission.PermissionId,
            It.IsAny<ElasticPermission>()
        ))
        .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(permission.PermissionId);
    }

    [Fact]
    public async Task Handle_ShouldMapCommandToPermissionCorrectly()
    {
        // Arrange
        var command = new RequestPermissionCommand("John", "Doe", 1);
        var permission = new Permission
        {
            PermissionId = 123,
            EmployeeName = "John",
            EmployeeSurname = "Doe",
            PermissionTypeId = 1,
            CreatedDate = DateTimeOffset.UtcNow,
            LastModifiedDate = DateTimeOffset.UtcNow
        };

        _mockMapper.Setup(m => m.Map<Permission>(command))
            .Returns(permission);
        _mockUnitOfWork.Setup(uow => uow.Permissions.AddAsync(permission, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockElasticsearch.Setup(es => es.RequestOrModify(
            permission.PermissionId,
            It.IsAny<ElasticPermission>()
        ))
        .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockMapper.Verify(m => m.Map<Permission>(command), Times.Once);
    }
}
