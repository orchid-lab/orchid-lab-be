using AutoMapper;
using orchid_backend_net.Application.Tasks.CreateTask;

namespace orchid_backend_net.Application.Tests.Config.TaskConfig
{
    internal abstract class TaskHandlerTestConfig : BaseHandlerTestConfig
    {
        protected CreateTaskCommandHandler CreateCommandHandler = null!;
        protected IMapper _mapper = null!;
        [SetUp]
        public void Setup()
        {
            CreateCommandHandler = new CreateTaskCommandHandler(
                TaskRepositoryMock.Object,
                CurrentUserServiceMock.Object);
        }
    }
}
