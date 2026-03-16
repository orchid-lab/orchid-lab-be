using FluentAssertions;
using Moq;
using orchid_backend_net.Application.Tasks.UseCase.CreateTask;
using orchid_backend_net.Application.Tasks.Dto;
using orchid_backend_net.Application.Tasks.Dto.Task;
using orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto;
using orchid_backend_net.Application.Tasks.Dto.TaskAttributeDto;
using orchid_backend_net.Application.Tests.Config.TaskConfig;
using System.Linq.Expressions;
using orchid_backend_net.Domain.Entities;
using orchid_backend_net.Application.Tasks.Dto.TaskCheckListItem;

namespace orchid_backend_net.Application.Tests.Tasks.CreateTaskCommandHandlerTest;

[TestFixture]
internal class CreateTaskCommandHandlerTest : TaskHandlerTestConfig
{

    #region SUCCESS CASE

    [Test]
    public async Task Handle_GivenTodoTask_ShouldCreateTaskAssignment()
    {
        // Arrange
        CurrentUserServiceMock
            .Setup(x => x.UserId)
            .Returns("researcher-1");

        TimeProviderMock
            .Setup(x => x.Now)
            .Returns(new DateTime(2026, 1, 3, 9, 0, 0, DateTimeKind.Utc));

        TimeProviderMock
            .Setup(x => x.IsInWorkingHour(It.IsAny<DateTime>()))
            .Returns(true);

        TaskRepositoryMock
            .Setup(x => x.UnitOfWork)
            .Returns(UnitOfWorkMock.Object);

        UnitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        Domain.Entities.Tasks? savedTask = null;

        TaskRepositoryMock
            .Setup(x => x.Add(It.IsAny<Domain.Entities.Tasks>()))
            .Callback<Domain.Entities.Tasks>(t => savedTask = t);

        var command = new CreateTaskCommand(
            new CreateTaskDto
            {
                Name = "Prepare culture medium",
                Description = "Prepare MS medium",
                StageId = null // to-do
            },
            createTaskAttributes: null,
            createTaskAssignment: new CreateTaskAssignmentDto
            {
                TechnicianId = "tech-1",
                TargetId = "sample-1",
                TargetType = Domain.Common.Enum.TaskTargetType.Sample,
                ExpectedEndDate = TimeProviderMock.Object.Now.AddDays(3)
            },
            createTaskCheckListItems:
            [
                new CreateTaskCheckListItemDto
                {
                    Name = "Weigh chemicals",
                    Description = "Weigh 4g of MS powder and 30g of sucrose",
                    Order = 1,
                    ExpectedUnit = "g",
                    ExpectedMinValue = 34,
                    ExpectedMaxValue = 34
                },
            ]);

        // Act
        var result = await CreateCommandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be("Tạo task thành công");

        savedTask.Should().NotBeNull();
        savedTask!.TaskAssignment.Should().NotBeNull();

        var assignment = savedTask.TaskAssignment;
        assignment.TechnicianId.Should().Be("tech-1");
        assignment.TargetId.Should().Be("sample-1");
        assignment.TargetType.Should().Be(Domain.Common.Enum.TaskTargetType.Sample);
        savedTask.TaskAttributes.Should().BeEmpty();
    }

    #endregion

    #region TEMPLATE TASK

