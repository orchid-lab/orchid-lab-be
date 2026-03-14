# Orchid Lab BE — Implementation Plan

> **Scope:** Bổ sung các feature còn thiếu so với đề đăng ký đồ án.  
> **Không làm:** SubcultureCount, FloweringInduction config, thêm SampleStageDefinition.  
> **Stack:** .NET 8, PostgreSQL, CQRS + MediatR, Scriban + Puppeteer (PDF).

---

## Phase 1 — Disease Incident Workflow

### Mục đích

AI (`AnalyzeOrchidImageCommand`) hiện chỉ trả kết quả phân tích và notify.  
Nhưng khi AI phát hiện bệnh, chưa có cơ chế để **nhân lực kiểm tra lại** và **ghi nhận hành động xử lý**.  
Phase này thêm `DiseaseIncident` (sự cố bệnh) và `DiseaseIncidentAction` (hành động xử lý) để khép vòng.

---

### Step 1.1 — Domain: thêm 2 entity mới

**File:** `orchid-backend-net.Domain/Entities/DiseaseIncident.cs`

```csharp
// Mục đích: Ghi nhận sự cố bệnh do AI phát hiện, chờ nhân lực xác nhận.
// Lifecycle: AIDetected → UnderReview → Confirmed / Dismissed
public class DiseaseIncident : AuditableEntity
{
    public required string SampleStageId { get; set; }
    [ForeignKey(nameof(SampleStageId))]
    public virtual SampleStage SampleStage { get; set; } = null!;

    public required string MonitoringLogId { get; set; }
    [ForeignKey(nameof(MonitoringLogId))]
    public virtual MonitoringLogs MonitoringLog { get; set; } = null!;

    public required int DiseaseId { get; set; }
    [ForeignKey(nameof(DiseaseId))]
    public virtual Disease Disease { get; set; } = null!;

    // Confidence score từ AI (0.0 - 1.0)
    public decimal AIConfidence { get; set; }

    public DiseaseIncidentStatus Status { get; set; }

    // Nhân lực điền vào sau khi kiểm tra thực tế
    public string? ReviewNote { get; set; }
    public string? ReviewedBy { get; set; }
    public DateTime? ReviewedAt { get; set; }

    public virtual List<DiseaseIncidentAction> Actions { get; set; } = new();

    // Domain methods
    public void ConfirmByHuman(string reviewerId, string? note)
    {
        if (Status != DiseaseIncidentStatus.AIDetected && Status != DiseaseIncidentStatus.UnderReview)
            throw new DomainException("Chỉ có thể xác nhận sự cố đang chờ review.");
        Status = DiseaseIncidentStatus.Confirmed;
        ReviewedBy = reviewerId;
        ReviewedAt = DateTime.UtcNow;
        ReviewNote = note;
    }

    public void DismissByHuman(string reviewerId, string reason)
    {
        if (Status != DiseaseIncidentStatus.AIDetected && Status != DiseaseIncidentStatus.UnderReview)
            throw new DomainException("Chỉ có thể dismiss sự cố đang chờ review.");
        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainException("Phải có lý do khi dismiss.");
        Status = DiseaseIncidentStatus.Dismissed;
        ReviewedBy = reviewerId;
        ReviewedAt = DateTime.UtcNow;
        ReviewNote = reason;
    }

    public void AddAction(string actionDescription, string performedBy)
    {
        if (Status != DiseaseIncidentStatus.Confirmed)
            throw new DomainException("Chỉ có thể thêm hành động cho sự cố đã xác nhận.");
        Actions.Add(new DiseaseIncidentAction
        {
            DiseaseIncidentId = ID,
            ActionDescription = actionDescription,
            PerformedBy = performedBy,
            PerformedAt = DateTime.UtcNow
        });
    }
}
```

**File:** `orchid-backend-net.Domain/Entities/DiseaseIncidentAction.cs`

```csharp
// Mục đích: Ghi từng bước hành động xử lý sau khi sự cố được xác nhận.
// Ví dụ: "Cách ly mẫu", "Phun thuốc Boocdo 1%", "Hủy mẫu nhiễm nặng"
public class DiseaseIncidentAction : BaseGuidEntity
{
    public required string DiseaseIncidentId { get; set; }
    [ForeignKey(nameof(DiseaseIncidentId))]
    public virtual DiseaseIncident DiseaseIncident { get; set; } = null!;

    public required string ActionDescription { get; set; }
    public required string PerformedBy { get; set; }
    public DateTime PerformedAt { get; set; }
    public string? Result { get; set; } // Kết quả sau hành động
}
```

**File:** `orchid-backend-net.Domain/Common/Enum/DiseaseIncidentStatus.cs`

```csharp
public enum DiseaseIncidentStatus
{
    AIDetected,    // AI vừa phát hiện, chưa có nhân lực review
    UnderReview,   // Researcher/Technician đang kiểm tra thực tế
    Confirmed,     // Nhân lực xác nhận đúng là bệnh
    Dismissed      // Nhân lực xác nhận AI phán đoán sai
}
```

---

### Step 1.2 — Domain: thêm IRepository interface

**File:** `orchid-backend-net.Domain/IRepositories/IDiseaseIncidentRepository.cs`

```csharp
// Mục đích: Interface chuẩn để Application layer gọi, không phụ thuộc Infrastructure.
public interface IDiseaseIncidentRepository : IEFRepository<DiseaseIncident>
{
    Task<DiseaseIncident?> FindWithDetailsAsync(
        string incidentId,
        CancellationToken cancellationToken);

    Task<List<DiseaseIncident>> FindBySampleStageAsync(
        string sampleStageId,
        CancellationToken cancellationToken);
}
```

---

### Step 1.3 — Application: tích hợp vào AnalyzeOrchidImageCommand

**File:** `orchid-backend-net.Application/MonitoringLog/UseCase/Analyze/AnalyzeOrchidImageCommand.cs`

Sửa phần sau khi save `analyticResultEntity`, thêm logic tạo `DiseaseIncident` nếu AI phát hiện bệnh:

