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
    private CreateTaskCommandHandler _handler = null!;

    #region SUCCESS CASE

    [Test]
    public async Task Handle_GivenValidTodoTask_ReturnSuccessMessage()
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
            .ReturnsAsync(1);

        var command = new CreateTaskCommand(
            new CreateTaskDto
            {
                Name = "Prepare culture medium",
                Description = "Prepare MS medium",
                StageId = null // to-do task
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
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be("Tạo task thành công");

        TaskRepositoryMock.Verify(
            x => x.Add(It.Is<Domain.Entities.Tasks>(t =>
                t.Name == "Prepare culture medium" &&
                t.StageId == null &&
                t.ResearcherId == "researcher-1" &&
                t.Status == Domain.Common.Enum.TaskStatus.Created
            )),
            Times.Once);

        UnitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region TEMPLATE TASK

    [Test]
    public async Task Handle_GivenValidTemplateTask_WithAttributes_ReturnSuccess()
    {
        // Arrange
        CurrentUserServiceMock
            .Setup(x => x.UserId)
            .Returns("researcher-2");

        TaskRepositoryMock
            .Setup(x => x.UnitOfWork)
            .Returns(UnitOfWorkMock.Object);

        UnitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var command = new CreateTaskCommand(
            new CreateTaskDto
            {
                Name = "Add hormone",
                Description = "Add BAP hormone",
                StageId = "stage-1"
            },
            createTaskAttributes:
            [
                new CreateTaskAttributeDto
                {
                    Value = 2,
                    Unit = "mg/L"
                }
            ],
            createTaskAssignment: new CreateTaskAssignmentDto
            {
                TechnicianId = "tech-2",
                SampleId = null,
                IsForWholeExperimentLog = true,
                ExpectedEndDate = DateTime.UtcNow.AddDays(5)
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be("Tạo task thành công");

        TaskRepositoryMock.Verify(
            x => x.Add(It.Is<Domain.Entities.Tasks>(t =>
                t.StageId == "stage-1" &&
                t.TaskAttributes!.Count == 1
            )),
            Times.Once);
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
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be("Tạo task thất bại");
    }

    [Test]
    public async Task Handle_GivenInvalidCommand_ThrowException()
    {
        // Arrange
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
            await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DomainException>();
    }

    #endregion
}
