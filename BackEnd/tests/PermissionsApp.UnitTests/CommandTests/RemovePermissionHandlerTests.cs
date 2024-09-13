using FluentAssertions;
using Moq;
using PermissionsApp.Application.Commands.RemovePermission;
using PermissionsApp.Domain.Entities;
using PermissionsApp.Domain.Events;
using PermissionsApp.Domain.Exceptions;
using PermissionsApp.Domain.Repositories;

namespace PermissionsApp.UnitTests.CommandTests;

public class RemovePermissionHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly RemovePermissionHandler _handler;

    public RemovePermissionHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new RemovePermissionHandler(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldRemovePermissionAndReturnTrue()
    {
        // Arrange
        var command = new RemovePermissionCommand(123);
        var permission = new Permission
        {
            PermissionId = 123,
            EmployeeName = "John",
            EmployeeSurname = "Doe",
            PermissionTypeId = 1,
            CreatedDate = DateTimeOffset.UtcNow,
            LastModifiedDate = DateTimeOffset.UtcNow
        };

        _mockUnitOfWork.Setup(uow => uow.Permissions.GetByIdAsync(command.PermissionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(permission);
        _mockUnitOfWork.Setup(uow => uow.Permissions.Delete(permission))
            .Verifiable();
        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        _mockUnitOfWork.Verify(uow => uow.Permissions.Delete(permission), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenPermissionNotFound()
    {
        // Arrange
        var command = new RemovePermissionCommand(123);
        _mockUnitOfWork.Setup(uow => uow.Permissions.GetByIdAsync(command.PermissionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Permission)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"The permission '{command.PermissionId}' was not found");
    }

    [Fact]
    public async Task Handle_ShouldNotThrowException_WhenPermissionExistsButNotDeleted()
    {
        // Arrange
        var command = new RemovePermissionCommand(456);
        var permission = new Permission
        {
            PermissionId = 456,
            EmployeeName = "Jane",
            EmployeeSurname = "Smith",
            PermissionTypeId = 2,
            CreatedDate = DateTimeOffset.UtcNow,
            LastModifiedDate = DateTimeOffset.UtcNow
        };

        _mockUnitOfWork.Setup(uow => uow.Permissions.GetByIdAsync(command.PermissionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(permission);
        _mockUnitOfWork.Setup(uow => uow.Permissions.Delete(permission))
            .Verifiable();
        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        _mockUnitOfWork.Verify(uow => uow.Permissions.Delete(permission), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCallAddDomainEvent_WhenPermissionIsRemoved()
    {
        // Arrange
        var command = new RemovePermissionCommand(789);
        var permission = new Permission
        {
            PermissionId = 789,
            EmployeeName = "Alice",
            EmployeeSurname = "Johnson",
            PermissionTypeId = 3,
            CreatedDate = DateTimeOffset.UtcNow,
            LastModifiedDate = DateTimeOffset.UtcNow
        };

        _mockUnitOfWork.Setup(uow => uow.Permissions.GetByIdAsync(command.PermissionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(permission);
        _mockUnitOfWork.Setup(uow => uow.Permissions.Delete(permission))
            .Verifiable();
        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        permission.DomainEvents.Should().Contain(e => e is RemovePermissionEvent);
    }
}