```csharp
// Mục đích: Tự động tạo DiseaseIncident khi AI predict không phải "healthy"
// Chỉ chạy khi SampleStageId được cung cấp (có context mẫu vật cụ thể)
if (!string.IsNullOrWhiteSpace(request.SampleStageId)
    && !analyticDisease.Code.Equals("healthy", StringComparison.OrdinalIgnoreCase))
{
    var confidence = analyticResult.Disease.Probability
        .GetValueOrDefault(analyticResult.Disease.Predict, 0f);

    var incident = new DiseaseIncident
    {
        SampleStageId = request.SampleStageId,
        MonitoringLogId = monitoringLogId, // lấy từ context nếu có
        DiseaseId = analyticDisease.Id,
        AIConfidence = Convert.ToDecimal(confidence),
        Status = DiseaseIncidentStatus.AIDetected,
        CreatedBy = "system",
        CreatedDate = DateTime.UtcNow
    };
    diseaseIncidentRepository.Add(incident);
}
```

---

### Step 1.4 — Application: 3 use cases mới

**File:** `orchid-backend-net.Application/DiseaseIncident/UseCase/ReviewIncident/ReviewDiseaseIncidentCommand.cs`

```csharp
// Mục đích: Researcher/Technician xem xét thực tế và xác nhận hoặc bác bỏ phát hiện của AI.
// Kết quả Confirmed → sample có thể bị hủy hoặc cần hành động xử lý tiếp.
public record ReviewDiseaseIncidentCommand(
    string IncidentId,
    bool IsConfirmed,   // true = xác nhận bệnh thật, false = AI sai
    string? Note
) : IRequest<string>;
```

**File:** `orchid-backend-net.Application/DiseaseIncident/UseCase/AddAction/AddDiseaseIncidentActionCommand.cs`

```csharp
// Mục đích: Ghi lại hành động xử lý cụ thể sau khi sự cố được xác nhận.
// Ví dụ: Cách ly mẫu, phun thuốc, loại bỏ mẫu nhiễm.
public record AddDiseaseIncidentActionCommand(
    string IncidentId,
    string ActionDescription,
    string? Result
) : IRequest<string>;
```

**File:** `orchid-backend-net.Application/DiseaseIncident/UseCase/GetByExperimentLog/GetDiseaseIncidentsByExperimentLogQuery.cs`

```csharp
// Mục đích: Researcher xem toàn bộ sự cố bệnh trong một thí nghiệm,
// bao gồm những cái AI phát hiện chưa review và những cái đã xử lý.
public record GetDiseaseIncidentsByExperimentLogQuery(
    string ExperimentLogId,
    DiseaseIncidentStatus? StatusFilter
) : IRequest<List<DiseaseIncidentDto>>;
```

---

### Step 1.5 — Infrastructure và API

**File:** `orchid-backend-net.Infrastructure/Repository/DiseaseIncidentRepository.cs`
- Implement `IDiseaseIncidentRepository` kế thừa `RepositoryBase<DiseaseIncident>`

**File:** `orchid-backend-net.API/Controllers/DiseaseIncidentController.cs`

```csharp
// Endpoints:
// POST /api/disease-incidents/{id}/review    → ReviewDiseaseIncidentCommand
// POST /api/disease-incidents/{id}/actions   → AddDiseaseIncidentActionCommand
// GET  /api/disease-incidents?experimentLogId=...&status=... → GetDiseaseIncidentsByExperimentLogQuery
```

---

## Phase 2 — Analytics Queries

### Mục đích

Đề yêu cầu "hybrid success rates per parent combination" và "sample status over time".  
Hiện tại không có endpoint nào trả data dạng analytics — chỉ có CRUD detail.

---

### Step 2.1 — GetExperimentLogSummaryQuery

**File:** `orchid-backend-net.Application/ExperimentLog/UseCase/GetExperimentLogSummary/GetExperimentLogSummaryQuery.cs`

```csharp
// Mục đích: Dashboard tổng quan cho Researcher theo dõi một thí nghiệm cụ thể.
// Trả về: tỷ lệ sống, phân bố giai đoạn, số báo cáo chờ duyệt, sự cố bệnh.
public record GetExperimentLogSummaryQuery(string ExperimentLogId)
    : IRequest<ExperimentLogSummaryDto>;

internal class GetExperimentLogSummaryQueryHandler(
    IExperimentLogRepository experimentLogRepository)
    : IRequestHandler<GetExperimentLogSummaryQuery, ExperimentLogSummaryDto>
{
    public async Task<ExperimentLogSummaryDto> Handle(
        GetExperimentLogSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var el = await experimentLogRepository.FindAsync(
            e => e.ID == request.ExperimentLogId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy thí nghiệm.");

        var samples = el.Samples;
        var totalSamples = samples.Count;
        var aliveSamples = samples.Count(s => !s.ExecutionDate.HasValue);
        var infectedSamples = samples.Count(s => s.ExecutionDate.HasValue);

        // Phân bố theo giai đoạn sinh học hiện tại của mẫu đang sống
        var stageDistribution = samples
            .Where(s => !s.ExecutionDate.HasValue)
            .GroupBy(s => s.SampleStages
                .FirstOrDefault(st => st.Status == SampleStatus.InProgressed)
                ?.SampleStageDefinition.Name ?? "Unknown")
            .Select(g => new SampleStageDistributionDto
            {
                StageName = g.Key,
                SampleCount = g.Count(),
                Percentage = totalSamples > 0
                    ? Math.Round((double)g.Count() / totalSamples * 100, 1)
                    : 0
            })
            .ToList();

        // Monitoring logs pending
        var allMonitoringLogs = samples
            .SelectMany(s => s.SampleStages)
            .SelectMany(ss => ss.MonitoringLogs)
            .ToList();

        return new ExperimentLogSummaryDto
        {
            ExperimentLogId = el.ID,
            ExperimentLogName = el.Name,
            TotalSamples = totalSamples,
            ExpectedSamples = el.ExpectedSampleCount,
            AliveSamples = aliveSamples,
            InfectedSamples = infectedSamples,
            SurvivalRate = totalSamples > 0
                ? Math.Round((double)aliveSamples / totalSamples * 100, 1) : 0,
            ProgressRate = el.ExpectedSampleCount > 0
                ? Math.Round((double)aliveSamples / el.ExpectedSampleCount * 100, 1) : 0,
            StageDistribution = stageDistribution,
            TotalMonitoringLogs = allMonitoringLogs.Count,
            PendingApprovalLogs = allMonitoringLogs.Count(m =>
                m.Status == MonitoringLogStatus.WaitingForApproval
                || m.Status == MonitoringLogStatus.Revised),
            RejectedLogs = allMonitoringLogs.Count(m =>
                m.Status == MonitoringLogStatus.Rejected),
        };
    }
}
```

