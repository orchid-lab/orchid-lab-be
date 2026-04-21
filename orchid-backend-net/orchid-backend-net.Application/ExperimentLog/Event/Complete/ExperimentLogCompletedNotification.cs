using MediatR;
using orchid_backend_net.Application.Common.Events;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Common.Const;
using UnitConst = orchid_backend_net.Domain.Common.Const.Unit;
using orchid_backend_net.Domain.Common.Interfaces;
using orchid_backend_net.Domain.Events.ExperimentLogEvents;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Application.ExperimentLog.Event.Complete
{
    internal class ExperimentLogCompletedNotificationHandler(
        ExperimentLogCompletedNotificationRepository repository,
        ExperimentLogCompletedNotificationService service)
        : INotificationHandler<DomainEventNotification<ExperimentLogCompleted>>
    {
        public async Task Handle(DomainEventNotification<ExperimentLogCompleted> evt, CancellationToken cancellationToken)
        {
            var experiment = await repository.ExperimentLogRepository.GetExperimentLogByIdAsync(evt.DomainEvent.ExperimentLogId, cancellationToken);
            var researcher = await repository.UserRepository.GetByIdAsync(experiment.CreatedBy, cancellationToken);
            var technician = await repository.UserRepository.GetByIdAsync(experiment.AssignedTo, cancellationToken);

            var title = "Thí nghiệm đã hoàn thành";
            var content = $"Thí nghiệm {experiment.Name} đã được đánh dấu hoàn thành bởi {researcher.Name}";
            var noti = CreateNotificationHelper.CreateForSingleUsers(technician.ID, title, content);
            await service.NotificationPushService.PushToSingleUserAsync(technician.ID, title, content);
            repository.NotificationRepository.Add(noti);

            //create cleanning task for technician 
            var cleaningTask = new Domain.Entities.Tasks
            {
                Name = $"Dọn dẹp sau thí nghiệm {experiment.Name}",
                Description = $"Dọn dẹp và chuẩn bị lại khu vực sau khi hoàn thành thí nghiệm {experiment.Name}",
                CreatedBy = researcher.ID,
                ResearcherId = researcher.ID,
            };

            cleaningTask.AddTaskAssignment(
                evt.DomainEvent.TechnicianId,
                Domain.Common.Enum.TaskTargetType.ExperimentLog,
                evt.DomainEvent.ExperimentLogId,
                DateTime.UtcNow.AddDays(3),
                DateTime.UtcNow,
                true);

            async Task<int> GetMaterialIdAsync(string materialName)
                => (await repository.MaterialRepository.FindAsync(x => x.Name == materialName, cancellationToken))!.ID;

            async Task<int> GetChemicalIdAsync(string chemicalName)
                => (await repository.ChemicalsRepository.FindAsync(x => x.Name == chemicalName, cancellationToken))!.ID;

            cleaningTask.AddTaskAttribute(null, await GetMaterialIdAsync(MaterialNames.GLOVES), UnitConst.MATERIAL_UNIT, 1);
            cleaningTask.AddTaskAttribute(null, await GetMaterialIdAsync(MaterialNames.TRAY), UnitConst.MATERIAL_UNIT, 1);
            cleaningTask.AddTaskAttribute(null, await GetMaterialIdAsync(MaterialNames.FILTER_PAPER), UnitConst.MATERIAL_UNIT, 1);
            cleaningTask.AddTaskAttribute(null, await GetMaterialIdAsync("Vòi nước"), UnitConst.MATERIAL_UNIT, 1);
            cleaningTask.AddTaskAttribute(null, await GetMaterialIdAsync("Bồn nước"), UnitConst.MATERIAL_UNIT, 1);
            cleaningTask.AddTaskAttribute(null, await GetMaterialIdAsync("Xà phòng"), UnitConst.MATERIAL_UNIT, 1);
            cleaningTask.AddTaskAttribute(null, await GetMaterialIdAsync("Cọ rửa chai"), UnitConst.MATERIAL_UNIT, 1);
            cleaningTask.AddTaskAttribute(null, await GetMaterialIdAsync("Giá, kệ để chai"), UnitConst.MATERIAL_UNIT, 1);
            cleaningTask.AddTaskAttribute(null, await GetMaterialIdAsync(MaterialNames.AUTOCLAVE), UnitConst.MATERIAL_UNIT, 1);

            cleaningTask.AddTaskAttribute(await GetChemicalIdAsync(ChemicalNames.ETHANOL), null, UnitConst.CHEMICAL_UNIT, 70);
            cleaningTask.AddTaskAttribute(await GetChemicalIdAsync(ChemicalNames.NAOCL), null, UnitConst.CHEMICAL_UNIT, 1);
            cleaningTask.AddTaskAttribute(await GetChemicalIdAsync(ChemicalNames.DISTILLED_WATER), null, UnitConst.CHEMICAL_UNIT, 500);
            cleaningTask.AddTaskAttribute(await GetChemicalIdAsync(ChemicalNames.TWEEN20), null, UnitConst.CHEMICAL_UNIT, 2);

            cleaningTask.AddSingleCheckListItem(
                "Vệ sinh cơ học khu thao tác",
                "Lau/rửa bề mặt, khay dụng cụ và khu vực thao tác. Sử dụng: Găng tay y tế, Khay inox, Giấy lọc, Vòi nước, Bồn nước, Xà phòng, Cọ rửa chai, Giá kệ để chai.",
                1,
                null,
                null,
                null);

            cleaningTask.AddSingleCheckListItem(
                "Khử khuẩn bề mặt bằng Ethanol 70 - 75%",
                "Lau/phun Ethanol 70 - 75% lên bề mặt thao tác để khử khuẩn nhanh.",
                2,
                "%",
                70,
                75);

            cleaningTask.AddSingleCheckListItem(
                "Khử khuẩn tăng cường bằng NaOCL",
                "Xử lý các khu vực rủi ro cao bằng NaOCL theo quy trình an toàn phòng thí nghiệm.",
                3,
                UnitConst.CHEMICAL_UNIT,
                0.5m,
                1.5m);

            cleaningTask.AddSingleCheckListItem(
                "Tráng/rửa lại bằng nước cất vô trùng",
                "Tráng lại bề mặt hoặc dụng cụ khi cần để loại bỏ tồn dư hóa chất sau khử khuẩn.",
                4,
                "ml",
                300,
                700);

            cleaningTask.AddSingleCheckListItem(
                "Khử trùng dụng cụ tái sử dụng",
                "Chuẩn bị dụng cụ đã vệ sinh để hấp khử trùng bằng Nồi hấp (Autoclave) trước khi đưa về trạng thái sẵn sàng.",
                5,
                null,
                null,
                null);

            repository.TaskRepository.Add(cleaningTask);
            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    public sealed class ExperimentLogCompletedNotificationRepository(
        IExperimentLogRepository experimentLogRepository,
        IUserRepository userRepository,
        INotificationRepository notificationRepository,
        ITaskRepository taskRepository,
        IMaterialRepository materialRepository,
        IChemicalsRepository chemicalsRepository,
        IUnitOfWork unitOfWork)
    {
        public IExperimentLogRepository ExperimentLogRepository { get; } = experimentLogRepository;
        public IUserRepository UserRepository { get; } = userRepository;
        public INotificationRepository NotificationRepository { get; } = notificationRepository;
        public ITaskRepository TaskRepository { get; } = taskRepository;
        public IMaterialRepository MaterialRepository { get; } = materialRepository;
        public IChemicalsRepository ChemicalsRepository { get; } = chemicalsRepository;
        public IUnitOfWork UnitOfWork { get; } = unitOfWork;
    }

    public sealed class ExperimentLogCompletedNotificationService(INotificationPushService pushService)
    {
        public INotificationPushService NotificationPushService { get; } = pushService;
    }
}