    [Test]
    public async Task Handle_GivenTemplateTask_ShouldCreateTaskAttributes()
    {
        // Arrange
        CurrentUserServiceMock
            .Setup(x => x.UserId)
            .Returns("researcher-2");

        TimeProviderMock
            .Setup(x => x.Now)
            .Returns(new DateTime(2026, 1, 3, 10, 0, 0, DateTimeKind.Utc));

        TimeProviderMock
            .Setup(x => x.IsInWorkingHour(It.IsAny<DateTime>()))
            .Returns(true);

        TaskRepositoryMock
            .Setup(x => x.UnitOfWork)
            .Returns(UnitOfWorkMock.Object);

        UnitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        Domain.Entities.Tasks? savedTask = null;

        TaskRepositoryMock
            .Setup(x => x.Add(It.IsAny<Domain.Entities.Tasks>()))
            .Callback<Domain.Entities.Tasks>(t => savedTask = t);

        StageDefinitionRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<MethodStageDefinition, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var command = new CreateTaskCommand(
            new CreateTaskDto
            {
                Name = "Add hormone",
                Description = "Add BAP",
                StageId = 1 // template
            },
            createTaskAttributes:
            [
                new CreateTaskAttributeDto
                {
                    ChemicalId = 2,
                    Value = 2,
                    Unit = "mg/L"
                }
            ],
            null,
            createTaskCheckListItems:
            [
                new CreateTaskCheckListItemDto
                {
                    Name = "Weigh chemicals",
                    Description = "Weigh 4g of MS powder and 30g of sucrose",
                    Order = 1,
                    ExpectedUnit = "g",
                    ExpectedMinValue = 34,
                    ExpectedMaxValue = 34
                },
            ]);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        // Act
        var result = await CreateCommandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be("Tạo task thành công");

        savedTask.Should().NotBeNull();
        savedTask!.StageId.Should().Be(1);

        savedTask.TaskAssignment.Should().BeNull();

        savedTask.TaskAttributes.Should().HaveCount(1);
        var attr = savedTask.TaskAttributes.First();
        attr.Should().NotBeNull();
        attr.ChemicalId.Should().Be(2);
        attr.Value.Should().Be(2);
        attr.Unit.Should().Be("mg/L");
    }

    #endregion

    #region FAILURE CASES

    [Test]
    public async Task Handle_SaveChangesFail_ReturnFailureMessage()
    {
        // Arrange
        CurrentUserServiceMock
            .Setup(x => x.UserId)
            .Returns("researcher-1");

        TaskRepositoryMock
            .Setup(x => x.UnitOfWork)
            .Returns(UnitOfWorkMock.Object);

        UnitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        TimeProviderMock
            .Setup(x => x.Now)
            .Returns(new DateTime(2026, 1, 3, 10, 0, 0, DateTimeKind.Utc));

        TimeProviderMock
            .Setup(x => x.IsInWorkingHour(It.IsAny<DateTime>()))
            .Returns(true);

        var command = new CreateTaskCommand(
            new CreateTaskDto
            {
                Name = "Invalid save",
                StageId = null
            },
            null,
            new CreateTaskAssignmentDto
            {
                TargetId = "target-1",
                TechnicianId = "tech-1",
                TargetType = Domain.Common.Enum.TaskTargetType.Sample,
                ExpectedEndDate = TimeProviderMock.Object.Now.AddDays(1)
            },
            createTaskCheckListItems:
            [
                new CreateTaskCheckListItemDto
                {
                    Name = "Weigh chemicals",
                    Description = "Weigh 4g of MS powder and 30g of sucrose",
                    Order = 1,
                    ExpectedUnit = "g",
                    ExpectedMinValue = 34,
                    ExpectedMaxValue = 34
                },
            ]);

        // Act
        var result = await CreateCommandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be("Tạo task thất bại");
    }

    [Test]
    public async Task Handle_GivenInvalidCommand_ThrowException()
    {
        // Arrange

        TimeProviderMock
            .Setup(x => x.Now)
            .Returns(new DateTime(2026, 1, 3, 10, 0, 0, DateTimeKind.Utc));

        TimeProviderMock
            .Setup(x => x.IsInWorkingHour(It.IsAny<DateTime>()))
            .Returns(false);

        var command = new CreateTaskCommand(
            new CreateTaskDto
            {
                Name = "", // invalid
                StageId = null
            },
            null,
            new CreateTaskAssignmentDto
            {
                TargetId = null!,
                TechnicianId = null!, // invalid
                TargetType = Domain.Common.Enum.TaskTargetType.Sample,
                ExpectedEndDate = TimeProviderMock.Object.Now.AddDays(-1)
            },
            createTaskCheckListItems:
            [
                new CreateTaskCheckListItemDto
                {
                    Name = "Weigh chemicals",
                    Description = "Weigh 4g of MS powder and 30g of sucrose",
                    Order = 1,
                    ExpectedUnit = "g",
                    ExpectedMinValue = 34,
                    ExpectedMaxValue = 34
                },
            ]);

        // Act
        Func<Task> act = async () =>
            await CreateCommandHandler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NullReferenceException>();
    }

    #endregion
}