**File:** `orchid-backend-net.Application/ExperimentLog/Dto/ExperimentLog/ExperimentLogSummaryDto.cs`

```csharp
// Mục đích: DTO trả về cho dashboard analytics của Researcher.
public class ExperimentLogSummaryDto
{
    public string ExperimentLogId { get; set; } = default!;
    public string ExperimentLogName { get; set; } = default!;

    // Tổng quan mẫu vật
    public int TotalSamples { get; set; }
    public int ExpectedSamples { get; set; }
    public int AliveSamples { get; set; }
    public int InfectedSamples { get; set; }
    public double SurvivalRate { get; set; }       // % so với tổng đã tạo
    public double ProgressRate { get; set; }        // % so với mục tiêu ban đầu

    // Phân bố theo giai đoạn
    public List<SampleStageDistributionDto> StageDistribution { get; set; } = new();

    // Monitoring
    public int TotalMonitoringLogs { get; set; }
    public int PendingApprovalLogs { get; set; }
    public int RejectedLogs { get; set; }
}

public class SampleStageDistributionDto
{
    public string StageName { get; set; } = default!;
    public int SampleCount { get; set; }
    public double Percentage { get; set; }
}
```

---

### Step 2.2 — GetHybridSuccessRateQuery

**File:** `orchid-backend-net.Application/Seedling/UseCase/GetHybridSuccessRate/GetHybridSuccessRateQuery.cs`

```csharp
// Mục đích: So sánh tỷ lệ thành công giữa các cây giống bố mẹ và phương pháp lai.
// Phục vụ feature "hybrid success rates per parent combination" trong đề.
// "Thành công" = ExperimentLog có Status = Completed và tỷ lệ sống >= threshold.
public record GetHybridSuccessRateQuery(
    string? SeedlingParentId,   // Lọc theo cây mẹ cụ thể (optional)
    int? MethodId,              // Lọc theo phương pháp (optional)
    DateOnly? FromDate,
    DateOnly? ToDate
) : IRequest<List<HybridSuccessRateDto>>;

internal class GetHybridSuccessRateQueryHandler(
    IExperimentLogRepository experimentLogRepository)
    : IRequestHandler<GetHybridSuccessRateQuery, List<HybridSuccessRateDto>>
{
    public async Task<List<HybridSuccessRateDto>> Handle(
        GetHybridSuccessRateQuery request,
        CancellationToken cancellationToken)
    {
        var logs = await experimentLogRepository.FindAllAsync(
            el =>
                (request.SeedlingParentId == null || el.SeedlingParentId == request.SeedlingParentId) &&
                (request.MethodId == null || el.MethodId == request.MethodId) &&
                (request.FromDate == null || el.StartDate >= request.FromDate) &&
                (request.ToDate == null || el.EndDate <= request.ToDate),
            cancellationToken);

        return logs
            .GroupBy(el => new { el.SeedlingParentId, el.MethodId })
            .Select(g =>
            {
                var total = g.Count();
                var completed = g.Count(el => el.Status == ExperimentLogStatus.Completed);
                var avgSurvival = g
                    .Where(el => el.Samples.Count > 0)
                    .Select(el =>
                        (double)el.Samples.Count(s => !s.ExecutionDate.HasValue) / el.Samples.Count * 100)
                    .DefaultIfEmpty(0)
                    .Average();

                return new HybridSuccessRateDto
                {
                    SeedlingParentId = g.Key.SeedlingParentId,
                    SeedlingParentName = g.First().SeedlingParent?.LocalName ?? "Unknown",
                    MethodId = g.Key.MethodId,
                    MethodName = g.First().Method?.Name ?? "Unknown",
                    TotalExperiments = total,
                    CompletedExperiments = completed,
                    SuccessRate = total > 0
                        ? Math.Round((double)completed / total * 100, 1) : 0,
                    AverageSurvivalRate = Math.Round(avgSurvival, 1)
                };
            })
            .OrderByDescending(r => r.SuccessRate)
            .ToList();
    }
}
```

**File:** `orchid-backend-net.Application/Seedling/Dto/HybridSuccessRateDto.cs`

```csharp
// Mục đích: Kết quả so sánh tỷ lệ thành công để Researcher chọn tổ hợp lai tốt nhất.
public class HybridSuccessRateDto
{
    public string? SeedlingParentId { get; set; }
    public string SeedlingParentName { get; set; } = default!;
    public int MethodId { get; set; }
    public string MethodName { get; set; } = default!;
    public int TotalExperiments { get; set; }
    public int CompletedExperiments { get; set; }
    public double SuccessRate { get; set; }         // % hoàn thành
    public double AverageSurvivalRate { get; set; } // % mẫu sống trung bình
}
```

---

### Step 2.3 — Thêm 2 endpoints vào controller

**File:** `orchid-backend-net.API/Controllers/ExperimentLogController.cs` — bổ sung:

