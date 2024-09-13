using AutoMapper;
using FluentAssertions;
using Moq;
using PermissionsApp.Application.Commands.ModifyPermission;
using PermissionsApp.Application.DTOs;
using PermissionsApp.Application.Services;
using PermissionsApp.Domain.Entities;
using PermissionsApp.Domain.Exceptions;
using PermissionsApp.Domain.Repositories;

namespace PermissionsApp.UnitTests.CommandTests;

public class ModifyPermissionHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IElasticsearchService<ElasticPermission>> _mockElasticsearch;
    private readonly ModifyPermissionHandler _handler;

    public ModifyPermissionHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockElasticsearch = new Mock<IElasticsearchService<ElasticPermission>>();
        _handler = new ModifyPermissionHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockElasticsearch.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdatePermissionAndSendToElasticsearch()
    {
        // Arrange
        var command = new ModifyPermissionCommand(123, "John", "Doe", 1);
        var existingPermission = new Permission
        {
            PermissionId = 123,
            EmployeeName = "Jane",
            EmployeeSurname = "Smith",
            PermissionTypeId = 2,
            CreatedDate = DateTimeOffset.UtcNow,
            LastModifiedDate = DateTimeOffset.UtcNow
        };

        _mockUnitOfWork.Setup(uow => uow.Permissions.GetByIdAsync(command.PermissionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingPermission);
        _mockMapper.Setup(m => m.Map(command, existingPermission))
            .Verifiable();
        _mockUnitOfWork.Setup(uow => uow.Permissions.Update(existingPermission))
            .Verifiable();
        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
        _mockElasticsearch.Setup(es => es.RequestOrModify(
            command.PermissionId,
            It.Is<ElasticPermission>(ep => ep.PermissionId == command.PermissionId)
        ))
        .Returns(Task.CompletedTask)
        .Verifiable();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockMapper.Verify();
        _mockUnitOfWork.Verify(uow => uow.Permissions.Update(existingPermission), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockElasticsearch.Verify(es => es.RequestOrModify(
            command.PermissionId,
            It.Is<ElasticPermission>(ep => ep.PermissionId == command.PermissionId)
        ), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenPermissionNotFound()
    {
        // Arrange
        var command = new ModifyPermissionCommand(123, "John", "Doe", 1);
        _mockUnitOfWork.Setup(uow => uow.Permissions.GetByIdAsync(command.PermissionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Permission)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"The permission '{command.PermissionId}' was not found");
    }
}