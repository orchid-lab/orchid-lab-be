using orchid_backend_net.Domain.Entities;
using System.Globalization;

namespace orchid_backend_net.Application.MonitoringLog.Helper
{
    /// <summary>
    /// Maps ONNX analysis results to database AnalyticResults entity.
    /// Handles stage validation and disease code conversion.
    /// </summary>
    public static class OrchidAnalysisMapper
    {
        /// <summary>
        /// Valid stage names from ONNX model output.
        /// </summary>
        private static readonly HashSet<string> ValidStageNames = new(StringComparer.OrdinalIgnoreCase)
        {
            "Coppice",  // Giai đoạn chồi non
            "Tissue",   // Giai đoạn mô nuôi cấy
            "Tree"      // Giai đoạn cây trưởng thành
        };

        /// <summary>
        /// Disease code mapping: ONNX output name → Database Disease.Code.
        /// Used for Disease table lookup.
        /// </summary>
        private static readonly Dictionary<string, string> DiseaseCodeMapping = new()
        {
            // ONNX prediction name → Disease.Code in database
            { "Anthracnose", "disease_anthracnose" },
            { "BacterialWilt", "disease_bacterial_wilt" },
            { "Blackrot", "disease_blackrot" },
            { "Brownspots", "disease_brownspots" },
            { "MoldBacterial", "disease_mold_bac" },
            { "MoldFungus", "disease_mold_fungus" },
            { "SoftRot", "disease_soft_rot" },
            { "StemRot", "disease_stemrot" },
            { "WitheredYellowRoot", "disease_withered_yellow_root" },
            { "Healthy", "healthy" },
            { "Oxidation", "oxidation" },
            { "Virus", "virus" }
        };

        /// <summary>
        /// Map ONNX analysis result to AnalyticResults entity.
        /// </summary>
        /// <param name="source">OrchidAnalysisResult from ONNX service</param>
        /// <returns>AnalyticResults entity ready for database insertion</returns>
        /// <exception cref="ArgumentNullException">If source is null</exception>
        /// <exception cref="ArgumentException">If disease data is missing</exception>
        // Thay method ToAnalyticResult cũ bằng cái này
public static AnalyticResults ToAnalyticResult(OrchidAnalysisResult source)
{
    ArgumentNullException.ThrowIfNull(source);

    if (source.Disease is null)
        throw new ArgumentException("Kết quả phân tích bệnh bị thiếu", nameof(source));

    var probabilities = source.Disease.Probability ?? new Dictionary<string, float>();

    // Convert float → decimal, lọc NaN/Infinity
    var predictions = probabilities.ToDictionary(
        kvp => kvp.Key,
        kvp => (float.IsNaN(kvp.Value) || float.IsInfinity(kvp.Value))
            ? 0m
            : Convert.ToDecimal(kvp.Value, CultureInfo.InvariantCulture)
    );

    // Tìm bệnh có xác suất cao nhất
    var topEntry = predictions.OrderByDescending(x => x.Value).FirstOrDefault();

    return new AnalyticResults
    {
        PredictionsJson = System.Text.Json.JsonSerializer.Serialize(predictions),
        TopDisease = topEntry.Key ?? "Unknown",
        Confidence = topEntry.Value,
        AnalyzedAt = DateTime.UtcNow
    };
}

        /// <summary>
        /// Validate and normalize stage name from ONNX output.
        /// </summary>
        /// <param name="stageName">Stage name from ONNX (e.g., "Coppice", "Tissue", "Tree")</param>
        /// <returns>Normalized stage name with proper casing</returns>
        /// <exception cref="ArgumentException">If stage name is empty or invalid</exception>
        public static string ValidateStageName(string stageName)
        {
            if (string.IsNullOrWhiteSpace(stageName))
                throw new ArgumentException("Stage name cannot be empty", nameof(stageName));

            // Check if stage is valid (case-insensitive)
            if (!ValidStageNames.Contains(stageName))
            {
                var validStages = string.Join(", ", ValidStageNames);
                throw new ArgumentException(
                    $"Invalid stage: '{stageName}'. Valid stages: {validStages}", 
                    nameof(stageName));
            }

            // Return normalized stage name (proper casing from the set)
            return ValidStageNames.First(s => s.Equals(stageName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Convert ONNX disease prediction to Disease.Code for database lookup.
        /// </summary>
        /// <param name="onnxDiseaseName">Disease name from ONNX (e.g., "Anthracnose")</param>
        /// <returns>Disease code for database lookup (e.g., "disease_anthracnose")</returns>
        /// <exception cref="ArgumentException">If disease name is empty or unknown</exception>
        public static string ToDiseaseCode(string onnxDiseaseName)
        {
            if (string.IsNullOrWhiteSpace(onnxDiseaseName))
                throw new ArgumentException("Disease name cannot be empty", nameof(onnxDiseaseName));

            // Try exact match first
            if (DiseaseCodeMapping.TryGetValue(onnxDiseaseName, out var code))
                return code;

            // Fallback: try case-insensitive match
            var match = DiseaseCodeMapping.FirstOrDefault(x => 
                x.Key.Equals(onnxDiseaseName, StringComparison.OrdinalIgnoreCase));
            
            if (match.Value != null)
                return match.Value;

            // No match found - throw descriptive error
            var availableDiseases = string.Join(", ", DiseaseCodeMapping.Keys);
            throw new ArgumentException(
                $"Unknown disease: '{onnxDiseaseName}'. Available: {availableDiseases}", 
                nameof(onnxDiseaseName));
        }
    }
}