```csharp
// Mục đích: Expose summary analytics cho Researcher xem dashboard thí nghiệm.
[HttpGet("{id}/summary")]
[Authorize(Roles = "Researcher")]
[ProducesResponseType(typeof(ExperimentLogSummaryDto), StatusCodes.Status200OK)]
public async Task<IActionResult> GetSummary(
    [FromRoute] string id,
    CancellationToken cancellationToken)
    => Ok(await Sender.Send(new GetExperimentLogSummaryQuery(id), cancellationToken));
```

**File:** `orchid-backend-net.API/Controllers/SeedlingController.cs` — bổ sung:

```csharp
// Mục đích: Researcher so sánh hiệu quả giữa các tổ hợp cây mẹ và phương pháp lai.
[HttpGet("hybrid-success-rates")]
[Authorize(Roles = "Researcher")]
[ProducesResponseType(typeof(List<HybridSuccessRateDto>), StatusCodes.Status200OK)]
public async Task<IActionResult> GetHybridSuccessRate(
    [FromQuery] GetHybridSuccessRateQuery query,
    CancellationToken cancellationToken)
    => Ok(await Sender.Send(query, cancellationToken));
```

---

## Phase 3 — PDF Report

### Mục đích

Đề yêu cầu "export research results". Hiện tại `IPdfReportGenerator` nhận `object model` (quá generic).  
Phase này tách thành 2 report rõ ràng với 2 template HTML riêng.

---

### Step 3.1 — Refactor IPdfReportGenerator

**File:** `orchid-backend-net.Application/Common/Interfaces/IPdfReportGenerator.cs`

```csharp
// Mục đích: Tách interface thành 2 method rõ ràng thay vì object model generic.
// Không breaking change vì chưa có caller nào đang dùng GenerateAsync(object).
public interface IPdfReportGenerator
{
    // Report 1: Process Log — analytics theo thời gian, trạng thái mẫu, sự cố bệnh
    Task<byte[]> GenerateProcessLogAsync(
        ExperimentProcessLogReportModel model,
        CancellationToken cancellationToken = default);

    // Report 2: Summary — tổng kết thí nghiệm để nộp hội đồng / lưu hồ sơ
    Task<byte[]> GenerateSummaryReportAsync(
        ExperimentSummaryReportModel model,
        CancellationToken cancellationToken = default);
}
```

---

### Step 3.2 — Report Models

**File:** `orchid-backend-net.Application/ExperimentLog/Dto/Report/ExperimentProcessLogReportModel.cs`

```csharp
// Mục đích: Model cho Report 1 — Process Log.
// Chứa data analytics: timeline, trạng thái mẫu theo giai đoạn, AI results, sự cố bệnh.
public class ExperimentProcessLogReportModel
{
    // Header
    public string ExperimentName { get; set; } = default!;
    public string MethodName { get; set; } = default!;
    public string SeedlingLocalName { get; set; } = default!;
    public string SeedlingScientificName { get; set; } = default!;
    public string ResearcherName { get; set; } = default!;
    public string TechnicianName { get; set; } = default!;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string GeneratedAt { get; set; } = default!;

    // Section 1: Tổng quan mẫu vật
    public int TotalSamples { get; set; }
    public int ExpectedSamples { get; set; }
    public int AliveSamples { get; set; }
    public int InfectedSamples { get; set; }
    public double SurvivalRate { get; set; }

    // Section 2: Phân bố theo giai đoạn sinh học
    public List<SampleStageProgressItem> StageProgress { get; set; } = new();

    // Section 3: Timeline giai đoạn method (thực tế vs dự kiến)
    public List<MethodStageTimelineItem> MethodStageTimeline { get; set; } = new();

    // Section 4: AI analysis results
    public List<AIAnalysisItem> AIAnalysisResults { get; set; } = new();

    // Section 5: Disease incidents
    public List<DiseaseIncidentReportItem> DiseaseIncidents { get; set; } = new();

    // Section 6: Task summary
    public int TotalTasks { get; set; }
    public int TasksCompletedOnTime { get; set; }
    public int TasksCompletedLate { get; set; }
}

public class SampleStageProgressItem
{
    public string StageName { get; set; } = default!;
    public int SampleCount { get; set; }
    public double Percentage { get; set; }
}

public class MethodStageTimelineItem
{
    public int StageOrder { get; set; }
    public string StageName { get; set; } = default!;
    public int PlannedDays { get; set; }
    public int? ActualDays { get; set; }        // null nếu chưa hoàn thành
    public string Status { get; set; } = default!; // "Completed" | "InProgress" | "Pending"
}

public class AIAnalysisItem
{
    public string SampleName { get; set; } = default!;
    public string StageName { get; set; } = default!;
    public string DetectedDisease { get; set; } = default!;
    public double Confidence { get; set; }
    public string IncidentStatus { get; set; } = default!; // AIDetected / Confirmed / Dismissed
    public string AnalyzedAt { get; set; } = default!;
}

public class DiseaseIncidentReportItem
{
    public string SampleName { get; set; } = default!;
    public string DiseaseName { get; set; } = default!;
    public double AIConfidence { get; set; }
    public string IncidentStatus { get; set; } = default!;
    public string? ReviewNote { get; set; }
    public List<string> Actions { get; set; } = new();
}
```

**File:** `orchid-backend-net.Application/ExperimentLog/Dto/Report/ExperimentSummaryReportModel.cs`

