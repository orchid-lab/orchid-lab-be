using FluentAssertions;
using Moq;
using orchid_backend_net.Application.Tasks.CreateTask;
using orchid_backend_net.Application.Tasks.Dto;
using orchid_backend_net.Application.Tasks.Dto.TaskAssignmentDto;
using orchid_backend_net.Application.Tasks.Dto.TaskAttributeDto;
using orchid_backend_net.Application.Tests.Config.TaskConfig;
using orchid_backend_net.Domain.Common.Exceptions;

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
                SampleId = "sample-1",
                IsForWholeExperimentLog = false,
                ExpectedEndDate = DateTime.UtcNow.AddDays(3)
            });

        // Act
        var result = await CreateCommandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be("Tạo task thành công");

        savedTask.Should().NotBeNull();
        savedTask!.TaskAssignments.Should().HaveCount(1);

        var assignment = savedTask.TaskAssignments.First();
        assignment.TechnicianId.Should().Be("tech-1");
        assignment.SampleId.Should().Be("sample-1");

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

        var command = new CreateTaskCommand(
            new CreateTaskDto
            {
                Name = "Add hormone",
                Description = "Add BAP",
                StageId = "stage-1" // template
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
            createTaskAssignment: new CreateTaskAssignmentDto
            {
                TechnicianId = null,
                SampleId = null,
                IsForWholeExperimentLog = true,
                ExpectedEndDate = DateTime.UtcNow.AddDays(5)
            });

        // Act
        var result = await CreateCommandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be("Tạo task thành công");

        savedTask.Should().NotBeNull();
        savedTask!.StageId.Should().Be("stage-1");

        savedTask.TaskAssignments.Should().BeEmpty();

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
                TechnicianId = "tech-1",
                ExpectedEndDate = DateTime.UtcNow.AddDays(1)
            });

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
            .Returns(true);

        var command = new CreateTaskCommand(
            new CreateTaskDto
            {
                Name = "", // invalid
                StageId = null
            },
            null,
            new CreateTaskAssignmentDto
            {
                TechnicianId = null!, // invalid
                ExpectedEndDate = DateTime.UtcNow.AddDays(-1)
            });

        // Act
        Func<Task> act = async () =>
            await CreateCommandHandler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    #endregion
}
