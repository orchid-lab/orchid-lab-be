using orchid_backend_net.Domain.Entities;
using System.Globalization;

namespace orchid_backend_net.Application.MonitoringLog.Helper
{
    /// <summary>
    /// this helper is used to map orchid analysis result to analytic results entity
    /// </summary>
    public static class OrchidAnalysisMapper
    {

        /// <summary>
        /// map the python api result to analytic results entity
        /// </summary>
        /// <param name="source">the result after using OrchidAnalyzerService</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static AnalyticResults ToAnalyticResult(OrchidAnalysisResult source)
        {
            ArgumentNullException.ThrowIfNull(source);
            if (source.Disease is null) throw new ArgumentException("Kết quả phân tích bệnh bị thiếu", nameof(source));
            var p = source.Disease.Probability ?? new Dictionary<string, float>();


            //helper to safely get and convert float -> decimal 
            decimal Get(string key)
            {
                if (p.TryGetValue(key, out var value))
                {
                    if (float.IsNaN(value) || float.IsInfinity(value))
                        return 0m;
                    return Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                }
                return 0m;
            }

            return new AnalyticResults
            {
                Anthracnose = Get("disease_anthracnose"),
                BacterialWilt = Get("disease_bacterial_wilt"),
                Blackrot = Get("disease_blackrot"),
                Brownspots = Get("disease_brownspots"),
                MoldBacterial = Get("disease_mold_bac"),
                MoldFungus = Get("disease_mold_fungus"),
                SoftRot = Get("disease_soft_rot"),
                StemRot = Get("disease_stemrot"),
                WitheredYellowRoot = Get("disease_withered_yellow_root"),
                Healthy = Get("healthy"),
                Oxidation = Get("oxidation"),
                Virus = Get("virus"),
            };
        }
    }
}