```csharp
// Mục đích: Model cho Report 2 — Summary Report.
// Dùng để nộp hội đồng hoặc lưu hồ sơ lab. Có đủ Objective → Conclusion → Recommendations.
public class ExperimentSummaryReportModel
{
    // Cover
    public string ExperimentName { get; set; } = default!;
    public string MethodName { get; set; } = default!;
    public string SeedlingLocalName { get; set; } = default!;
    public string SeedlingScientificName { get; set; } = default!;
    public string ResearcherName { get; set; } = default!;
    public string TechnicianName { get; set; } = default!;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string GeneratedAt { get; set; } = default!;

    // Mục 1: Mục tiêu
    public string? Objective { get; set; }

    // Mục 2: Tóm tắt quy trình (timeline)
    public List<MethodStageTimelineItem> MethodStageTimeline { get; set; } = new();

    // Mục 3: Kết quả mẫu vật
    public int TotalSamples { get; set; }
    public int ExpectedSamples { get; set; }
    public int AliveSamples { get; set; }
    public int InfectedSamples { get; set; }
    public double SurvivalRate { get; set; }
    public List<SampleStageProgressItem> FinalStageDistribution { get; set; } = new();

    // Mục 4: Kết quả AI phân tích tóm tắt
    public int TotalAIScans { get; set; }
    public int DiseasesDetected { get; set; }
    public int DiseasesConfirmedByHuman { get; set; }
    public int DiseasesDismissedByHuman { get; set; }
    public List<string> TopDiseasesFound { get; set; } = new(); // top 3

    // Mục 5: Kết luận & Đề xuất
    public string? Conclusion { get; set; }
    public string? Issues { get; set; }
    public string? Recommendations { get; set; }

    // Mục 6: Xác nhận
    public string? ResearcherSignature { get; set; }
    public string CompletedDate { get; set; } = default!;
}
```

---

### Step 3.3 — Template HTML: Report 1 (Process Log)

**File:** `orchid-backend-net.Infrastructure/Service/PdfGenerator/Template/ExperimentProcessLog.html`

> Template dùng Scriban syntax (`{{ }}`). Thay thế `ExperimentReport.html` cũ hoặc giữ song song.

```html
<!DOCTYPE html>
<html lang="vi">
<head>
  <meta charset="UTF-8"/>
  <style>
    body { font-family: 'Segoe UI', sans-serif; margin: 24px; color: #1a1a1a; font-size: 13px; }
    h1 { font-size: 20px; text-align: center; color: #1a5c38; margin-bottom: 4px; }
    .subtitle { text-align: center; color: #555; font-size: 12px; margin-bottom: 24px; }
    h2 { font-size: 14px; color: #1a5c38; border-left: 4px solid #1a5c38;
         padding-left: 8px; margin-top: 28px; margin-bottom: 10px; }
    table { width: 100%; border-collapse: collapse; margin-bottom: 16px; }
    th { background: #e8f5ee; color: #1a5c38; font-weight: 600;
         padding: 7px 10px; text-align: left; font-size: 12px; border: 1px solid #c8e6d4; }
    td { padding: 6px 10px; border: 1px solid #e0e0e0; vertical-align: top; }
    tr:nth-child(even) td { background: #f9fffe; }
    .badge { display: inline-block; padding: 2px 8px; border-radius: 10px;
             font-size: 11px; font-weight: 600; }
    .badge-green { background: #d4edda; color: #1a5c38; }
    .badge-red   { background: #f8d7da; color: #8b1a1a; }
    .badge-amber { background: #fff3cd; color: #7a5c00; }
    .badge-gray  { background: #e9ecef; color: #444; }
    .stat-grid { display: grid; grid-template-columns: repeat(4, 1fr); gap: 12px; margin-bottom: 16px; }
    .stat-card { border: 1px solid #c8e6d4; border-radius: 6px; padding: 10px 14px; text-align: center; }
    .stat-num { font-size: 22px; font-weight: 700; color: #1a5c38; }
    .stat-label { font-size: 11px; color: #666; margin-top: 2px; }
    .footer { margin-top: 32px; border-top: 1px solid #ddd; padding-top: 10px;
              font-size: 11px; color: #888; text-align: right; }
  </style>
</head>
<body>

<h1>Nhật ký theo dõi thí nghiệm</h1>
<div class="subtitle">
  {{ experiment_name }} &nbsp;|&nbsp; {{ method_name }}
  &nbsp;|&nbsp; {{ start_date }} – {{ end_date }}
  &nbsp;|&nbsp; Xuất ngày: {{ generated_at }}
</div>

<!-- Header info -->
<table>
  <tr><th width="25%">Cây giống</th><td>{{ seedling_local_name }} ({{ seedling_scientific_name }})</td>
      <th width="25%">Researcher</th><td>{{ researcher_name }}</td></tr>
  <tr><th>Phương pháp</th><td>{{ method_name }}</td>
      <th>Kỹ thuật viên</th><td>{{ technician_name }}</td></tr>
</table>

<!-- Section 1: Tổng quan mẫu vật -->
<h2>1. Tổng quan mẫu vật</h2>
<div class="stat-grid">
  <div class="stat-card">
    <div class="stat-num">{{ total_samples }}</div>
    <div class="stat-label">Tổng mẫu tạo</div>
  </div>
  <div class="stat-card">
    <div class="stat-num" style="color:#1a5c38">{{ alive_samples }}</div>
    <div class="stat-label">Mẫu còn sống</div>
  </div>
  <div class="stat-card">
    <div class="stat-num" style="color:#c0392b">{{ infected_samples }}</div>
    <div class="stat-label">Mẫu nhiễm bệnh</div>
  </div>
  <div class="stat-card">
    <div class="stat-num" style="color:#1a5c38">{{ survival_rate }}%</div>
    <div class="stat-label">Tỷ lệ sống</div>
  </div>
</div>

<!-- Section 2: Phân bố giai đoạn -->
<h2>2. Phân bố mẫu theo giai đoạn sinh học</h2>
<table>
  <tr><th>Giai đoạn</th><th>Số mẫu</th><th>Tỷ lệ (%)</th></tr>
  {{ for s in stage_progress }}
  <tr>
    <td>{{ s.stage_name }}</td>
    <td>{{ s.sample_count }}</td>
    <td>{{ s.percentage }}%</td>
  </tr>
  {{ end }}
</table>

<!-- Section 3: Timeline phương pháp -->
<h2>3. Timeline thực hiện quy trình</h2>
<table>
  <tr><th>#</th><th>Giai đoạn</th><th>Dự kiến (ngày)</th><th>Thực tế (ngày)</th><th>Trạng thái</th></tr>
  {{ for s in method_stage_timeline }}
  <tr>
    <td>{{ s.stage_order }}</td>
    <td>{{ s.stage_name }}</td>
    <td>{{ s.planned_days }}</td>
    <td>{{ if s.actual_days }}{{ s.actual_days }}{{ else }}—{{ end }}</td>
    <td>
      {{ if s.status == "Completed" }}<span class="badge badge-green">Hoàn thành</span>
      {{ else if s.status == "InProgress" }}<span class="badge badge-amber">Đang thực hiện</span>
      {{ else }}<span class="badge badge-gray">Chưa bắt đầu</span>{{ end }}
    </td>
  </tr>
  {{ end }}
</table>

<!-- Section 4: AI Analysis -->
<h2>4. Kết quả AI phân tích mẫu</h2>
{{ if ai_analysis_results.size > 0 }}
<table>
  <tr><th>Mẫu</th><th>Giai đoạn AI nhận diện</th><th>Bệnh phát hiện</th>
      <th>Độ tin cậy</th><th>Trạng thái sự cố</th><th>Thời gian</th></tr>
  {{ for a in ai_analysis_results }}
  <tr>
    <td>{{ a.sample_name }}</td>
    <td>{{ a.stage_name }}</td>
    <td>{{ a.detected_disease }}</td>
    <td>{{ a.confidence }}%</td>
    <td>
      {{ if a.incident_status == "Confirmed" }}<span class="badge badge-red">Đã xác nhận</span>
      {{ else if a.incident_status == "Dismissed" }}<span class="badge badge-green">Bác bỏ (AI sai)</span>
      {{ else if a.incident_status == "AIDetected" }}<span class="badge badge-amber">Chờ review</span>
      {{ else }}<span class="badge badge-gray">{{ a.incident_status }}</span>{{ end }}
    </td>
    <td>{{ a.analyzed_at }}</td>
  </tr>
  {{ end }}
</table>
{{ else }}
<p style="color:#888;font-style:italic">Không có kết quả AI phân tích.</p>
{{ end }}

<!-- Section 5: Disease Incidents -->
<h2>5. Sự cố bệnh và hành động xử lý</h2>
{{ if disease_incidents.size > 0 }}
<table>
  <tr><th>Mẫu</th><th>Bệnh</th><th>Tin cậy AI</th><th>Trạng thái</th>
      <th>Ghi chú nhân lực</th><th>Hành động đã thực hiện</th></tr>
  {{ for d in disease_incidents }}
  <tr>
    <td>{{ d.sample_name }}</td>
    <td>{{ d.disease_name }}</td>
    <td>{{ d.ai_confidence }}%</td>
    <td>
      {{ if d.incident_status == "Confirmed" }}<span class="badge badge-red">Xác nhận</span>
      {{ else if d.incident_status == "Dismissed" }}<span class="badge badge-green">Bác bỏ</span>
      {{ else }}<span class="badge badge-amber">Chờ review</span>{{ end }}
    </td>
    <td>{{ d.review_note ?? "—" }}</td>
    <td>
      {{ if d.actions.size > 0 }}
      <ul style="margin:0;padding-left:16px">
        {{ for act in d.actions }}<li>{{ act }}</li>{{ end }}
      </ul>
      {{ else }}—{{ end }}
    </td>
  </tr>
  {{ end }}
</table>
{{ else }}
<p style="color:#888;font-style:italic">Không có sự cố bệnh được ghi nhận.</p>
{{ end }}

<!-- Section 6: Task summary -->
<h2>6. Tổng kết công việc</h2>
<table>
  <tr>
    <th>Tổng task</th><th>Hoàn thành đúng hạn</th><th>Hoàn thành trễ</th>
  </tr>
  <tr>
    <td>{{ total_tasks }}</td>
    <td><span class="badge badge-green">{{ tasks_completed_on_time }}</span></td>
    <td><span class="badge badge-amber">{{ tasks_completed_late }}</span></td>
  </tr>
</table>

<div class="footer">
  Orchid Research &amp; Lab Management System — DaLatOrchidLab &nbsp;|&nbsp;
  Xuất ngày {{ generated_at }}
</div>
</body>
</html>
```

