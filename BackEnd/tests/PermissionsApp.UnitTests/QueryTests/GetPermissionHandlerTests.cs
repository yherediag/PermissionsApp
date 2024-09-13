﻿using AutoMapper;
using FluentAssertions;
using Moq;
using PermissionsApp.Application.Queries.GetPermission;
using PermissionsApp.Domain.Entities;
using PermissionsApp.Domain.Exceptions;
using PermissionsApp.Domain.Primitives;
using PermissionsApp.Domain.Repositories;

namespace PermissionsApp.UnitTests.QueryTests;

public class GetPermissionHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IEventBus> _mockEventBus;
    private readonly GetPermissionHandler _handler;

    public GetPermissionHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockEventBus = new Mock<IEventBus>();
        _handler = new GetPermissionHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockEventBus.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPermission_WhenPermissionExists()
    {
        // Arrange
        var query = new GetPermissionQuery(1);
        var permission = new Permission
        {
            PermissionId = 1,
            EmployeeName = "John",
            EmployeeSurname = "Doe",
            PermissionTypeId = 1,
            CreatedDate = DateTimeOffset.UtcNow,
            LastModifiedDate = DateTimeOffset.UtcNow
        };
        var dto = new GetPermissionDto(
            permission.PermissionId,
            permission.EmployeeName,
            permission.EmployeeSurname,
            permission.PermissionTypeId,
            null,
            permission.CreatedDate,
            permission.LastModifiedDate
        );

        _mockUnitOfWork
            .Setup(uow => uow.Permissions.GetByIdAsync(query.PermissionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(permission);
        _mockMapper
            .Setup(m => m.Map<GetPermissionDto>(permission))
            .Returns(dto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result
            .Should()
            .BeEquivalentTo(dto);

        _mockEventBus
            .Verify(bus => bus.Publish(It.IsAny<EventMessage>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenPermissionDoesNotExist()
    {
        // Arrange
        var query = new GetPermissionQuery(1);

        _mockUnitOfWork
            .Setup(uow => uow.Permissions.GetByIdAsync(query.PermissionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Permission)null);

        // Act
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>().WithMessage($"The permission '{query.PermissionId}' was not found");
        
        _mockEventBus
            .Verify(bus => bus.Publish(It.IsAny<EventMessage>()), Times.Never);
    }
}
