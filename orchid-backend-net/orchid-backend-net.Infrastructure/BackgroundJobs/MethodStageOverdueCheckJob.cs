using Microsoft.Extensions.Logging;
using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.Notification.Helper;
using orchid_backend_net.Domain.Common.Enum;
using orchid_backend_net.Domain.IRepositories;

namespace orchid_backend_net.Infrastructure.BackgroundJobs
{
    /// <summary>
    /// Background job to check for overdue method stages in experiment logs.
    /// Sends notifications to researchers about delayed biological growth.
    /// </summary>
    public class MethodStageOverdueCheckJob
    {
        private readonly IExperimentLogRepository _experimentLogRepository;
        private readonly IMethodRepository _methodRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationPushService _pushService;
        private readonly ILogger<MethodStageOverdueCheckJob> _logger;

        public MethodStageOverdueCheckJob(
            IExperimentLogRepository experimentLogRepository,
            IMethodRepository methodRepository,
            INotificationRepository notificationRepository,
            INotificationPushService pushService,
            ILogger<MethodStageOverdueCheckJob> logger)
        {
            _experimentLogRepository = experimentLogRepository;
            _methodRepository = methodRepository;
            _notificationRepository = notificationRepository;
            _pushService = pushService;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                _logger.LogInformation("Starting method stage overdue check at {Time}", DateTime.UtcNow);

                // Lấy tất cả experiment logs đang InProgress
                var activeExperiments = await _experimentLogRepository.FindAllAsync(
                    e => e.Status == ExperimentLogStatus.InProgress,
                    cancellationToken: default);

                if (activeExperiments.Count == 0)
                {
                    _logger.LogInformation("No active experiments to check.");
                    return;
                }

                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                var overdueExperiments = new List<(string ExperimentId, string ResearcherId, string ExperimentName, int OverdueDays)>();

                foreach (var experiment in activeExperiments)
                {
                    if (!experiment.StartDate.HasValue)
                        continue;

                    var method = await _methodRepository.FindAsync(m => m.ID == experiment.MethodId, default);
                    if (method is null)
                        continue;

                    var currentStage = method.MethodStages
                        .FirstOrDefault(ms => ms.Order == experiment.CurrentStageOrder);

                    if (currentStage is null)
                        continue;

                    // Sửa: Dùng DayNumber để tính khoảng cách giữa 2 DateOnly
                    var daysSinceStart = today.DayNumber - experiment.StartDate.Value.DayNumber;

                    // Tính tổng duration từ stage 1 đến stage hiện tại
                    var expectedDaysUpToCurrent = method.MethodStages
                        .Where(ms => ms.Order <= experiment.CurrentStageOrder)
                        .Sum(ms => ms.DurationsDays);

                    if (daysSinceStart > expectedDaysUpToCurrent)
                    {
                        var overdueDays = daysSinceStart - expectedDaysUpToCurrent;
                        
                        // Sửa: Đúng thứ tự các field trong tuple
                        overdueExperiments.Add((
                            ExperimentId: experiment.ID,
                            ResearcherId: experiment.CreatedBy,
                            ExperimentName: experiment.Name,
                            OverdueDays: overdueDays));
                    }
                }

                // Gửi notification cho từng researcher
                foreach (var overdue in overdueExperiments)
                {
                    var title = "Thí nghiệm quá hạn giai đoạn";
                    var content = $"Thí nghiệm '{overdue.ExperimentName}' đã quá hạn {overdue.OverdueDays} ngày so với thời gian dự kiến. " +
                                  $"Tốc độ sinh trưởng của mẫu có thể chậm hơn dự tính. Vui lòng kiểm tra và đánh giá.";

                    var notification = CreateNotificationHelper.CreateForSingleUsers(
                        overdue.ResearcherId, 
                        title, 
                        content,
                        Domain.Common.Enum.NotificationTargetType.ExperimentLog,
                        overdue.ExperimentId);
                    
                    _notificationRepository.Add(notification);

                    await _pushService.PushToSingleUserAsync(overdue.ResearcherId, title, content);

                    _logger.LogWarning(
                        "Experiment {ExperimentId} overdue by {Days} days. Notification sent to researcher {ResearcherId}.",
                        overdue.ExperimentId,
                        overdue.OverdueDays,
                        overdue.ResearcherId);
                }

                if (overdueExperiments.Count > 0)
                {
                    await _notificationRepository.UnitOfWork.SaveChangesAsync(default);
                }

                _logger.LogInformation("Method stage overdue check completed. Found {Count} overdue experiments.", 
                    overdueExperiments.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during method stage overdue check.");
            }
        }
    }
}