---

### Step 3.4 — Template HTML: Report 2 (Summary Report)

**File:** `orchid-backend-net.Infrastructure/Service/PdfGenerator/Template/ExperimentSummaryReport.html`

```html
<!DOCTYPE html>
<html lang="vi">
<head>
  <meta charset="UTF-8"/>
  <style>
    body { font-family: 'Segoe UI', sans-serif; margin: 32px 40px; color: #1a1a1a; font-size: 13px; }
    .cover { text-align: center; padding: 40px 0 32px; border-bottom: 2px solid #1a5c38; margin-bottom: 32px; }
    .cover h1 { font-size: 22px; color: #1a5c38; margin-bottom: 8px; }
    .cover .lab { font-size: 13px; color: #888; margin-bottom: 16px; }
    .cover .meta-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 6px 24px;
                        max-width: 480px; margin: 0 auto; text-align: left; font-size: 12px; }
    .cover .meta-label { color: #666; }
    .cover .meta-val { font-weight: 600; }
    h2 { font-size: 15px; color: #1a5c38; margin-top: 28px; margin-bottom: 10px;
         padding-bottom: 4px; border-bottom: 1px solid #c8e6d4; }
    .section-box { background: #f9fffe; border: 1px solid #c8e6d4; border-radius: 6px;
                   padding: 14px 18px; margin-bottom: 16px; white-space: pre-wrap; line-height: 1.7; }
    table { width: 100%; border-collapse: collapse; margin-bottom: 16px; }
    th { background: #e8f5ee; color: #1a5c38; padding: 7px 10px;
         text-align: left; font-size: 12px; border: 1px solid #c8e6d4; }
    td { padding: 6px 10px; border: 1px solid #e0e0e0; }
    tr:nth-child(even) td { background: #f9fffe; }
    .stat-row { display: flex; gap: 16px; margin-bottom: 16px; }
    .stat-box { flex: 1; border: 1px solid #c8e6d4; border-radius: 6px; padding: 10px;
                text-align: center; }
    .stat-num { font-size: 24px; font-weight: 700; color: #1a5c38; }
    .stat-lbl { font-size: 11px; color: #666; }
    .badge { display:inline-block; padding:2px 8px; border-radius:10px; font-size:11px; font-weight:600; }
    .badge-green { background:#d4edda; color:#1a5c38; }
    .badge-amber { background:#fff3cd; color:#7a5c00; }
    .sign-area { margin-top: 48px; display: flex; justify-content: flex-end; }
    .sign-box { text-align: center; width: 200px; }
    .sign-line { border-top: 1px solid #333; margin-top: 48px; padding-top: 6px;
                 font-size: 12px; color: #444; }
    .footer { margin-top: 32px; border-top: 1px solid #ddd; padding-top: 8px;
              font-size: 11px; color: #aaa; text-align: center; }
  </style>
</head>
<body>

<!-- Cover -->
<div class="cover">
  <div class="lab">DaLatOrchidLab — Orchid Research &amp; Lab Management System</div>
  <h1>Báo cáo tổng kết thí nghiệm</h1>
  <div style="font-size:16px; font-weight:600; color:#333; margin-bottom:16px">
    {{ experiment_name }}
  </div>
  <div class="meta-grid">
    <span class="meta-label">Cây giống:</span>
    <span class="meta-val">{{ seedling_local_name }} ({{ seedling_scientific_name }})</span>
    <span class="meta-label">Phương pháp:</span>
    <span class="meta-val">{{ method_name }}</span>
    <span class="meta-label">Researcher:</span>
    <span class="meta-val">{{ researcher_name }}</span>
    <span class="meta-label">Kỹ thuật viên:</span>
    <span class="meta-val">{{ technician_name }}</span>
    <span class="meta-label">Thời gian:</span>
    <span class="meta-val">{{ start_date }} – {{ end_date }}</span>
    <span class="meta-label">Ngày xuất báo cáo:</span>
    <span class="meta-val">{{ generated_at }}</span>
  </div>
</div>

<!-- Mục 1: Mục tiêu -->
<h2>1. Mục tiêu thí nghiệm</h2>
<div class="section-box">{{ objective ?? "(Chưa ghi nhận)" }}</div>

<!-- Mục 2: Timeline quy trình -->
<h2>2. Tóm tắt quy trình thực hiện</h2>
<table>
  <tr><th>#</th><th>Giai đoạn</th><th>Dự kiến (ngày)</th>
      <th>Thực tế (ngày)</th><th>Trạng thái</th></tr>
  {{ for s in method_stage_timeline }}
  <tr>
    <td>{{ s.stage_order }}</td>
    <td>{{ s.stage_name }}</td>
    <td>{{ s.planned_days }}</td>
    <td>{{ if s.actual_days }}{{ s.actual_days }}{{ else }}—{{ end }}</td>
    <td>
      {{ if s.status == "Completed" }}<span class="badge badge-green">Hoàn thành</span>
      {{ else }}<span class="badge badge-amber">{{ s.status }}</span>{{ end }}
    </td>
  </tr>
  {{ end }}
</table>

<!-- Mục 3: Kết quả mẫu vật -->
<h2>3. Kết quả mẫu vật</h2>
<div class="stat-row">
  <div class="stat-box">
    <div class="stat-num">{{ total_samples }}</div>
    <div class="stat-lbl">Tổng mẫu tạo</div>
  </div>
  <div class="stat-box">
    <div class="stat-num">{{ expected_samples }}</div>
    <div class="stat-lbl">Mục tiêu ban đầu</div>
  </div>
  <div class="stat-box">
    <div class="stat-num" style="color:#1a5c38">{{ alive_samples }}</div>
    <div class="stat-lbl">Mẫu còn sống</div>
  </div>
  <div class="stat-box">
    <div class="stat-num" style="color:#c0392b">{{ infected_samples }}</div>
    <div class="stat-lbl">Mẫu nhiễm bệnh</div>
  </div>
  <div class="stat-box">
    <div class="stat-num">{{ survival_rate }}%</div>
    <div class="stat-lbl">Tỷ lệ sống</div>
  </div>
</div>
<table>
  <tr><th>Giai đoạn cuối của mẫu sống</th><th>Số mẫu</th><th>Tỷ lệ</th></tr>
  {{ for s in final_stage_distribution }}
  <tr>
    <td>{{ s.stage_name }}</td>
    <td>{{ s.sample_count }}</td>
    <td>{{ s.percentage }}%</td>
  </tr>
  {{ end }}
</table>

<!-- Mục 4: AI phân tích -->
<h2>4. Tóm tắt kết quả AI phân tích</h2>
<table>
  <tr><th>Chỉ số</th><th>Giá trị</th></tr>
  <tr><td>Tổng lần scan AI</td><td>{{ total_ai_scans }}</td></tr>
  <tr><td>Bệnh AI phát hiện</td><td>{{ diseases_detected }}</td></tr>
  <tr><td>Nhân lực xác nhận đúng</td><td>{{ diseases_confirmed_by_human }}</td></tr>
  <tr><td>Nhân lực bác bỏ (AI sai)</td><td>{{ diseases_dismissed_by_human }}</td></tr>
  <tr>
    <td>Bệnh phổ biến nhất</td>
    <td>
      {{ for d in top_diseases_found }}{{ d }}{{ if !for.last }}, {{ end }}{{ end }}
    </td>
  </tr>
</table>

<!-- Mục 5: Kết luận -->
<h2>5. Kết luận</h2>
<div class="section-box">{{ conclusion ?? "(Chưa ghi nhận)" }}</div>

<h2>6. Vấn đề gặp phải</h2>
<div class="section-box">{{ issues ?? "(Không có)" }}</div>

<h2>7. Đề xuất / Điều chỉnh</h2>
<div class="section-box">{{ recommendations ?? "(Không có)" }}</div>

<!-- Chữ ký -->
<div class="sign-area">
  <div class="sign-box">
    <div style="font-size:12px;color:#666;margin-bottom:4px">{{ completed_date }}</div>
    <div class="sign-line">{{ researcher_name }}<br/>Researcher</div>
  </div>
</div>

<div class="footer">
  Orchid Research &amp; Lab Management System — DaLatOrchidLab
</div>
</body>
</html>
```

---

### Step 3.5 — Use Cases và Endpoint Export

**File:** `orchid-backend-net.Application/ExperimentLog/UseCase/ExportReport/ExportExperimentReportCommand.cs`

```csharp
// Mục đích: Command duy nhất cho cả 2 loại report. type = "process" hoặc "summary".
// Handler build model từ DB rồi gọi IPdfReportGenerator tương ứng.
public record ExportExperimentReportCommand(
    string ExperimentLogId,
    string ReportType  // "process" | "summary"
) : IRequest<byte[]>;

internal class ExportExperimentReportCommandHandler(
    IExperimentLogRepository experimentLogRepository,
    IUserRepository userRepository,
    IPdfReportGenerator pdfReportGenerator)
    : IRequestHandler<ExportExperimentReportCommand, byte[]>
{
    public async Task<byte[]> Handle(
        ExportExperimentReportCommand request,
        CancellationToken cancellationToken)
    {
        var el = await experimentLogRepository.FindAsync(
            e => e.ID == request.ExperimentLogId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy thí nghiệm.");

        var researcher = await userRepository.FindAsync(
            u => u.ID == el.CreatedBy, cancellationToken);
        var technician = await userRepository.FindAsync(
            u => u.ID == el.AssignedTo, cancellationToken);

        var generatedAt = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        var researcherName = researcher?.FullName ?? el.CreatedBy;
        var technicianName = technician?.FullName ?? el.AssignedTo;

        if (request.ReportType.Equals("process", StringComparison.OrdinalIgnoreCase))
        {
            var model = BuildProcessLogModel(el, researcherName, technicianName, generatedAt);
            return await pdfReportGenerator.GenerateProcessLogAsync(model, cancellationToken);
        }
        else if (request.ReportType.Equals("summary", StringComparison.OrdinalIgnoreCase))
        {
            var model = BuildSummaryModel(el, researcherName, technicianName, generatedAt);
            return await pdfReportGenerator.GenerateSummaryReportAsync(model, cancellationToken);
        }

        throw new ArgumentException($"ReportType không hợp lệ: {request.ReportType}");
    }

    // Tách private helper methods để build từng model
    private static ExperimentProcessLogReportModel BuildProcessLogModel(...) { ... }
    private static ExperimentSummaryReportModel BuildSummaryModel(...) { ... }
}
```

**File:** `orchid-backend-net.API/Controllers/ExperimentLogController.cs` — bổ sung endpoint:

```csharp
// Mục đích: Download PDF report trực tiếp từ browser.
// Content-Disposition: attachment → trình duyệt tự download file.
[HttpGet("{id}/report")]
[Authorize(Roles = "Researcher")]
[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
public async Task<IActionResult> ExportReport(
    [FromRoute] string id,
    [FromQuery] string type,  // "process" | "summary"
    CancellationToken cancellationToken)
{
    var pdfBytes = await Sender.Send(
        new ExportExperimentReportCommand(id, type), cancellationToken);

    var fileName = type == "summary"
        ? $"summary-report-{id[..8]}.pdf"
        : $"process-log-{id[..8]}.pdf";

    return File(pdfBytes, "application/pdf", fileName);
}
```

---

## Tóm tắt file cần tạo / sửa

| Phase | File | Hành động |
|-------|------|-----------|
| 1 | `Domain/Entities/DiseaseIncident.cs` | Tạo mới |
| 1 | `Domain/Entities/DiseaseIncidentAction.cs` | Tạo mới |
| 1 | `Domain/Common/Enum/DiseaseIncidentStatus.cs` | Tạo mới |
| 1 | `Domain/IRepositories/IDiseaseIncidentRepository.cs` | Tạo mới |
| 1 | `Application/DiseaseIncident/UseCase/*` (3 files) | Tạo mới |
| 1 | `Application/MonitoringLog/UseCase/Analyze/AnalyzeOrchidImageCommand.cs` | Sửa |
| 1 | `Infrastructure/Repository/DiseaseIncidentRepository.cs` | Tạo mới |
| 1 | `API/Controllers/DiseaseIncidentController.cs` | Tạo mới |
| 2 | `Application/ExperimentLog/UseCase/GetExperimentLogSummary/*` | Tạo mới |
| 2 | `Application/ExperimentLog/Dto/ExperimentLog/ExperimentLogSummaryDto.cs` | Tạo mới |
| 2 | `Application/Seedling/UseCase/GetHybridSuccessRate/*` | Tạo mới |
| 2 | `Application/Seedling/Dto/HybridSuccessRateDto.cs` | Tạo mới |
| 2 | `API/Controllers/ExperimentLogController.cs` | Sửa (thêm endpoint) |
| 2 | `API/Controllers/SeedlingController.cs` | Sửa (thêm endpoint) |
| 3 | `Application/Common/Interfaces/IPdfReportGenerator.cs` | Sửa |
| 3 | `Application/ExperimentLog/Dto/Report/ExperimentProcessLogReportModel.cs` | Tạo mới |
| 3 | `Application/ExperimentLog/Dto/Report/ExperimentSummaryReportModel.cs` | Tạo mới |
| 3 | `Application/ExperimentLog/UseCase/ExportReport/ExportExperimentReportCommand.cs` | Tạo mới |
| 3 | `Infrastructure/Service/PdfGenerator/Template/ExperimentProcessLog.html` | Tạo mới |
| 3 | `Infrastructure/Service/PdfGenerator/Template/ExperimentSummaryReport.html` | Tạo mới |
| 3 | `Infrastructure/Service/PdfGenerator/PdfReportGenerator.cs` | Sửa |
| 3 | `API/Controllers/ExperimentLogController.cs` | Sửa (thêm endpoint export) |
